using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.Envelope;
using System.Collections.Generic;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            HttpClient client = new HttpClient();
            
            string URL = "https://data.messari.io/api/v2/assets?fields=id,name,slug,symbol,metrics/market_data/price_usd,metrics/market_data/last_trade_at";
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("").Result;
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }
            client.Dispose();
            
            var dezerializedData = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(response, true);
            //return dezerializedData;
            var envelope = new Envelope<ExchangeDto>(pageNumber, 5, dezerializedData);

            return envelope;
        }
    }
}