using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace CalculationApi.Tests
{
    public class CalculationControllerTests
    {
        private readonly HttpClient _client;

        public CalculationControllerTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5005")
            };
        }

        [Fact]
        public async Task Test_Calculate_Success()
        {
            var requestBody = new
            {
                Number1 = 10,
                Number2 = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiem9oYXJhIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NDY0NjQxODJ9.QbYX59Y6j_ss3T39E_xc8KHWjkwGASGdipunnKVoHUs");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Calculation")
            {
                Content = content
            };
            request.Headers.Add("Operator", "add");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<double>(responseContent);

            Console.WriteLine($"Result: {result}, Raw Response: {responseContent}");

            Assert.Equal(15, result);
        }

        [Fact]
        public async Task Test_Calculate_Unauthorized()
        {
            var requestBody = new
            {
                number1 = 10,
                number2 = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Calculation?Operator=add", content);

            Assert.Equal(401, (int)response.StatusCode);
        }
    }
}
