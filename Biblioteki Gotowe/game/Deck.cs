using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
// Removed dependency on the `hints` project to avoid circular project references.


namespace game
{
    public enum Team
    {
        Blue,
        Red,
        Neutral,
        Assassin
    }

    public sealed class Deck
    {
        public List<Card> Cards { get; init; } = new List<Card>();
        public Team StartingTeam { get; init; }

        private Deck()
        {
        }

        public static Deck CreateFromDictionary(Dictionary<string, List<double>> dictionary, Random? rng = null)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.Count < 25)
            {
                throw new ArgumentException("Dictionary must contain at least 25 entries.", nameof(dictionary));
            }

            rng ??= new Random();

            // Take 25 unique random words
            var keys = dictionary.Keys.ToList();
            Shuffle(keys, rng);
            var selectedKeys = keys.Take(25).ToList();

            // Determine starting team: randomly Blue or Red
            var startingTeam = rng.Next(2) == 0 ? Team.Blue : Team.Red;
            var otherTeam = startingTeam == Team.Blue ? Team.Red : Team.Blue;

            const int totalCards = 25;
            const int assassinCount = 1;
            int startingTeamCount = 9;
            int otherTeamCount = 8;
            int neutralCount = totalCards - (startingTeamCount + otherTeamCount + assassinCount);

            // Build assignments and shuffle them
            var assignments = new List<Team>(totalCards);
            assignments.AddRange(Enumerable.Repeat(startingTeam, startingTeamCount));
            assignments.AddRange(Enumerable.Repeat(otherTeam, otherTeamCount));
            assignments.AddRange(Enumerable.Repeat(Team.Assassin, assassinCount));
            assignments.AddRange(Enumerable.Repeat(Team.Neutral, neutralCount));
            if (assignments.Count != totalCards)
            {
                throw new InvalidOperationException("Team assignment counts do not add up to 25.");
            }

            Shuffle(assignments, rng);

            // Create cards
            var cards = new List<Card>(totalCards);
            for (int i = 0; i < totalCards; i++)
            {
                var word = selectedKeys[i];
                // Copy vector to avoid external mutation
                var vector = new List<double>(dictionary[word]);
                var team = assignments[i];
                cards.Add(new Card(word, vector, team.ToString()));
            }

            // Optionally shuffle final deck (cards already in random order due to assignments shuffle but shuffle again)
            Shuffle(cards, rng);

            return new Deck
            {
                Cards = cards,
                StartingTeam = startingTeam
            };
        }

        public string ToJson()
        {
            var options = CreateJsonOptions();
            return JsonSerializer.Serialize(this, options);
        }

        public static Deck FromJson(string json)
        {
           

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Input jsonFormat is null or whitespace.");

            Deck? deck;

            try
            {
                deck = JsonSerializer.Deserialize<Deck>(json);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Deck from JSON.", ex);
            }

            if (deck == null)
                throw new InvalidOperationException("Deck did not deserialized properly!");
            else
                return deck;
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return options;
        }

        // Fisher-Yates shuffle for IList<T>
        private static void Shuffle<T>(IList<T> list, Random rng)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                if (j == i) continue;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            int i = 1;

            foreach (Card item in Cards)
            {
                sb.AppendLine("Card " + i + ":\n" + item + "\n");
                i++;
            }

            return sb.ToString();

        }
    }
}
