using System;
using System.Threading.Tasks;
using TajniacyAI;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Tester AI - Wersja z Wektorami";

        Console.Write("Podaj klucz API: ");
        string apiKey = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(apiKey)) return;

        OdpowiedzMistrzaGry.Konfiguruj(apiKey);

        Console.WriteLine("\n--- ROZPOCZYNAM TESTY (Słowo, Drużyna, Wektor) ---\n");

        // Symulacja wektora (w prawdziwej grze to wynik z modelu embeddingów)
        float[] przykladowyWektor = new float[] { 0.1f, 0.5f, -0.2f, 0.9f };

        // 1. Sytuacja: Trafienie (Niebiescy zgadują Niebieską kartę)
        var hint1 = new Podpowiedz("Woda", 2);
        var karta1 = new Karta("Ocean", Druzyna.Niebiescy, przykladowyWektor);
        await Testuj(hint1, karta1, czyPoprawne: true);

        // 2. Sytuacja: Cywil
        var hint2 = new Podpowiedz("Las", 3);
        var karta2 = new Karta("Samochód", Druzyna.Cywil, przykladowyWektor);
        await Testuj(hint2, karta2, czyPoprawne: false);

        // 3. Sytuacja: Zabójca
        var hint3 = new Podpowiedz("Życie", 1);
        var karta3 = new Karta("Śmierć", Druzyna.Zabojca, przykladowyWektor);
        await Testuj(hint3, karta3, czyPoprawne: false); // Poprawność nie ma znaczenia przy zabójcy

        // 4. Sytuacja: Karta Wroga (Czerwoni kliknęli Niebieską lub odwrotnie)
        var hint4 = new Podpowiedz("Ogień", 2);
        var karta4 = new Karta("Lód", Druzyna.Niebiescy, przykladowyWektor); // Zakładamy że zgaduje Czerwony
        await Testuj(hint4, karta4, czyPoprawne: false);

        Console.WriteLine("Koniec testów.");
        Console.ReadLine();
    }

    // Metoda pomocnicza do wyświetlania
    static async Task Testuj(Podpowiedz hint, Karta karta, bool czyPoprawne)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[SCENARIUSZ] Hint: {hint.Haslo} | Karta: {karta.Slowo} | Typ: {karta.Przynaleznosc}");
        // Wyświetlamy długość wektora, żeby potwierdzić że tam jest
        Console.WriteLine($"[INFO] Wektor załadowany, długość: {karta.Wektor.Length}");
        Console.ResetColor();

        string odp = await OdpowiedzMistrzaGry.Stworz(karta, hint, czyPoprawne);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"> BOMBA: {odp}\n");
        Console.ResetColor();
    }
}