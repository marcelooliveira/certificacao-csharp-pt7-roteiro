Consultar e manipular dados e objetos usando o LINQ

Comandos SQL s�o demorados para criar e
processar o resultado de uma consulta � um trabalho �rduo.
A Consulta INtegrated Language, ou LINQ, foi criada para
torna muito f�cil para programadores C# trabalhar com
fontes de dados. Esta se��o aborda o LINQ e como
use-o.
Note que h� muitos exemplos de programas em
esta se��o, e as instru��es LINQ podem ser um pouco
dif�cil de entender no come�o. No entanto, lembre-se que
voc� pode baixar e executar todo o c�digo de exemplo.

Aplica��o de exemplo

A fim de fornecer um bom contexto para a explora��o
de LINQ vamos desenvolver os Music TrackS
aplicativo usado anteriormente neste livro. Esta aplica��o
permite o armazenamento de dados de faixa de m�sica. Figura 4�15
mostra as classes no sistema. Note que no
momento em que n�o estamos usando um banco de dados para armazenar a classe
em forma��o. Mais tarde, vamos considerar como mapear isso
projeto em um banco de dados.

Nas vers�es anteriores do aplicativo, o
A classe MusicTrack continha uma string que dava
nome do artista que gravou a faixa. No novo
design, uma faixa de m�sica cont�m uma refer�ncia a um
Objeto do artista que descreve o artista que registrou o
faixa. A Figura 4�15 mostra como isso funciona. Se um artista
registra mais de uma faixa (o que � muito prov�vel), a
detalhes do artista s� ser�o armazenados uma vez e referidos por
muitas inst�ncias do Music Track.

FIGURA 4-15 Projeto de classe de trilhas de m�sica

O c�digo a seguir mostra o c�digo C # para as classes.

```csharp
class Diretor
{
    public string Nome { get; set; }
}

class Filme
{
    public Diretor Diretor { get; set; }
    public string Titulo { get; set; }
    public int Ano { get; set; }
}
```

Agora que voc� tem o design de classe para o
aplica��o, a pr�xima coisa a fazer � criar alguns
dados de amostra. Isso deve ser feito de forma program�tica.
Testar um sistema inserindo dados � m�o � um mau
id�ia por v�rias raz�es. Primeiro, � muito
consumir. Em segundo lugar, quaisquer altera��es no armazenamento de dados
design Vai significar que voc� provavelmente ter� que entrar
todos os dados novamente. E terceiro, o ato de criar o
os dados de teste podem fornecer informa��es �teis sobre sua turma
desenhar.

LISTAGEM mostra c�digo que cria alguns artistas
e faixas. Voc� pode aumentar a quantidade de dados de teste
adicionando mais artistas e t�tulos. Nesta vers�o todos
os artistas gravaram todas as faixas. Aleat�rio
gerador de n�meros fornece cada faixa aleat�ria
comprimento no intervalo de 20 a 600 segundos. Observe que
porque o gerador de n�meros aleat�rios tem um fixo
valor da semente os comprimentos de cada faixa Ser�o os mesmos
cada vez que voc� executar o programa.

```csharp
static void Main(string[] args)
{
    List<Diretor> diretores = GetDiretores();
    List<Filme> filmes = GetFilmes();

    Console.WriteLine($"{"T�tulo",-30}{"Diretor",-20}{"Ano",4}");
    Console.WriteLine(new string('=', 54));
    foreach (var filme in filmes)
    {
        Console.WriteLine($"{filme.Titulo, -30}{filme.Diretor.Nome, -20}{filme.Ano}");
    }

    Console.ReadKey();
}

private static List<Diretor> GetDiretores()
{
    return new List<Diretor>
    {
        new Diretor { Nome = "Quentin Tarantino" },
        new Diretor { Nome = "James Cameron" },
        new Diretor { Nome = "Tim Burton" }
    };
}

private static List<Filme> GetFilmes()
{
    return new List<Filme> {
        new Filme {
            Diretor = new Diretor { Nome = "Quentin Tarantino" },
            Titulo = "Pulp Fiction",
            Ano = 1994
        },
        ...
    };
}
```

![File3](file3.png)

Use um inicializador de objeto

