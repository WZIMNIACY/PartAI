using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wzimniacy2;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // ===============================
        // 1. Wybor modelu
        // ===============================
        Console.WriteLine("Wybierz model:");
        Console.WriteLine("1. Local LLM (np. LM Studio, Ollama)");
        Console.WriteLine("2. Google AI Studio (Gemini)");
        Console.Write("Opcja: ");
        string opt = Console.ReadLine();

        ILLM llm;

        if (opt == "1")
        {
            Console.Write("Podaj nazwę modelu lokalnego (np. llama-3.1): ");
            string model = Console.ReadLine();

            // POPRAWKA: ustawiamy właściwość ModelName zamiast wywoływać nieistniejący konstruktor
            llm = new LocalLLM { ModelName = model };

            Console.WriteLine($"Wybrano lokalny model: {model}\n");
        }
        else
        {
            Console.Write("Wklej swój Google API Key: ");
            string apiKey = Console.ReadLine();

            // ApiLLM ma konstruktor przyjmujący apiKey w wersji, którą udostępniałem
            llm = new ApiLLM(apiKey);
            Console.WriteLine("Wybrano Google Gemini.\n");
        }

        // ===============================
        // 2. Tworzenie planszy Tajniaków
        // ===============================
        var engine = new GameEngine();
        var board = engine.CreateNewBoard("wekory.json");

        Console.WriteLine("Wylosowana plansza (25 słów):");
        int i = 1;
        foreach (var c in board)
        {
            Console.WriteLine($"{i++:00}. {c.Word,-12} [{c.Team}]");
        }

        // ===============================
        // 3. Wybór drużyny gracza
        // ===============================
        Console.WriteLine("\nWybierz drużynę:");
        Console.WriteLine("1. Czerwona");
        Console.WriteLine("2. Niebieska");
        Console.Write("Opcja: ");
        string teamOpt = Console.ReadLine();

        Team currentTeam = teamOpt == "1" ? Team.Red : Team.Blue;

        // ===============================
        // 4. Przygotowanie prompta
        // ===============================
        var myWords = board.Where(c => c.Team == currentTeam).Select(c => c.Word).ToList();
        var enemyWords = board.Where(c => c.Team != currentTeam && c.Team != Team.Assassin && c.Team != Team.Neutral).Select(c => c.Word).ToList();
        var neutralWords = board.Where(c => c.Team == Team.Neutral).Select(c => c.Word).ToList();
        var assassinWord = board.First(c => c.Team == Team.Assassin).Word;

        string prompt = File.ReadAllText("prompt.txt")
                            .Replace("{CurrentTeamColor}", currentTeam == Team.Red ? "CZERWONA" : "NIEBIESKA")
                            .Replace("{MyWords}", string.Join(", ", myWords))
                            .Replace("{EnemyWords}", string.Join(", ", enemyWords))
                            .Replace("{NeutralWords}", string.Join(", ", neutralWords))
                            .Replace("{AssassinWord}", assassinWord);

        Console.WriteLine("\n--- WYSYŁAM PROMPT DO MODELU ---\n");
        Console.WriteLine(prompt);
        Console.WriteLine("\n--------------------------------\n");

        // ===============================
        // 5. Zapytanie do modelu
        // ===============================
        string answer = await llm.Ask(prompt);

        // ===============================
        // 6. Wynik końcowy
        // ===============================
        Console.WriteLine("\n===== WSKAZÓWKA MISTRZA GRY =====");
        Console.WriteLine(answer);
        Console.WriteLine("=================================\n");
    }
}

