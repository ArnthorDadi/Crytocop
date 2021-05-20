using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

namespace Cryptocop.Software.API.Middleware
{
    public static class JwtAuthenticationMiddleware
    {
        public static AuthenticationBuilder AddJwtTokenAuthentication(this AuthenticationBuilder builder, IConfiguration config)
        {
            var jwtConfig = config.GetSection("JwtConfig");
            var secret = jwtConfig.GetValue<string>("secret");
            var issuer = jwtConfig.GetValue<string>("issuer");
            var audience = jwtConfig.GetValue<string>("audience");
            var key = Encoding.ASCII.GetBytes(secret);

            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x => 
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    NameClaimType = "name" /// User.Identity.Name
                };
                x.Events = new JwtBearerEvents{
                    OnTokenValidated = async context =>
                    {
                        var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == "tokenId").Value;
                        int.TryParse(claim, out var tokenId);
                        var accountService = context.HttpContext.RequestServices.GetService<IJwtTokenService>();

                        if(accountService.IsTokenBlacklisted(tokenId))
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("JWT token provided is invalid.");
                        }
                    }
                };
            });
            return builder;
        }
    }
}