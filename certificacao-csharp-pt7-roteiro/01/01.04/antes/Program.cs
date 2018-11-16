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
        }
    }
}