Se voc� olhar o c�digo na LISTAGEM, ver� que
estamos usando a sintaxe do inicializador de objetos para criar novos
inst�ncias dos objetos de m�sica e inicializar seus
valores ao mesmo tempo. Esta � uma Ci� muito �til
recurso que permite inicializar objetos quando eles
s�o criados sem a necessidade de criar um construtor
m�todo na classe que est� sendo inicializada.
O c�digo a seguir mostra como funciona. A declara��o
cria e inicializa uma nova inst�ncia do Music Track.
Observe o uso de chaves ({e}) para delimitar os itens
que inicializam a inst�ncia e COMmas para separar
cada valor sendo usado para inicializar o objeto.

```csharp
Filme novoFilme = new Filme
{
    Diretor = new Diretor { Nome = "Tim Burton" },
    Titulo = "A Fant�stica F�brica de Chocolate",
    Ano = 2005
};
```

Voc� n�o precisa inicializar todos os elementos do
inst�ncia; quaisquer propriedades n�o inicializadas s�o definidas como suas
valores padr�o (zero para um valor num�rico e nulo para um
corda). As propriedades a serem inicializadas dessa maneira
todos devem ser membros p�blicos da classe.

Use um operador LINQ

Agora que voc� tem alguns dados pode usar operadores LINQ
para construir consultas e extrair resultados dos dados. o
c�digo na LISTAGEM imprime os t�tulos de todos os
faixas que foram gravadas pelo artista com o nome
"Rob Miles". A primeira declara��o usa uma consulta LINQ para
criar uma cole��o enumer�vel de MusicTrack

refer�ncias chamadas selectedTracks que � ent�o
enumerado pelo f o chegar a constru��o para imprimir
os resultados.

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;

