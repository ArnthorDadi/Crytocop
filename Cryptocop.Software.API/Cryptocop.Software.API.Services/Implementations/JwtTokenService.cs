using System.Linq;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly CryptocopDbContext _dbContext;

        public JwtTokenService(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            var token = _dbContext.JwtTokens.FirstOrDefault(t => t.Id == tokenId);
            if (token == null) { return true; }
            return token.Blacklisted;
        }
    }
}