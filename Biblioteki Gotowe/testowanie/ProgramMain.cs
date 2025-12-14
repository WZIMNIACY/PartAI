using System;

namespace testowanie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReactionTesting.Test();
            }
            catch (System.Exception ex)
            {
                
                System.Console.WriteLine(ex.Message);
            }

            
        }

        
    }
}
