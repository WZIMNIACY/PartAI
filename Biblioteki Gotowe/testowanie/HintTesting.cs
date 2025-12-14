using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using FileOperations;
using game;
using hints;


namespace testowanie
{
    internal static class HintTesting
    {
        public static void Test()
        {
//### Hint Generating ###//
            string vectorsPath = Path.Combine(Directory.GetCurrentDirectory(), "reducedVectorsBase.txt");
            string data = FileOp.Read(vectorsPath);

            //Creating llm connection
            string baseDir = Directory.GetCurrentDirectory();
            var parentDirInfo = Directory.GetParent(baseDir);
            if (parentDirInfo == null)
                throw new Exception("Cannot locate parent directory for apiKey.txt.");
            string apiKeyPath = Path.Combine(parentDirInfo.FullName, "apiKey.txt");
            string apiKey = FileOp.Read(apiKeyPath).Trim();
            
            LLM llm = new(apiKey);

            for (int i = 0; i < 5; i++)
            {
                            //Getting data (use absolute path to avoid relative-path confusion)

             //Creating deck
            Dictionary<string, List<double>>? words = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(data, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

                Deck? deck; 

            if (words != null)
                deck = Deck.CreateFromDictionary(words);
            else
                throw new Exception("Dictionary deserialization failed.");

            

            Hint hint = Hint.Create(deck, llm, Team.Red);

            System.Console.WriteLine(hint.toJson());

                
            }

        }
    }
}