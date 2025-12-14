using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TajniacyAI
{
    internal class LLM
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
       
        private const string Endpoint = "https://api.deepseek.com/chat/completions";

        public LLM(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30); 
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GenerujTekst(string systemPrompt, string userPrompt)
        {
            var payload = new
            {
                model = "deepseek-chat", 
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 1.3, 
                max_tokens = 150
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(Endpoint, content);

                if (!response.IsSuccessStatusCode)
                    return $"[Błąd API: {response.StatusCode}] Tępe ch*je, łącze zerwało!";

                var responseString = await response.Content.ReadAsStringAsync();

                
                using var doc = JsonDocument.Parse(responseString);
                var text = doc.RootElement
                              .GetProperty("choices")[0]
                              .GetProperty("message")
                              .GetProperty("content")
                              .GetString();

                return text ?? "Bomba zaniemówił.";
            }
            catch (Exception ex)
            {
                return $"[Błąd krytyczny: {ex.Message}] Kurvinox uszkodził kabel!";
            }
        }
    }
}