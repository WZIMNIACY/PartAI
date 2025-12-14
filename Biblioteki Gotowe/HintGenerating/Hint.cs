using System.Text.Json;
using System.Text.Json.Serialization;
using FileOperations;
using game;
using System;

namespace hints
{
    [Serializable]
    public sealed class Hint
    {
        [JsonPropertyName("Word")]
        public string Word { get; set; }
        [JsonPropertyName("NOSW")]
        public int NoumberOfSimilarWords { get; set; }

        [JsonPropertyName("Cards")]
        public List<Card>? Cards { get; set; }

        [JsonConstructor]
        public Hint(string word, List<Card>? cards, int noumberOfSimilarWords)
        {
            this.Word = word;
            this.NoumberOfSimilarWords = noumberOfSimilarWords;
            this.Cards = cards;
        }

        public static Hint Create(Deck deck, LLM llm, Team nowTour)
        {
            //Prompt set up
            string _nowTour = nowTour.ToString();
            string _actualDeck = deck.ToJson();
            string hintPromptPath = Path.Combine(Directory.GetCurrentDirectory(), "hintPrompt.txt");
            string systemPromptHint = FileOp.Read(hintPromptPath);
            string userPromptHint = $"" +
                $"_nowTour = {_nowTour}\n" +
                $"_actualDeck = {_actualDeck}";

            //Genereting hint

            while (true)
            {
                Console.WriteLine("Generating hint...");
                string response = llm.SendRequestAsync(systemPromptHint, userPromptHint).Result;
                Hint hint = Hint.FromJson(response);

                bool hintInDeck = deck.Cards.Any(card => card.Word.Equals(hint.Word, StringComparison.OrdinalIgnoreCase));
                bool hintIsEmpty = string.IsNullOrWhiteSpace(hint.Word);
                bool teamOK = hint.Cards != null && hint.Cards.All(card => card.Team == nowTour.ToString());

                if (hintInDeck || hintIsEmpty || !teamOK)
                {
                    Console.WriteLine("Generated hint is invalid, regenerating...");
                    continue;
                }
                else
                {
                    return hint;
                }



            }
        }

        public string toJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            string json = JsonSerializer.Serialize(this, options);

            if (json == "" || json == " " || json == null)
                throw new HintException("HITN Json serialization ERROR!");
            else
                return json;
        }

        public static Hint FromJson(string jsonFormat)
        {
            if (string.IsNullOrWhiteSpace(jsonFormat))
                throw new HintException("Input jsonFormat is null or whitespace.");


            int start = jsonFormat.IndexOf('{');
            int end = jsonFormat.LastIndexOf('}');
            if (start == -1 || end == -1 || end < start)
                throw new HintException("Input does not contain a valid JSON object.");

            string trimmed = jsonFormat.Substring(start, end - start + 1).Trim();

            Hint? hint;

            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                hint = JsonSerializer.Deserialize<Hint>(trimmed, options);
            }
            catch (JsonException ex)
            {
                throw new HintException("Failed to deserialize Hint from JSON.", ex);
            }

            if (hint == null)
                throw new HintException("Hint did not deserialized properly!");
            else
                return hint;
        }

        public override string ToString()
        {
            return $"{Word} | {NoumberOfSimilarWords}";
        }


        [Serializable]
        public class HintException : Exception
        {
            public HintException() { }
            public HintException(string message) : base(message) { }
            public HintException(string message, Exception inner) : base(message, inner) { }
        }
    }
}