Imprimir(filmesSelecionados);
```

![File4](file4.png)

A consulta LINQ retorna um resultado IEnumerable
que � enumerado por uma constru��o foreach. Voc�
pode encontrar uma explica��o de IEnumerable na Skill 2.4
na se��o "IEnumerable". O �Create method-
com base em consultas LINQ �tem mais detalhes sobre como
consulta � realmente implementada pelo c�digo C #.
Use a palavra-chave var com o LINQ
A linguagem C # � "estaticamente digitada". O tipo de
objetos em um programa � determinado em tempo de compila��o
e o compilador rejeita quaisquer a��es que n�o s�o v�lidas.
Por exemplo, o c�digo a seguir falha ao compilar
porque o compilador �n�o permite que uma string seja
subtra�do de um n�mero.

```csharp
string nome = "Steven Spielberg";
int ano = 1984;
int teste = ano - nome;
```

Isso proporciona mais confian�a de que nossos programas s�o
correto antes que eles corram. A desvantagem � que voc� tem
para colocar no esfor�o de dar a cada vari�vel um tipo quando
voc� declara isso. Na maior parte do tempo, entretanto,
compilador pode inferir o tipo a ser usado para qualquer dado
vari�vel. A vari�vel de nome no exemplo anterior
deve ser do tipo string, j� que uma string est� sendo
atribu�do a ele. Pela mesma l�gica, a vari�vel idade
deve ser um int.

Voc� pode simplificar o c�digo usando a palavra-chave var para
diga ao compilador para inferir o tipo da vari�vel sendo
criado a partir do contexto em que a vari�vel � usada.
O compilador ir� definir uma vari�vel de string chamada
nameVar em resposta � seguinte declara��o:

```csharp
var nomeVar = "Steven Spielberg";
```

Note que isto n�o significa que o compilador
n�o pode detectar erros de compila��o. As declara��es em
LISTAGEM ainda n�o compila:

```csharp
var nomeVar = "Steven Spielberg";
var anoVar = 1984;
var testeVar = ano - nome;
```

A palavra-chave var � especialmente �til ao usar
LINQ. O resultado de uma consulta LINQ simples � um
cole��o enumer�vel do tipo de elemento de dados mantido
na fonte de dados. A declara��o a seguir mostra o
consulta da LISTAGEM.


```csharp
IEnumerable<Filme> selecionados =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select filme;
```


Para escrever esta declara��o, voc� deve descobrir o tipo
de dados na cole��o de faixas de m�sica e, em seguida, use
esse tipo com IEnumerable. A palavra-chave var
torna este c�digo muito mais f�cil de escrever (veja a LISTAGEM
32).

```csharp
var selecionados =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select filme;
```


Existem algumas situa��es onde voc� n�o sabe
o tipo de uma vari�vel Ao escrever o c�digo. Mais tarde
esta se��o voc� vai descobrir objetos que s�o criados
dinamicamente como o programa � executado e n�o tem nenhum tipo de
todos. Estes s�o chamados de tipos an�nimos. O �nico jeito
c�digo pode se referir a estes � pelo uso de vari�veis do tipo
var.

Voc� pode usar o tipo var em todo o seu c�digo
Se voc� gosta, mas por favor tenha cuidado. Uma declara��o como
o seguinte n�o vai fazer voc� muito popular com
colegas programadores porque � imposs�vel para eles
inferir o tipo de vari�vel v Sem cavar no
c�digo e descobrir que tipo � retornado pelo
M�todo de an�ncio DoRe.

var v = DoRead();

Se voc� realmente quer usar var nessas situa��es, voc�
deve certificar-se de que voc� seleciona nomes de vari�veis que
s�o adequadamente informativos, ou voc� pode inferir o tipo de
o item do c�digo, como no seguinte
afirma��es.


var nextPerson = DoReadPerson();
var newPerson = new Person();


Proje��o LINQ

Voc� pode usar a opera��o de sele��o no LINQ para
produzir uma vers�o filtrada de uma fonte de dados. Em
exemplos anteriores voc� descobriu todas as faixas
gravado por um artista em particular. Voc� pode criar outro
crit�rios de pesquisa, por exemplo, selecionando as faixas
com um certo t�tulo, ou faixas mais longas que um certo
comprimento.

O resultado de um select � uma cole��o de refer�ncias
para objetos na cole��o de dados de origem. Existe um
algumas raz�es Por que um programa pode n�o querer
funcionam assim. Primeiro, voc� pode n�o querer fornecer
refer�ncias aos objetos de dados reais nos dados
fonte. Em segundo lugar, voc� pode querer o resultado de uma consulta
para conter um subconjunto dos dados originais.

Voc� pode usar a proje��o para fazer uma consulta para "projetar"
os dados na classe em novas inst�ncias de uma classe
criado apenas para conter os dados retornados pela consulta.

Vamos come�ar criando a classe chamada TrackDetai 1. s
que vai conter apenas o nome do artista e o t�tulo de um
faixa. Voc� vai usar isso para segurar o resultado da pesquisa
inquerir.


```csharp
class FilmeResumido
{
    public string Diretor { get; set; }
    public string Titulo { get; set; }
}
```


A consulta pode agora ser solicitada para criar uma nova
inst�ncia desta classe para manter o resultado de cada consulta.

```csharp
var selecionados2 =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select new FilmeResumido
{
    NomeDiretor = filme.Diretor.Nome,
    Titulo = filme.Titulo
};
```


Resultados de proje��o como este s�o particularmente �teis
Quando voc� est� usando a vincula��o de dados para exibir
resultados para o usu�rio. Os valores no resultado da consulta podem ser
vinculado a itens a serem exibidos.

Tipos an�nimos

Voc� pode remover a necessidade de criar uma classe para manter o
resultado de uma consulta de pesquisa, fazendo com que a consulta retorne
resultados de um tipo an�nimo. Voc� pode ver como isso
agora est� faltando no final do novo select
declara��o.


```csharp
var selecionados3 =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select new // tipo an�nimo: tipo da proje��o n�o � necess�rio
{
    NomeDiretor = filme.Diretor.Nome,
    filme.Titulo
};
```


A consulta na LISTAGEM cria novas inst�ncias de
um tipo an�nimo que cont�m apenas os itens de dados
caso a primeira propriedade no tipo � o nome do
artista que grava a faixa, e o segundo � o t�tulo de
a faixa. Para a primeira propriedade voc� realmente fornece
o nome do campo a ser criado no novo tipo.
Para a segunda propriedade, a propriedade � criada com
mesmo nome que a propriedade de origem, neste caso o
nome da propriedade ser� 1 e.

O item que � retornado por este queiy � um
cole��o enumer�vel de inst�ncias de um tipo que tem
sem nome � um tipo an�nimo. Isso significa que voc�
tem que usar uma refer�ncia var para se referir � consulta
resultado. Voc� pode percorrer a cole��o neste
resultado como voc� faria com qualquer outro. Note que cada item
a cole��o de faixas selecionada deve ser de 110W
referido usando var porque seu tipo n�o tem nome.
O c�digo a seguir mostra como var � usado para cada item

Ao imprimir os resultados da consulta na LISTAGEM
4�34.


```csharp
foreach (var filme in selecionados3)
{
    Console.WriteLine($"{filme.Titulo,-40}{filme.NomeDiretor,-20}");
}
```


Observe que o uso de um tipo an�nimo n�o
significa que o compilador � menos rigoroso quando
verificando a exatid�o do c�digo. Se o programa
tenta usar uma propriedade que n�o est� presente no item,
por exemplo, se ele tentar obter a propriedade Length
a partir do resultado do quely, isso gera um erro em
tempo de compila��o.

Jun��o LINQ

O design de classe usado at� este ponto usa c #
refer�ncias para implementar as associa��es entre os
objetos no sistema. Em outras palavras, uma faixa de m�sica
objeto cont�m uma refer�ncia ao objeto Artist que
representa o artista que gravou aquela faixa. Se voc�s
armazenar seus dados em um banco de dados, no entanto, voc� n�o ser�
capaz de armazenar as associa��es dessa maneira.

Em vez disso, cada item no banco de dados ter� um
ID exclusivo (sua chave prim�ria) e objetos referentes a
esse objeto conter� esse valor de ID (uma chave estrangeira).
Em vez de uma refer�ncia a uma inst�ncia do Artista, o
O MusicTrack agora cont�m um campo ArtistID que
identifica o artista associado a essa faixa. Figura
4�16 mostra como esta associa��o � implementada.

olD: In!
3:53:25
Names�rmn

FIGURA 4-16 Faixas de M�sica e Identifica��o do Artista

Esse design dificulta um pouco a pesquisa
para faixas de um artista em particular. O programa precisa
encontre o valor do ID para o artista com o nome sendo
procurou e, em seguida, procure por todas as faixas com isso
valor do id do artista. Felizmente, o LINQ fornece uma jun��o
operador que pode ser usado para juntar a sa�da de um LINQ
consulta para a entrada de outro.

A LISTAGEM mostra como isso funciona. A primeira consulta
seleciona o artista Com o nome "Rob Miles". Os resultados
dessa consulta s�o unidos � segunda consulta
que pesquisa a cole��o de faixas de m�sica para faixas
Com uma propriedade stid Arti que corresponda � do
artista encontrado pela primeira quely.

LISTAGEM Jun��o LINQ


```csharp
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
```


Grupo LINQ

Outro recurso �til do LINQ � a capacidade de agrupar

resultados de uma consulta para criar uma sa�da de resumo. Para
Por exemplo, voc� pode querer criar uma consulta para saber como
muitas faixas existem por cada artista na m�sica
cole��o.
A LISTAGEM mostra como fazer isso. O grupo
a��o � dado o item de dados para agrupar por e o
propriedade por que � para ser agrupado. o
O resumo da trilha do artista cont�m uma entrada para cada
artista diferente. Cada um dos itens no resumo tem
uma propriedade Key, que � o valor que o item �
"Agrupados" ao redor. Voc� quer criar um grupo ao redor
artistas, ent�o a chave � o valor do ID do Artista de cada
faixa. A propriedade Key do
O resumo da trilha do artista fornece o valor dessa chave.
Voc� pode usar comportamentos fornecidos por um objeto de resumo
para descobrir o conte�do do resumo e
o m�todo Count retorna o n�mero de itens no
resumo. Voc� descobrir� mais resumo
comandos na discuss�o sobre o agregado
comandos mais adiante nesta se��o.

LISTAGEM grupo LINQ


```csharp
var resumoDiretor =
from filme in filmes
group filme by filme.DiretorId
into resumoFilmeDiretor
select new
{
    ID = resumoFilmeDiretor.Key,
    Count = resumoFilmeDiretor.Count()
};
```


classes produzidas por esta consulta usando um foreach
loop como mostrado a seguir.


```csharp
Console.WriteLine($"{"DiretorId",-20}{"Quantidade",10}");
Console.WriteLine(new string('=', 30));
foreach (var item in resumoDiretor)
{
    Console.WriteLine($"{item.ID,-20}{item.Count,10}");
}
Console.WriteLine();
```


O problema Com esta consulta � que quando execut�-lo
produz os resultados como mostrado a seguir. Ao inv�s de
gerando o nome do artista, o programa
exibe os valores do ID do Artista.


Artist: 0 Tracks recordedz5
Artist: 6 Tracks recorded: 5
Artist:12 Tracks recordedz5
Artist:18 Tracks recorded:5


Voc� pode consertar isso usando uma opera��o de jun��o
Isso ir� extrair o nome do artista para uso na consulta.
A jun��o necess�ria � mostrada a seguir. Voc� pode ent�o criar
o grupo digitou o nome do artista em vez do ID
para obter o resultado desejado.

```csharp
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
```

A sa�da desta consulta � mostrada aqui:


```
Nome Diretor        Quantidade
==============================
Quentin Tarantino            3
James Cameron                3
Tim Burton                   4
```


Note que esta � uma forte magia LINQ. Vale a pena
jogando com o c�digo de amostra um pouco e examinando
a estrutura da consulta para ver o que est� acontecendo.
LINQ Pegue e pule

Uma consulta LINQ normalmente retornar� todos os itens
que se encontra. No entanto, isso pode ser mais itens que
seu programa quer. Por exemplo, voc� pode querer
mostre ao usu�rio a sa�da uma p�gina por vez. Voc� pode
use take para dizer � consulta para pegar um n�mero espec�fico
de itens e o Skip para dizer uma consulta para pular um
determinado n�mero de itens no resultado antes de tomar
o n�mero solicitado.

O programa de amostra na LISTAGEM exibe todos
a m�sica acompanha dez itens de cada vez. Ele usa um loop
que usa o Skip para ir progressivamente mais abaixo
o banco de dados toda vez que o loop � repetido. O la�o
termina Quando a consulta LINQ retorna um vazio
cole��o. O usu�rio pressiona uma tecla no final de cada
p�gina para passar para a pr�xima p�gina.

LISTAGEM LINQ tomar e pular


```csharp
int numeroPagina = 0;
int tamanhoPagina = 4;

