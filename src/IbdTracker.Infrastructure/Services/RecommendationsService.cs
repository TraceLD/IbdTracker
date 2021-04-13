using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Config;
using IbdTracker.Core.Recommendations;

namespace IbdTracker.Infrastructure.Services
{
    /// <summary>
    /// Service for interacting with the Python recommendations microservice over localhost.
    /// </summary>
    public interface IRecommendationsService
    {
        /// <summary>
        /// Obtains the recommendations for food items.
        /// </summary>
        /// <param name="recommendationData">Data needed to compute recommendations.</param>
        /// <returns>Recommendations for food items.</returns>
        Task<IEnumerable<FoodItemRecommendation>> GetFoodItemRecommendations(
            IEnumerable<FoodItemRecommendationData> recommendationData);
    }
    
    /// <inheritdoc />
    public class RecommendationsService : IRecommendationsService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Instantiates a new instance of <see cref="RecommendationsService"/>.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/>.</param>
        /// <param name="config">Python microservice configuration.</param>
        public RecommendationsService(HttpClient httpClient, PythonMicroserviceConfig config)
        {
            httpClient.BaseAddress = new Uri(config.RecommendationsMicroserviceIp);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "ASP.NET Core Kestrel");

            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FoodItemRecommendation>> GetFoodItemRecommendations(IEnumerable<FoodItemRecommendationData> recommendationData)
        {
            var response =
                await _httpClient.PostAsJsonAsync("/recommendations/fi", recommendationData, _serializerOptions);
            
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<FoodItemRecommendation>>(responseStream,
                _serializerOptions) ?? Enumerable.Empty<FoodItemRecommendation>();
        }
    }
}