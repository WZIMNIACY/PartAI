namespace TajniacyAI
{
    
    public enum Druzyna
    {
        Czerwoni,
        Niebiescy,
        Cywil,      
        Zabojca     
    }

    
    public class Karta
    {
        public string Slowo { get; set; }
        public Druzyna Przynaleznosc { get; set; } 
        public float[] Wektor { get; set; }        

        
        public Karta(string slowo, Druzyna przynaleznosc, float[] wektor = null)
        {
            Slowo = slowo;
            Przynaleznosc = przynaleznosc;
            Wektor = wektor ?? new float[0];
        }
    }

    public class Podpowiedz
    {
        public string Haslo { get; set; }
        public int Liczba { get; set; }

        public Podpowiedz(string haslo, int liczba)
        {
            Haslo = haslo;
            Liczba = liczba;
        }
    }
}