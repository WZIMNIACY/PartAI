using System;

namespace testowanie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HintTesting.Test();
            }
            catch (System.Exception ex)
            {
                
                System.Console.WriteLine(ex.Message);
            }

            
        }

        
    }
}
