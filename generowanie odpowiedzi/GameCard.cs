using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wzimniacy2
{
    public enum Team { Red, Blue, Neutral, Assassin }

    public class GameCard
    {
        public string Word { get; set; }
        public float[] Vector { get; set; }
        public Team Team { get; set; }
        public bool IsRevealed { get; set; } = false;

        public override string ToString() => $"{Word} ({Team})";
    }
}
