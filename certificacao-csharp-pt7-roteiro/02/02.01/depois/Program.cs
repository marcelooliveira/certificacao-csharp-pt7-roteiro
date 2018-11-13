using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _01_03
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Diretor> diretores = GetDiretores();
            List<Filme> filmes = GetFilmes();

            //Console.WriteLine($"{"Título",-40}{"Diretor",-20}{"Ano",4}");
            //Console.WriteLine(new string('=', 64));
            //foreach (var filme in filmes)
            //{
            //    Console.WriteLine($"{filme.Titulo, -40}{filme.Diretor.Nome, -20}{filme.Ano}");
            //}

            Imprimir(filmes);

            Filme novoFilme = new Filme
            {
                DiretorId = 3,
                Diretor = new Diretor { Id = 3, Nome = "Tim Burton" },
                Titulo = "A Fantástica Fábrica de Chocolate",
                Ano = 2005,
                Minutos = 115
            };

            filmes.Add(novoFilme);

            IEnumerable<Filme> filmesSelecionados
                = from filme in filmes
                  where filme.Diretor.Nome == "Tim Burton"
                  select filme;

            Imprimir(filmesSelecionados);

            //string nome = "Steven Spielberg";
            //int ano = 1984;
            //int teste = ano - nome;

            var selecionados =
            from filme in filmes
            where filme.Diretor.Nome == "Tim Burton"
            select filme;

            Imprimir(filmesSelecionados);

            var selecionados2 =
            from filme in filmes
            where filme.Diretor.Nome == "Tim Burton"
            select new FilmeResumido
            {
                NomeDiretor = filme.Diretor.Nome,
                Titulo = filme.Titulo
            };

            Imprimir(selecionados2);

            var selecionados3 =
            from filme in filmes
            where filme.Diretor.Nome == "Tim Burton"
            select new // tipo anônimo: tipo da projeção não é necessário
            {
                NomeDiretor = filme.Diretor.Nome,
                filme.Titulo
            };

            foreach (var filme in selecionados3)
            {
                Console.WriteLine($"{filme.Titulo,-40}{filme.NomeDiretor,-20}");
            }
            Console.WriteLine();

            var filmesDeDiretores =
            from diretor in diretores
            where diretor.Nome == "Tim Burton"
            join filme in filmes
            on diretor.Id equals filme.DiretorId
            select new
            {
                NomeDiretor = diretor.Nome,
                filme.Titulo
            };

            foreach (var filme in filmesDeDiretores)
            {
                Console.WriteLine($"{filme.Titulo,-40}{filme.NomeDiretor,-20}");
            }
            Console.WriteLine();



            var resumoDiretor =
            from filme in filmes
            group filme by filme.DiretorId
            into resumoFilmeDiretor
            select new
            {
                ID = resumoFilmeDiretor.Key,
                Count = resumoFilmeDiretor.Count()
            };

            Console.WriteLine($"{"DiretorId",-20}{"Quantidade",10}");
            Console.WriteLine(new string('=', 30));
            foreach (var item in resumoDiretor)
            {
                Console.WriteLine($"{item.ID,-20}{item.Count,10}");
            }
            Console.WriteLine();




            var resumoDiretorComNome =
            from filme in filmes
            join diretor in diretores
            on filme.DiretorId equals diretor.Id
            group filme by diretor.Nome
            into resumoFilmeDiretor
            select new
            {
                ID = resumoFilmeDiretor.Key,
                Count = resumoFilmeDiretor.Count()
            };

            Console.WriteLine($"{"Nome Diretor",-20}{"Quantidade",10}");
            Console.WriteLine(new string('=', 30));
            foreach (var item in resumoDiretorComNome)
            {
                Console.WriteLine($"{item.ID,-20}{item.Count,10}");
            }
            Console.WriteLine();




            int numeroPagina = 0;
            int tamanhoPagina = 4;

            while (true)
            {
                // obtém informação sobre o filme
                var listaDeFilmes =
                from filme
                    in filmes.Skip(numeroPagina * tamanhoPagina).Take(tamanhoPagina)
                join diretor in diretores
                    on filme.DiretorId equals diretor.Id
                select new
                {
                    NomeDiretor = diretor.Nome,
                    filme.Titulo
                };
                // Sai do laço while se chegar ao final dos dados

                if (listaDeFilmes.Count() == 0)
                    break;

                // Exibe os resultados da consulta
                foreach (var item in listaDeFilmes)
                {
                    Console.WriteLine($"{item.NomeDiretor,-30}{item.Titulo,-30}");
                }
                Console.WriteLine("Tecle algo para continuar...");
                Console.ReadKey();
                // avança uma página
                numeroPagina++;
            }






            var resumoDoDiretor =
            from filme in filmes
            join diretor in diretores
            on filme.DiretorId equals diretor.Id
            group filme by diretor.Nome
            into resumoDiretorFilme
            select new
            {
                Diretor = resumoDiretorFilme.Key,
                TotalMinutos = resumoDiretorFilme.Sum(x => x.Minutos)
            };

            Console.WriteLine($"{"Nome Diretor",-30}{"Total Minutos",20}");
            Console.WriteLine(new string('=', 50));
            foreach (var item in resumoDoDiretor)
            {
                Console.WriteLine($"{item.Diretor,-30}{item.TotalMinutos,20}");
            }
            Console.WriteLine();

            IEnumerable<Filme> queryMetodo =
                filmes.Where(filme => filme.Diretor.Nome == "James Cameron");





            var resumoArtistaPorMetodo =
            filmes
            .Join(diretores,
                filme => filme.DiretorId,
                diretor => diretor.Id,
                (filme, diretor) =>
                new
                {
                    filme,
                    diretor
                }
            )
            .GroupBy(temp => temp.diretor)
            .Select(resumoDiretorFilme =>
            new
            {
                Diretor = resumoDiretorFilme.Key,
                TotalMinutos = resumoDiretorFilme.Sum(x => x.filme.Minutos)
            });
            Console.WriteLine($"{"Nome Diretor",-30}{"Total Minutos",20}");
            Console.WriteLine(new string('=', 50));
            foreach (var item in resumoDoDiretor)
            {
                Console.WriteLine($"{item.Diretor,-30}{item.TotalMinutos,20}");
            }
            Console.WriteLine();





            var diretorFilmeQuery =
            from diretor in diretores
            where diretor.Nome == "James Cameron"
            join filme in filmes
            on diretor.Id equals filme.DiretorId
            select new
            {
                NomeDiretor = diretor.Nome,
                filme.Titulo
            };

            var diretorFilmeArray = diretorFilmeQuery.ToArray();
            foreach (var item in diretorFilmeArray)
            {
                Console.WriteLine($"{item.NomeDiretor,-30}{item.Titulo,-30}");
            }
            Console.WriteLine();




            string xmlText =
            "<Filmes>" +
                "<Filme>" +
                    "<Diretor>Quentin Tarantino</Diretor>" +
                    "<Titulo>Pulp Fiction</Titulo>" +
                    "<Minutos>154</Minutos>" +
                "</Filme>" +
                "<Filme>" +
                    "<Diretor>James Cameron</Diretor>" +
                    "<Titulo>Avatar</Titulo>" +
                    "<Minutos>162</Minutos>" +
                "</Filme>" +
            "</Filmes>";

            XDocument documentoFilmes = XDocument.Parse(xmlText);

            IEnumerable<XElement> filmesSelecionados2 =
            from filme in documentoFilmes.Descendants("Filme")
            select filme;
            foreach (XElement item in filmesSelecionados2)
            {
                Console.WriteLine("Diretor: {0}, Título: {1} ",
                    item.Element("Diretor").FirstNode,
                    item.Element("Titulo").FirstNode);
            }
            Console.WriteLine();


            filmesSelecionados2 =
            from filme in documentoFilmes.Descendants("Filme")
            where (string)filme.Element("Diretor") == "Quentin Tarantino"
            select filme;
            foreach (XElement item in filmesSelecionados2)
            {
                Console.WriteLine("Diretor: {0}, Título: {1} ",
                    item.Element("Diretor").FirstNode,
                    item.Element("Titulo").FirstNode);
            }
            Console.WriteLine();


            filmesSelecionados2 =
            from filme in documentoFilmes.Descendants("Filme")
            .Where(elemento => (string)elemento.Element("Diretor")
                == "Quentin Tarantino")
            select filme;
            foreach (XElement item in filmesSelecionados2)
            {
                Console.WriteLine("Diretor: {0}, Título: {1} ",
                    item.Element("Diretor").FirstNode,
                    item.Element("Titulo").FirstNode);
            }
            Console.WriteLine();

            XElement filmesXML = new XElement("Filmes",
                new List<XElement>
                {
                    new XElement ("Filme" ,
                        new XElement ("Diretor" , "Steven Spielberg"),
                        new XElement ("Titulo" , "A Lista de Schindler")),
                    new XElement ("Filme",
                        new XElement ("Diretor" , "Christopher Nolan"),
                        new XElement ("Titulo" , "Batman: O Cavaleiro das Trevas"))
                }
            );




            filmesSelecionados2 =
            from filme in documentoFilmes.Descendants("Filme")
            select filme;
            foreach (XElement item in filmesSelecionados2)
            {
                item.Element("Titulo").FirstNode.ReplaceWith("Novo filme");
            }

            foreach (XElement item in filmesSelecionados2)
            {
                Console.WriteLine("Diretor: {0}, Título: {1} ",
                    item.Element("Diretor").FirstNode,
                    item.Element("Titulo").FirstNode);
            }
            Console.WriteLine();



            var filmesSelecionados3 =
            from filme in documentoFilmes.Descendants("Filme")
            select filme;
            foreach (XElement item in filmesSelecionados3)
            {
                item.Add(new XElement("Genero", "Drama"));
            }

            foreach (XElement item in filmesSelecionados3)
            {
                Console.WriteLine("Diretor: {0}, Título: {1}, Gênero: {2} ",
                    item.Element("Diretor").FirstNode,
                    item.Element("Titulo").FirstNode,
                    item.Element("Genero").FirstNode);
            }
            Console.WriteLine();


            Console.ReadKey();
        }

        private static void Imprimir(IEnumerable<Filme> filmes)
        {
            Console.WriteLine($"{"Título",-40}{"Diretor",-20}{"Ano",4}");
            Console.WriteLine(new string('=', 64));
            foreach (var filme in filmes)
            {
                Console.WriteLine($"{filme.Titulo,-40}{filme.Diretor.Nome,-20}{filme.Ano}");
            }
            Console.WriteLine();
        }

        private static void Imprimir(IEnumerable<FilmeResumido> filmes)
        {
            Console.WriteLine($"{"Título",-40}{"Diretor",-20}");
            Console.WriteLine(new string('=', 60));
            foreach (var filme in filmes)
            {
                Console.WriteLine($"{filme.Titulo,-40}{filme.NomeDiretor,-20}");
            }
            Console.WriteLine();
        }

        private static List<Diretor> GetDiretores()
        {
            return new List<Diretor>
            {
                new Diretor { Id = 1, Nome = "Quentin Tarantino" },
                new Diretor { Id = 2, Nome = "James Cameron" },
                new Diretor { Id = 3, Nome = "Tim Burton" }
            };
        }

        private static List<Filme> GetFilmes()
        {
            return new List<Filme> {
                new Filme {
                    DiretorId = 1,
                    Diretor = new Diretor { Nome = "Quentin Tarantino" },
                    Titulo = "Pulp Fiction",
                    Ano = 1994,
                    Minutos = 2 * 60 + 34
                },
                new Filme {
                    DiretorId = 1,
                    Diretor = new Diretor { Nome = "Quentin Tarantino" },
                    Titulo = "Django Livre",
                    Ano = 2012,
                    Minutos = 2 * 60 + 45
                },
                new Filme {
                    DiretorId = 1,
                    Diretor = new Diretor { Nome = "Quentin Tarantino" },
                    Titulo = "Kill Bill Volume 1",
                    Ano = 2003,
                    Minutos = 1 * 60 + 51
                },

                new Filme {
                    DiretorId = 2,
                    Diretor = new Diretor { Nome = "James Cameron" },
                    Titulo = "Avatar",
                    Ano = 2009,
                    Minutos = 2 * 60 + 42
                },
                new Filme {
                    DiretorId = 2,
                    Diretor = new Diretor { Nome = "James Cameron" },
                    Titulo = "Titanic",
                    Ano = 1997,
                    Minutos = 3 * 60 + 14
                },
                new Filme {
                    DiretorId = 2,
                    Diretor = new Diretor { Nome = "James Cameron" },
                    Titulo = "O Exterminador do Futuro",
                    Ano = 1984,
                    Minutos = 1 * 60 + 47
                },

                new Filme {
                    DiretorId = 3,
                    Diretor = new Diretor { Nome = "Tim Burton" },
                    Titulo = "O Estranho Mundo de Jack",
                    Ano = 1993,
                    Minutos = 1 * 60 + 16
                },
                new Filme {
                    DiretorId = 3,
                    Diretor = new Diretor { Nome = "Tim Burton" },
                    Titulo = "Alice no País das Maravilhas",
                    Ano = 2010,
                    Minutos = 1 * 60 + 48
                },
                new Filme {
                    DiretorId = 3,
                    Diretor = new Diretor { Nome = "Tim Burton" },
                    Titulo = "A Noiva Cadáver",
                    Ano = 2005,
                    Minutos = 1 * 60 + 17
                }
            };
        }
    }

    class Diretor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    class Filme
    {
        public int DiretorId { get; set; }
        public Diretor Diretor { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public int Minutos { get; set; }
    }

    class FilmeResumido
    {
        public string NomeDiretor { get; set; }
        public string Titulo { get; set; }
    }
}
