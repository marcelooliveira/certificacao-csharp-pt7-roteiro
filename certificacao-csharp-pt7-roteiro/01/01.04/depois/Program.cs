using System;

namespace _01_04
{
    //class Program
    //{
    //    delegate int Operacao(int a, int b);

    //    static int Subtrair(int a, int b)
    //    {
    //        Console.WriteLine("Foi chamado: Subtrair");
    //        return a - b;
    //    }

    //    static void Main(string[] args)
    //    {
    //        //Operacao adicionar = (a, b) => a + b;

    //        Operacao adicionar = (a, b) =>
    //        {
    //            Console.WriteLine("Foi chamado: adicionar");
    //            return a + b;
    //        };

    //        Console.WriteLine(adicionar(2, 2));

    //        Operacao op = Subtrair;
    //        Console.WriteLine(op(2, 2));
    //        Console.ReadKey();
    //    }
    //}

    //class Program
    //{
    //    delegate int GetValor();
    //    static GetValor getValorLocal;
    //    static void SetValorLocal()
    //    {
    //        int valorLocal = 99;
    //        getValorLocal = () => valorLocal;
    //    }
    //    static void Main(string[] args)
    //    {
    //        SetValorLocal();
    //        Console.WriteLine("Valor de valorLocal: {0}", getValorLocal());
    //        Console.ReadKey();
    //    }
    //}

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