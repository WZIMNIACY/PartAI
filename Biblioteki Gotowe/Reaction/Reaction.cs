using System.Runtime.InteropServices;
using game;
using hints;
using FileOperations;
using System.Runtime.CompilerServices;


namespace Reaction;

public static class Reaction
{

    public static string create(LLM llm, Hint hint, Card pickedCard, bool KapitnBomba, string actualTour)
    {

        string hintPromptPath;
        string reaction = string.Empty;

        string _pickedCard = pickedCard.toJson();
        string _givenHint = hint.toJson();
        bool _properGuess = actualTour == pickedCard.Team;

        string baseDir = Directory.GetCurrentDirectory();
        var parentDirInfo = Directory.GetParent(baseDir);

        if (parentDirInfo == null)
                throw new Exception("Cannot locate parent directory for apiKey.txt.");
            


        if (!KapitnBomba)
            hintPromptPath = Path.Combine(parentDirInfo.FullName, @"Reaction/ReactionPrompt.txt");
        else
            hintPromptPath = Path.Combine(parentDirInfo.FullName, @"Reaction/ReactionPromptKapitanBomba.txt");

        string prompt = FileOp.Read(hintPromptPath).Trim();
        
        string systemPromptReaction = FileOp.Read(hintPromptPath);
        string userPromptReaction = $"" +
            $"_pickedCard = {_pickedCard}\n" +
            $"_givenHint = {_givenHint}\n" + 
            $"_properGuess = {_properGuess}";

        while (true)
        {
            reaction = llm.SendRequestAsync(systemPromptReaction, userPromptReaction).Result;

            bool validReaction = string.IsNullOrWhiteSpace(reaction);

            if (validReaction)
            {

                Console.WriteLine("Generated reaction is invalid, regenerating...");
                continue;
            }
            else
            {

            int start = reaction.IndexOf('{') + 1;
            int end = reaction.LastIndexOf('}') - 1;
            
                if (start == -1 || end == -1 || end < start)
                    throw new ReactionException("Input does not contain a valid JSON object.");

                string trimmed = reaction.Substring(start, end - start + 1).Trim();

                return trimmed;

            }
        }

        


    }

    [System.Serializable]
        public class ReactionException : System.Exception
        {
            public ReactionException() { }
            public ReactionException(string message) : base(message) { }
            public ReactionException(string message, System.Exception inner) : base(message, inner) { }
            
        }

}