while (true)
{
    // obt�m informa��o sobre o filme
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
    // Sai do la�o while se chegar ao final dos dados

    if (listaDeFilmes.Count() == 0)
        break;

    // Exibe os resultados da consulta
    foreach (var item in listaDeFilmes)
    {
        Console.WriteLine($"{item.NomeDiretor,-30}{item.Titulo,-30}");
    }
    Console.WriteLine("Tecle algo para continuar...");
    Console.ReadKey();
    // avan�a uma p�gina
    numeroPagina++;
}
```

Comandos Agregados do LINQ

No contexto dos comandos LINQ, a palavra
agregado significa �reunir um certo n�mero de
valores para criar um �nico resultado. �Voc� pode usar
operadores sobre os resultados produzidos por
opera��es. Voc� j� usou um agregado
operador em uma consulta LINQ. Voc� usou o Conde

operador na LISTAGEM para contar o n�mero de trilhas
em um grupo extra�do pelo artista. Isso forneceu a
n�mero de faixas atribu�das a um determinado artista. Voc�
pode querer obter o comprimento total de todas as faixas
atribu�do a um artista, e para isso voc� pode usar a Soma
operador agregado.
O par�metro para o operador Sum � um lambda
express�o que o operador �mal usa em cada item
o grupo para obter o valor a ser adicionado ao total
soma para esse item. Para obter a soma do MusicTrack
comprimentos, a express�o lambda apenas retorna o valor
da propriedade Length para o item. LISTAGEM
mostra como isso funciona.

LISTAGEM Agregado LINQ


```csharp
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
```


O resultado dessa consulta � uma cole��o de
Objetos an�nimos que cont�m o nome do artista
e o comprimento total de todas as faixas gravadas por esse
artista. O programa produz a seguinte sa�da:



```
Nome Diretor                         Total Minutos
==================================================
Quentin Tarantino                              430
James Cameron                                  463
Tim Burton                                     261
```


Voc� pode usar Average, Max e Min para gerar
outros itens de informa��o agregada. Voc� tamb�m pode
crie seu pr�prio comportamento Agregado que ser�
chamado em cada item sucessivo no grupo e Will
gerar um �nico resultado agregado.
Criar consultas LINQ baseadas em m�todo

A primeira consulta LINQ que voc� viu estava na LISTAGEM
como mostrado aqui.

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;
```


