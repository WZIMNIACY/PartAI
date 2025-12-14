using System.Text.Json;
using System.Text;

public class LocalLLM : ILLM
{
    public string ModelName { get; set; } = "";
    private static readonly HttpClient client = new HttpClient();
    private const string ApiUrl = "http://localhost:11434/v1/chat/completions";

    public async Task<string> Ask(string prompt)
    {
        // tworzenie payload dla modelu lokalnego
        var payload = new
        {
            model = ModelName,
            messages = new[]
            {
                new { role = "system", content = "Jesteś mistrzem gry Tajniacy." },
                new { role = "user", content = prompt }
            },
            temperature = 0.3
        };

        string json = JsonSerializer.Serialize(payload);
        var response = await client.PostAsync(ApiUrl, new StringContent(json, Encoding.UTF8, "application/json"));
        var respJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(respJson);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();  
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ??"";
    }
}