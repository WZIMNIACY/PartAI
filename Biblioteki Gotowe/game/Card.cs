using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace game
{
    public sealed record Card
    {
        public string Word { get; init; }
        public List<double>? Vector { get; init; }
        public string Team { get; init; }

        public Card(string word, List<double>? vector, string team)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
            Vector = vector;
            Team = team ?? throw new ArgumentNullException(nameof(team));
        }

        
        


        public string fullWordInfo()
        {
            StringBuilder sb = new StringBuilder();

            if (Vector != null)
            {
                foreach (double item in Vector)
                {
                    sb.AppendLine(item.ToString());
                }
            }

            return $"Word: {Word}\nVector: {sb.ToString()}\nTeam: {Team}";
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Vector != null)
            {
                foreach (double item in Vector)
                {
                    sb.AppendLine(item.ToString());
                }

            }

            return $"Word: {Word}\nTeam: {Team.ToString()}";
        }

        public string toJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)

            });
        }

        public static Card fromJson(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<Card>(json)!;
        }

    }
}
