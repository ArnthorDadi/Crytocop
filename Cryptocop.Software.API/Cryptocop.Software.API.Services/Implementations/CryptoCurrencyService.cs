using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
        {
            HttpClient client = new HttpClient();
            
            string URL = "https://data.messari.io/api/v2/assets?fields=id,name,slug,symbol,metrics/market_data/price_usd,profile/general/overview/project_details";
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
            var dezerializedData = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptocurrencyDto>(response, true);
            
            List<CryptocurrencyDto> returnList = new List<CryptocurrencyDto> ();
            
            List<string> allowedCrypto = new List<string>{
                "BTC", "ETH", "USDT", "XMR"
            };

            foreach(var data in dezerializedData)
            {
                if(allowedCrypto.Contains(data.Symbol))
                {
                    returnList.Add(data);
                }
            }
            return returnList;
        }

        public async Task<CryptocurrencyDto> GetCryptoToUsd(string product_identifier)
        {
            HttpClient client = new HttpClient();
            
            string URL = "https://data.messari.io/api/v1/assets/" + product_identifier + "/metrics?fields=market_data/price_usd";
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
            var dezerializedData = await HttpResponseMessageExtensions.DeserializeJsonToObject<CryptocurrencyDto>(response, true);
            return dezerializedData;
        }
    }
}