A instru��o de consulta usa a compreens�o de consulta
sintaxe, que inclui os operadores de, em,
onde e selecione. O compilador usa isso para
gerar uma chamada para o m�todo Where no
Cole��o de faixas de m�sica. Em outras palavras, o c�digo
que � realmente criado para realizar a consulta � o
declara��o abaixo:


```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;

Imprimir(filmesSelecionados);
```



O m�todo Where aceita uma express�o lambda como
um par�metro. Neste caso, a express�o lambda
aceita uma faixa de m�sica como um par�metro e retorna
Verdadeiro se a propriedade Name do elemento Artist em
o Mus i cTrack corresponde ao nome que est� sendo
selecionado.

Voc� viu pela primeira vez express�es lambda na Habilidade 1.4,
�Crie e implemente eventos e retornos de chamada.�
express�o lambda � um peda�o de comportamento que pode ser
considerado como um objeto. Nesta situa��o o Onde
m�todo est� recebendo um peda�o de comportamento que o
m�todo pode usar para determinar quais faixas selecionar.

Neste caso, o comportamento � �pegar uma pista e ver se o
nome do artista � Rob Miles. �Voc� pode criar o seu pr�prio
consultas baseadas em m�todo em vez de usar o LINQ
operadores. LISTAGEM mostra a consulta LINQ e o
comportamento baseado em m�todo correspondente.

