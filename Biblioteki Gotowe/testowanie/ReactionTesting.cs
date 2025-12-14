using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using FileOperations;
using game;
using hints;


namespace testowanie
{

    internal static class ReactionTesting
    {
        public static void Test()
        {

            string baseDir = Directory.GetCurrentDirectory();
            var parentDirInfo = Directory.GetParent(baseDir);
            if (parentDirInfo == null)
                throw new Exception("Cannot locate parent directory for apiKey.txt.");
            string apiKeyPath = Path.Combine(parentDirInfo.FullName, "apiKey.txt");
            string apiKey = FileOp.Read(apiKeyPath).Trim();

            //LLM Connection
            LLM llm = new(apiKey);

            //Creating cards
            Card card1 = new("kot", new List<double>(), Team.Blue.ToString());
            Card card2 = new("pies", new List<double>(), Team.Blue.ToString());
            //Bad answer card
            Card BadAnswer = new("traktor", new List<double>(), Team.Red.ToString());

            //Creating deck
            List<Card> deck = new List<Card>() { card1, card2 };
            Hint hint = new("zwierze", deck, 2);


            // false - Kapitan Bomba mode OFF
            // true - Kapitan Bomba mode ON

            //Good answer with no Kapitan Bomba mode
            string GoodAnsNoKB = Reaction.Reaction.create(llm, hint, card1, false, Team.Blue.ToString());
            //Bad answer with no Kapitan Bomba mode
            string BadAnsNoKB = Reaction.Reaction.create(llm, hint, BadAnswer, false, Team.Blue.ToString());
            //Good answer with Kapitan Bomba mode
            string GoodAnsKB = Reaction.Reaction.create(llm, hint, card1, true, Team.Blue.ToString());
            //Bad answer with Kapitan Bomba mode
            string BadAnsKB = Reaction.Reaction.create(llm, hint, BadAnswer, true, Team.Blue.ToString());

            StringBuilder sb = new();
            sb.AppendLine("########### TESTS ###########");
            sb.AppendLine("#############################");
            sb.AppendLine("GoodAnsNoKB: " + GoodAnsNoKB);
            sb.AppendLine("BadAnsNoKB: " + BadAnsNoKB);
            sb.AppendLine("GoodAnsKB: " + GoodAnsKB);
            sb.AppendLine("BadAnsKB: " + BadAnsKB);
            sb.AppendLine("#############################");

            System.Console.WriteLine(sb.ToString());

        }
    }

}