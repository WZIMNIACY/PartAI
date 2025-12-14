using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Wzimniacy2
{
    public class GameEngine
    {
        private readonly Random _rng = new Random();

        public List<GameCard> CreateNewBoard(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                throw new FileNotFoundException("Brak pliku: " + jsonFilePath);

            string json = File.ReadAllText(jsonFilePath);

            var rawData = JsonSerializer.Deserialize<Dictionary<string, float[]>>(json);

            if (rawData == null || rawData.Count < 25)
                throw new Exception("Za mało słów w bazie (min 25).");

            var selected = rawData.OrderBy(_ => Guid.NewGuid()).Take(25).ToList();
            var cards = new List<GameCard>();

            foreach (var e in selected)
            {
                cards.Add(new GameCard
                {
                    Word = e.Key,
                    Vector = e.Value,
                    IsRevealed = false
                });
            }

            // ROLE: 9 czerwonych, 8 niebieskich, 7 neutral, 1 zabójca
            for (int i = 0; i < 25; i++)
            {
                if (i < 9) cards[i].Team = Team.Red;
                else if (i < 17) cards[i].Team = Team.Blue;
                else if (i < 24) cards[i].Team = Team.Neutral;
                else cards[i].Team = Team.Assassin;
            }

            return cards.OrderBy(_ => _rng.Next()).ToList();
        }

        
    }
}
