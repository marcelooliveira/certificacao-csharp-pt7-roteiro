using System;

namespace _01_04
{
    class Program
    {
        static Func<int, int, int> adicionar = (a, b) => a + b;
        static Action<string> logMensagem = (mensagem) => Console.WriteLine(mensagem);
        static Predicate<int> divisivelPor3 = (i) => i % 3 == 0;

        static void Main()
        {
            Console.WriteLine($"adicionar(2, 3): {adicionar(2, 3)}");
            Console.WriteLine($"logMensagem('esta é uma Action')");
            logMensagem("esta é uma Action");
            var numeros = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var divisiveis = Array.FindAll(numeros, divisivelPor3);
            Console.WriteLine("Divisíveis por 3: " + string.Join(',', divisiveis));
            Console.ReadKey();
        }
    }
}