LISTAGEM Consulta baseada em m�todo


```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;
```


Implementa��o baseada em m�todo desta consulta


```csharp
IEnumerable<Filme> queryMetodo =
filmes.Where(filme => filme.Diretor.Nome == "James Cameron");
```


Programas podem usar os m�todos LINQ que (e
executar consultas LINQ) em cole��es de dados, como
listas e matrizes, e tamb�m em conex�es de banco de dados.
Os m�todos que implementam os comportamentos do LINQ
n�o s�o adicionados �s classes que os utilizam. Em vez de
eles s�o implementados como m�todos de extens�o. Voc� pode
saiba mais sobre m�todos de extens�o na Habilidade 2.1, em
a se��o �M�todos de extens�o�.

Consultar dados usando consulta
sintaxe de compreens�o

A frase �sintaxe de compreens�o de consulta� refere-se a
a maneira que voc� pode construir consultas LINQ para usar o

Operadores C # fornecidos especificamente para expressar dados
consultas. A inten��o � fazer as declara��es C #
que se assemelham fortemente �s consultas SQL que executam
a mesma fun��o. Isso facilita para os desenvolvedores
familiarizado com a sintaxe SQL para usar o LINQ.

A LISTAGEM mostra uma consulta LINQ complexa que �
com base na consulta LINQ usada na LISTAGEM para
produzir um resumo dando a dura��o da m�sica por
cada artista. Isso usa o operador orderby para pedir
a sa�da pelo nome do artista.

LISTAGEM Consulta completa


```csharp
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
```

A consulta SQL que corresponde a este LINQ � mostrada
abaixo:


```csharp
SELECT SUM([t0].[Minutos]) AS [TotalMinutos]
, [t1].[Nome] AS [Diretor]
FROM [Filme] AS [t0]
INNER JOIN [Diretor] AS [�51]
ON [t0].[DiretorId] = [t1].[Id]
GROUP BY [t1].[Nome]
```


Esta sa�da foi gerada usando o LINQPad
aplica��o que permite que os programadores criem LINQ
consultas e Ver o SQL e baseado em m�todos
implementa��es. A edi��o padr�o � muito
poderoso recurso para desenvolvedores e pode ser
baixado gratuitamente de http://www.linqpad.net/.
Selecione dados usando anonyn10us
tipos

Voc� viu pela primeira vez o uso de tipos an�nimos no
Se��o �Tipos an�nimos�, anteriormente neste cap�tulo.
Os �ltimos exemplos de programas mostraram o uso de
tipos an�nimos movendo-se da cria��o de valores que
resumir o conte�do de um objeto de dados de origem
exemplo extraindo apenas o artista e t�tulo
informa��es de um valor do MusicTrack), para criar
tipos completamente novos que cont�m dados do
banco de dados e os resultados dos operadores agregados.

