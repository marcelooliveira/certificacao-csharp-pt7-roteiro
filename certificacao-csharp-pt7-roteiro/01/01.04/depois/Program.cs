using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace depois
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> materias = new List<string>
            {
                "Matemática", "Português", "Inglês", "Geografia", "História", "Biologia"
            };

            Console.WriteLine($"Buscando 'Inglês': {Existe(materias, "Inglês")}");
            Console.WriteLine($"Buscando 'English': {Existe(materias, "English")}");
            Console.WriteLine($"Buscando 'Geografia': {Existe(materias, "Geografia")}");
            
            Console.ReadKey();
        }

        private static bool Existe(List<string> materias, string busca)
        {
            //var encontrarMateria = materias.Exists(delegate (string materia)
            //{
            //    return materia.Equals(busca);
            //});

            var encontrarMateria = materias.Exists(x => x == busca);

            return encontrarMateria;
        }
    }
}
