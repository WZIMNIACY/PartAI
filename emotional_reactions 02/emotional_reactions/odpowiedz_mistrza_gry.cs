using System.Text;
using System.Threading.Tasks;

namespace TajniacyAI
{
    public static class OdpowiedzMistrzaGry
    {
        private static LLM _silnik;

        public static void Konfiguruj(string kluczApi)
        {
            _silnik = new LLM(kluczApi);
        }

        public static async Task<string> Stworz(Karta karta, Podpowiedz podpowiedz, bool czyPoprawne)
        {
            if (_silnik == null) return "BŁĄD: Użyj OdpowiedzMistrzaGry.Konfiguruj()!";

            
            string systemPrompt = @"
                Jesteś Kapitanem Tytusem Bombą.
                1. Traktuj graczy jak idiotów.
                2. Używaj wulgaryzmów, ale CENZURUJ je gwiazdkami (np. k*rwa, ch*j).
                3. Komentuj celność strzału w grze Tajniacy.
                4. Bądź krótki i agresywny.";

            
            StringBuilder sb = new StringBuilder();
            sb.Append($"Hint: '{podpowiedz.Haslo}' ({podpowiedz.Liczba}). ");
            sb.Append($"Wybrano: '{karta.Slowo}'. ");

            
            switch (karta.Przynaleznosc)
            {
                case Druzyna.Zabojca:
                    sb.Append("To jest ZABÓJCA! Koniec gry. Wpadnij w szał, wyzwij ich.");
                    break;

                case Druzyna.Cywil:
                    sb.Append("To jest CYWIL (Neutralny). Pudło. Wyśmiej ich celność.");
                    break;

                default:
                    if (czyPoprawne)
                    {
                        sb.Append("To jest DOBRA KARTA (ich drużyny). Pochwal ich z niechęcią.");
                    }
                    else
                    {
                        sb.Append("To jest KARTA PRZECIWNIKA! Oddali punkt wrogowi. Zmieszaj ich z błotem.");
                    }
                    break;
            }

            return await _silnik.GenerujTekst(systemPrompt, sb.ToString());
        }
    }
}