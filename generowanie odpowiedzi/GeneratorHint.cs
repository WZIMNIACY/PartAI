using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Wzimniacy2
{
    public class GeneratorHint
    {
        private readonly LocalLLM _llm;

        public GeneratorHint(LocalLLM llm)
        {
            _llm = llm;
        }

        public async Task<string> GetHint(
            List<string> myWords,
            List<string> enemyWords,
            List<string> neutralWords,
            string assassinWord,
            string teamColor)
        {
            string template = File.ReadAllText("prompt.txt");

            string prompt = template
                .Replace("{CurrentTeamColor}", teamColor)
                .Replace("{MyWords}", string.Join(", ", myWords))
                .Replace("{EnemyWords}", string.Join(", ", enemyWords))
                .Replace("{NeutralWords}", string.Join(", ", neutralWords))
                .Replace("{AssassinWord}", assassinWord);

            return await _llm.Ask(prompt);
        }
    }
}