� importante notar que voc� tamb�m pode criar
inst�ncias de tipo an�nimas no SQL baseado em m�todo
consultas. LISTAGEM mostra o m�todo baseado em
implementa��o da consulta da LISTAGEM;
tipo an�nimo � mostrado em negrito. Observe o uso de um
classe an�nima intermedi�ria que � usada para
implementar a jun��o entre as duas consultas e
gerar objetos que contenham artista e faixa
em forma��o.

LISTAGEM Tipos an�nimos complexos


```csharp
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
```


For�ar execu��o de uma consulta

O resultado de uma consulta LINQ � um item que pode ser
iterado. N�s usamos a constru��o foreach para
exibir os resultados das consultas. A avalia��o real
de uma consulta LINQ normalmente s� ocorre quando
programa come�a a extrair resultados da consulta. este
� chamado de execu��o adiada. Se voc� quiser for�ar o
execu��o de uma consulta, voc� pode usar o ToArray ()
m�todo conforme mostrado na LISTAGEM. A consulta �
executada e o resultado retornado como uma matriz.

LISTAGEM For�ar execu��o de consulta


```csharp
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
```

Note que, no caso deste exemplo, o resultado ser�
resultado. O programa foi pausado logo ap�s a
a vari�vel Resultado do acompanhamento do artista foi definida como
resultado da consulta, e o depurador est� mostrando o conte�do
do artista Track Result.


[IMAGEM]

FIGURA 4-17 Resultados imediatos da consulta

Um resultado da consulta tamb�m fornece ToList e
ToDictionary m�todos que for�ar� a execu��o de
a consulta e gerar um resultado imediato desse tipo.
Se uma consulta retornar um valor singleton (por exemplo,
resultado de uma opera��o de agrega��o, como soma ou
contagem) ser� imediatamente avaliado.
Leia, filtre, crie e modifique
estruturas de dados usando LINQ para
XML

Nesta se��o, vamos investigar o LINQ para
Recursos XML que permitem usar o LINQ
constru��es para analisar documentos XML. As classes
que fornecem esses comportamentos est�o no
Sistema. XML Namespace Linq.
Exemplo de Documento XML
O documento XML de amostra � mostrado a seguir. Cont�m
dois itens do Musictrack que s�o mantidos dentro de
Elemento Music Tracks. O texto da amostra
documento � armazenado em uma vari�vel de cadeia chamada
XMLText.

```csharp
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
```

XDocument

Na se��o anterior, "Consumir dados XML" voc�
Aprendi a consumir dados XML em um programa
usando a classe XMLDocument. Esta classe foi
substitu�do em vers�es posteriores do .NET (vers�o 3.5
em diante) pela classe XDocument, que permite a
uso de consultas LINQ para analisar arquivos XML.
Um programa pode criar uma inst�ncia do XDocument que
representa o documento anterior usando o Parse
m�todo fornecido pela classe XDocument como mostrado
Aqui.


```csharp
XDocument documentoFilmes = XDocument.Parse(xmlText);
```


O formato das consultas LINQ � um pouco diferente
Ao trabalhar com XML. Isso porque a fonte

da consulta � um conjunto filtrado de entradas XML do
documento Fonte. A LISTAGEM mostra como isso funciona.
A consulta seleciona todos os elementos "Music Track" de
o documento de origem. O resultado da consulta � um
enumera��o de itens do XElement que foram
extra�do do documento. A classe XElement �
um desenvolvimento da classe XMLElement que inclui
Comportamentos XML. O programa usa um foreach
constru��o para trabalhar atrav�s da cole��o de
Resultados XElement, extraindo os valores de texto necess�rios.

LISTAGEM: Ler XML com LINQ


```csharp
IEnumerable<XElement> filmesSelecionados2 =
from filme in documentoFilmes.Descendants("Filme")
select filme;
foreach (XElement item in filmesSelecionados2)
{
    Console.WriteLine("Diretor: {0}, T�tulo: {1} ",
        item.Element("Diretor").FirstNode,
        item.Element("Titulo").FirstNode);
}
```


Filtrar dados XML com o LINQ para XML

