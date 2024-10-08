using Microsoft.AspNetCore.Http;
using StocksApp_Whole.ServiceContracts;
using System.Text.Json;

namespace StocksApp_Whole.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubAPI"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();

                Dictionary<string, object>? profileDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (profileDictionary == null || profileDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException("No data available.");
                }

                return profileDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubAPI"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();

                Dictionary<string, object>? stockDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (stockDictionary == null || stockDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException("No data available.");
                }

                return stockDictionary;
            }
        }

    }
}
