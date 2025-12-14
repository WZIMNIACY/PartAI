using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiLLM : ILLM
{
    private readonly string _apiKey;
    private static readonly HttpClient _client = new HttpClient();
    private const string ApiUrl = "[https://api.deepsika.ai/v1/generate](https://api.deepsika.ai/v1/generate)"; // przykładowy endpoint

    public string ModelName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ApiLLM(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public async Task<string> Ask(string prompt)
    {
        var payload = new
        {
            prompt = prompt,
            max_tokens = 200,
            temperature = 0.3
        };

        string json = JsonSerializer.Serialize(payload);
        var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string respJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(respJson);
            // zakładam, że DeepSika zwraca pole "text" w pierwszym elemencie "choices"
            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("text")
                      .GetString()
                      ?.Trim() ?? "";
        }
        catch (Exception ex)
        {
            return $"Błąd API: {ex.Message}";
        }
    }


}