O programa na LISTAGEM exibe todo o
conte�do do documento XML. Um programa pode
realizar filtragem na consulta, adicionando um onde
operador, assim como com o LIN Q que vimos antes.
A LISTAGEM mostra como isso funciona. Note que o
Onde a opera��o tem que extrair o valor de dados de
o elemento para que ele possa realizar a compara��o.

LISTAGEM: Filtrar XML com LINQ

```csharp
filmesSelecionados2 =
from filme in documentoFilmes.Descendants("Filme")
where (string)filme.Element("Diretor") == "Quentin Tarantino"
select filme;
foreach (XElement item in filmesSelecionados2)
{
    Console.WriteLine("Diretor: {0}, T�tulo: {1} ",
        item.Element("Diretor").FirstNode,
        item.Element("Titulo").FirstNode);
}
Console.WriteLine();
```


As consultas LINQ que temos visto at� agora t�m
foi expressa usando a compreens�o de consulta. Isto �
poss�vel, no entanto, expressar a mesma pergunta no
forma de uma consulta baseada em m�todo. Os descendentes
m�todo retorna um objeto que fornece o Onde
comportamento. O c�digo a seguir mostra a consulta na LISTAGEM
44 implementado como uma consulta baseada em m�todo.


```csharp
filmesSelecionados2 =
from filme in documentoFilmes.Descendants("Filme")
.Where(elemento =>(string)elemento.Element("Diretor")
    == "Quentin Tarantino")
select filme;
foreach (XElement item in filmesSelecionados2)
{
    Console.WriteLine("Diretor: {0}, T�tulo: {1} ",
        item.Element("Diretor").FirstNode,
        item.Element("Titulo").FirstNode);
}
```


Crie XML com o LINQ para XML

Os recursos LINQ to XML incluem uma maneira muito f�cil de
criar documentos XML. O c�digo na LISTAGEM
cria um documento exatamente como o XML de amostra para
esta se��o. Note que o arranjo do
chamadas de construtor para cada item XElement espelham
estrutura do documento.

LISTAGEM Criar XML com LINQ

```csharp
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
```


Modificar dados com o LINQ para XML

A classe XElement fornece m�todos que podem ser
usado para modificar o conte�do de um determinado elemento XML.
O programa na LISTAGEM cria um que
identifica todos os itens da faixa de m�sica que t�m
t�tulo "meu caminho" e, em seguida, usa o ReplaceWith
m�todo sobre os dados do t�tulo no elemento para mudar o
t�tulo para o t�tulo correto, que � "meu caminho".

LISTAGEM Modificar XML com LINQ

```csharp
filmesSelecionados2 =
from filme in documentoFilmes.Descendants("Filme")
select filme;
foreach (XElement item in filmesSelecionados2)
{
    item.Element("Titulo").FirstNode.ReplaceWith("Novo n�");
}

foreach (XElement item in filmesSelecionados2)
{
    Console.WriteLine("Diretor: {0}, T�tulo: {1} ",
        item.Element("Diretor").FirstNode,
        item.Element("Titulo").FirstNode);
}
Console.WriteLine();
```


Como voc� viu Ao criar um novo documento XML,
um XElement pode conter uma cole��o de outros
elementos para construir a estrutura da �rvore de um XML
documento. Voc� pode adicionar programaticamente e
remover elementos para alterar a estrutura do XML
documento.

Suponha que voc� decida adicionar um novo elemento de dados
para Musictrack. Voc� quer armazenar o "g�nero" do
c�digo na LISTAGEM encontra todos os itens em nossa
dados de amostra que est�o faltando um elemento de estilo e, em seguida,
adiciona o elemento ao item.

LISTAGEM: Adicionar XML com LINQ


```csharp
var filmesSelecionados3 =
from filme in documentoFilmes.Descendants("Filme")
select filme;
foreach (XElement item in filmesSelecionados3)
{
    item.Add(new XElement("Genero", "Drama"));
}

foreach (XElement item in filmesSelecionados3)
{
    Console.WriteLine("Diretor: {0}, T�tulo: {1}, G�nero: {2} ",
        item.Element("Diretor").FirstNode,
        item.Element("Titulo").FirstNode,
        item.Element("Genero").FirstNode);
}
Console.WriteLine();
```


