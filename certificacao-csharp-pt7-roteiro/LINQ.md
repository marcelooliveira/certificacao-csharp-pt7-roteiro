# Consultar e manipular dados e objetos usando o LINQ

A Consulta Integrada � Linguagem, ou LINQ, foi criada para
tornar f�cil o trabalho de programadores C# com
fontes de dados.

Vamos trabalhar com um programa que vai obter dados a partir
de 2 listas:

- 1 lista com dados de filmes
- 1 lista com diretores de filmes

O c�digo a seguir mostra o c�digo C# para as classes que representam o diretor e o filme.

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

Al�m dos dados de testes, tamb�m disponibilizamos no in�cio do curso
dois m�todos `GetDiretores()` e `GetFilmes()`, que fornecem as listas de diretores e de filmes:

```csharp
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

Vamos come�ar nosso programa criando uma inst�ncia para cada lista:

```csharp
static void Main(string[] args)
{
    List<Diretor> diretores = GetDiretores();
    List<Filme> filmes = GetFilmes();
}
```

Mas qual o conte�do dessas listas? Vamos criar um la�o para pegar cada um dos
elementos de `filmes` e imprimir seus dados no console:

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
```

Rodando a aplica��o, temos nosso primeiro resultado:

![File3](file3.png)

Mas note que esse c�digo que imprime os filmes poder� ser utilizado depois, ent�o podemos
evitar a repeti��o desse algoritmo extraindo-o para um novo m�todo `Imprimir()`::

```csharp
static void Main(string[] args)
{
    List<Diretor> diretores = GetDiretores();
    List<Filme> filmes = GetFilmes();

    Imprimir(filmes);

    Console.ReadKey();
}

private static void Imprimir(List<Filme> filmes)
{
    Console.WriteLine($"{"T�tulo",-40}{"Diretor",-20}{"Ano",4}");
    Console.WriteLine(new string('=', 64));
    foreach (var filme in filmes)
    {
        Console.WriteLine($"{filme.Titulo,-40}{filme.Diretor.Nome,-20}{filme.Ano}");
    }
    Console.WriteLine();
}
```


**Use um inicializador de objeto**

Se voc� olhar o c�digo na LISTAGEM, ver� que
estamos usando a sintaxe do inicializador de objetos para criar novos
inst�ncias dos objetos de filme e inicializar seus
valores ao mesmo tempo:

```csharp
new Filme {
    DiretorId = 1,
    Diretor = new Diretor { Nome = "Quentin Tarantino" },
    Titulo = "Pulp Fiction",
    Ano = 1994,
    Minutos = 2 * 60 + 34
}
```

Este � um recurso muito �til
que permite inicializar objetos quando eles
s�o criados sem a necessidade de criar um construtor
na classe que est� sendo inicializada.

O c�digo a seguir mostra como funciona. A declara��o
cria e inicializa uma nova inst�ncia do Filme.
Observe o uso de chaves ( { e } ) para delimitar os itens
que inicializam a inst�ncia e v�rgulas para separar
cada valor sendo usado para inicializar o objeto.

```csharp
Filme novoFilme = new Filme
{
    Diretor = new Diretor { Nome = "Tim Burton" },
    Titulo = "A Fant�stica F�brica de Chocolate",
    Ano = 2005
};
filmes.Add(novoFilme);
```

Voc� n�o precisa inicializar todos os elementos do
inst�ncia: quaisquer propriedades n�o inicializadas s�o definidas com seus
valores-padr�o (zero para um valor num�rico e nulo para strings).

(Todas as propriedades a serem inicializadas dessa maneira
devem ser membros p�blicos da classe)

**Usando um operador LINQ**

Agora que voc� tem alguns dados, pode come�ar a usar operadores LINQ
para construir consultas e extrair resultados desses dados.

Vamos declarar uma vari�vel `filmesSelecionados` para armazenar nossa consulta.
Inicialmente, a consulta cont�m TODOS os elementos da lista de filmes:

```csharp
IEnumerable<Filme> filmesSelecionados = filmes;
Imprimir(filmesSelecionados);
```

Como resultado, temos:

![File5](file5.png)

Agora faremos uma pequena altera��o: em vez de pegar o resultado diretamente da lista
de filmes, vamos **criar uma consulta** sobre essa lista de filmes. Como seria
essa consulta, se voc� estivesse trabalhando com SQL Server?

```
SELECT f.*
FROM filmes AS f
WHERE NomeDiretor = 'Tim Burton'
```

Onde:

- A cl�usula SELECT define a "PROJE��O", isto �, quais colunas devem ser retornadas no resultado
- A cl�usula FROM define a origem dos dados
- A letra "f" representa o ALIAS para a tabela de filmes
- A cl�usula WHERE filtra os dados

As consultas LINQ usam uma sintaxe parecida:

```csharp
IEnumerable<Filme> filmesSelecionados
    = select f from filmes as f;
```

Por�m, uma consulta LINQ sempre deve come�ar pela cl�usula FROM:

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filmes as f select f;
```

Al�m disso, o ALIAS da tabela de filmes tem a sintaxe "[alias] in [dados]":

```csharp
IEnumerable<Filme> filmesSelecionados
    = from f in filmes select f;
Imprimir(filmesSelecionados);
```

Rodando a aplica��o, teremos o mesmo resultado.

O pr�ximo passo � filtrar pelo nome do diretor do filme.

Podemos acessar as propriedades de um filme atrav�s do seu alias,
que funciona como uma vari�vel tempor�ria dentro da consulta.

Vamos introduzir a cl�usula `where` para filtrar pelos filmes de um
diretor:

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;

Imprimir(filmesSelecionados);
```

Rodando a aplica��o, temos agora o resultado filtrado:

![File6](file6.png)

O c�digo na LISTAGEM imprime os t�tulos de todos os
filmes que foram gravadas pelo diretor com o nome
"Tim Burton". A primeira declara��o usa uma consulta LINQ para
criar uma cole��o enumer�vel de Filme.

A consulta LINQ retornou um resultado IEnumerable, e por isso 
ele pode ser enumerado por uma constru��o foreach.

**Usando a palavra-chave var para tipos impl�citos com o LINQ**

A linguagem C# � "estaticamente tipada". Por isso, o tipo de
objetos em um programa � determinado em tempo de compila��o
e o compilador rejeita quaisquer a��es que n�o forem v�lidas.

Na consulta, declaramos a vari�vel como `IEnumerable<Filme>`,
mas voc� pode simplificar o c�digo usando a palavra-chave `var` para
dizer ao compilador para inferir o tipo da vari�vel sendo
criado a partir do contexto em que a vari�vel � usada.

A palavra-chave var � especialmente �til quando trabalhamos com
LINQ. O resultado de uma consulta LINQ simples � um
cole��o enumer�vel do tipo de elemento de dados mantido
na fonte de dados. A declara��o a seguir mostra nossa
consulta atual.


```csharp
IEnumerable<Filme> selecionados =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select filme;
```


Para escrever esta declara��o, voc� deve primeiro descobrir o tipo
de dados na cole��o de filmes e, em seguida, usar
esse tipo com `IEnumerable<Filme>`.

A palavra-chave `var` torna este c�digo mais f�cil de escrever:

```csharp
var selecionados =
from filme in filmes
where filme.Diretor.Nome == "Tim Burton"
select filme;
```

**Proje��o LINQ**

No exemplo acima, a consulta produz o mesmo objeto contido na
cole��o de origem (filmes).

Mas voc� pode usar a opera��o de sele��o no LINQ para
produzir uma vers�o *transformada* de cada objeto da fonte de dados. 
Em vez de retornar um Filme, voc� pode retornar um outro tipo de objeto
usando os dados de Filme.

O nome desse processo de tranformar os dados da sa�da � **proje��o LINQ**.

Vamos demonstrar isso criando a classe chamada FilmeResumido,
que vai conter apenas o nome do diretor e o t�tulo de um
filme.

```csharp
class FilmeResumido
{
    public string Diretor { get; set; }
    public string Titulo { get; set; }
}
```

Essa ser� a classe usada na nossa **proje��o de dados** com a cl�usula select da consulta LINQ.

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

Esse tipo de consulta � bem comum quando temos "dados brutos" em banco
de dados, mas queremos exibir ao usu�rio os dados "transformados", num formato amig�vel.

**Tipos an�nimos**

Observe a cl�usula de "proje��o" (select) desta consulta LINQ

```csharp
var selecionados2 =
from ...
select new FilmeResumido
{
    NomeDiretor = filme.Diretor.Nome,
    Titulo = filme.Titulo
};
```

Nesse caso, fomos obrigados a criar um tipo novo, a classe FilmeResumido, para
armazenar os dados do filme resumido.

Mas muitas vezes isso n�o � obrigat�rio. Vamos remover o nome da classe FilmeResumido no instanciamento do select:

```csharp
var selecionados2 =
from ...
select new // tipo an�nimo: tipo da proje��o n�o � obrigat�rio
{
    NomeDiretor = filme.Diretor.Nome,
    Titulo = filme.Titulo
};
```

A consulta acima cria novas inst�ncias de
um **tipo an�nimo** que cont�m apenas os itens de dados:
- nome do diretor que dirigiu o filme
- t�tulo do filme.

Observe que, para a primeira propriedade, voc� forneceu
o nome do campo a ser criado no novo tipo (NomeDiretor). J� a
segunda propriedade foi criada com o
**mesmo nome** que a propriedade de origem.
Quando n�o h� redefini��o de nome, o compilador presume o nome
da propriedade `Titulo`.

O resultado retornado por esta query � uma
cole��o enumer�vel de inst�ncias de um tipo sem nome.
� um **tipo an�nimo**. Isso significa que voc�
tem que usar uma **refer�ncia var** para se referir ao 
resultado da consulta.

Voc� pode percorrer a cole��o neste
resultado como voc� faria com qualquer outro. Note que cada item
da cole��o de filmes selecionada deve ser agora
referido usando **var** na instru��o `foreach` porque seu tipo n�o tem nome.
O c�digo a seguir mostra como `var` � usado para cada item:


```csharp
foreach (var filme in selecionados3)
{
    Console.WriteLine($"{filme.Titulo,-40}{filme.NomeDiretor,-20}");
}
```


Observe que, mesmo quando o tipo � an�nimo, n�o
significa que o compilador vai deixar de ser rigoroso com a
exatid�o do c�digo.

Enquanto digitamos, as 2 propriedades (`Titulo` e `NomeDiretor`) s�o
verificadas pelo compilador. N�o podemos inventar e utilizar aqui
propriedades que n�o existem no `select` da consulta LINQ anterior.

**Jun��o LINQ**

At� aqui, trabalhamos com a classe `Filme`, que possui como propriedade
o `Diretor`, que � uma inst�ncia da classe `Diretor`:

```csharp
class Filme
{
    public Diretor Diretor { get; set; }
    public string Titulo { get; set; }
}
```

No entanto, se voc� est� trabalhando com um banco de dados, 
muitas vezes voc� n�o pode armazenar as associa��es dessa maneira.

Em vez disso, cada item no banco de dados ter� um
ID exclusivo (sua chave prim�ria) e objetos referentes a
esse objeto conter� esse valor de ID (uma chave estrangeira).

Em vez de uma *refer�ncia* a uma inst�ncia do `Diretor`, o
o `Filme` agora cont�m um campo `DiretorId` que
identifica o diretor associado a esse filme.

```csharp
class Filme
{
    public int DiretorId { get; set; }
    public string Titulo { get; set; }
}

```

Esse design dificulta um pouco a pesquisa
para filmes de um diretor em particular. O programa precisa
encontrar o valor do ID para o diretor com um determinado nome sendo
procurado e, em seguida, procurar por todos os filmes com esse
valor de `DiretorId`.

Felizmente, o LINQ fornece um **operador de jun��o** (`join`)
que pode ser usado para *juntar* a sa�da de uma consulta LINQ
com a *entrada* de outra consulta.

A cl�usula *join* da consulta � definida pela sintaxe:

```csharp
join b in [OrigemB]
on b.CampoB equals a.CampoA
```

Aplicando em nossa consulta, a `OrigemB` � a lista `diretores`,
e os campos `CampoA` e `CampoB` s�o `filme.DiretorId` e
`diretor.Id`, respectivamente. Esses campos s�o as propriedades
onde � realizada a *jun��o*.

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

Note que a cl�usula `select` agora foi modificada, de forma
que cada propriedade do objeto an�nimo de sa�da (proje��o LINQ)
vem de uma entidade diferente do **join**:

```csharp
select new
{
    NomeDiretor = diretor.Nome,
    filme.Titulo
};
```

**Grupo LINQ**

Outro recurso �til do LINQ � a capacidade de agrupar

Imagine uma nova consulta, descrita em portugu�s como:

> ***"Traga-me o Id de cada diretor, ao lado da quantidade de filmes
> dirigidos desse diretor"***

Como implementar essa consulta com LINQ?

Primeiro temos que lembrar que cada elemento da nossa consulta atual
representa *um filme individual* de um diretor.

Precisamos modificar a consulta para que cada linha do resultado
seja um **agrupamento** para um �nico diretor. 
 
Para isso, temos que usar a sintaxe de agrupamento da **cl�usula group** do LINQ:

Primeiro, come�amos com a origem de dados, contendo todos os dados de filmes.

```csharp
var resumoDiretor =
from filme in filmes
```

Em seguida, agrupamos os filmes por *id de diretor*, e armazenamos
o resultado do agrupamento em uma *vari�vel de consulta* chamada
`resumoFilmeDiretor`:

```csharp
var resumoDiretor =
from filme in filmes
group filme by filme.DiretorId
into resumoFilmeDiretor
```

Essa vari�vel `resumoFilmeDiretor` � vis�vel apenas dentro da consulta.

O pr�ximo passo � fornecer informa��es para as 2 colunas de dados
que precisamos:

- O Id do diretor
- A quantidade de filmes desse diretor

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

Para obtermos esse resultado, foi necess�rio obter a *chave (key)*
do agrupamento, e invocar uma das *fun��es de agrega��o (Count)* 
que est�o presentes quando realizamos um agrupamento.

![File7](file7.png)


Para listar os resultados desta �ltima consulta, vamos percorrer
o resultado da consulta com a instru��o `foreach`:

```csharp
Console.WriteLine($"{"DiretorId",-20}{"Quantidade",10}");
Console.WriteLine(new string('=', 30));
foreach (var item in resumoDiretor)
{
    Console.WriteLine($"{item.ID,-20}{item.Count,10}");
}
Console.WriteLine();
```

Note que cada elemento cont�m o Id do diretor e a quantidade de filmes.

```
DiretorId           Quantidade
==============================
1                            3
2                            3
3                            3
```

Mas um relat�rio com o id do diretor n�o � muito �til, porque n�o sabemos quem � cada diretor.

Voc� pode consertar isso introduzindo na consulta uma *opera��o de jun��o*.
Isso ir� extrair o nome do diretor para uso na consulta.

A jun��o necess�ria � mostrada a seguir. Voc� pode ent�o criar
o grupo com o nome do diretor em vez do ID
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

**LINQ Take e Skip**

Uma consulta LINQ normalmente retornar� todos os itens
que se encontra. No entanto, esse tipo de consulta pode 
trazer mais itens que seu programa precisa.

Por exemplo, voc� pode desenvolver uma consulta para um relat�rio 
enorme a ser exibido numa p�gina web, e querer mostrar ao usu�rio
o conte�do de **uma p�gina por vez**.

Voc� pode usar o m�todo `Take()` para dizer � consulta para pegar
um n�mero  espec�fico de itens e o `Skip()` para dizer uma consulta
para pular um determinado n�mero de itens no resultado antes de obter
a quantidade solicitada.

Geralmente, os m�todos Take() e Skip() s�o usados em algoritmos de
pagina��o. A sintaxe simplificada de uma consulta LINQ com pagina��o �:

```csharp
from d in dados
dados.Skip(NUMERO_DA_PAGINA * TAMANHO_PAGINA).Take(TAMANHO_PAGINA)
```

Podemos agora aplicar essa pagina��o em nossa consulta:

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

O programa exibe dez itens de filmes cada vez.
Ele usa um loop que usa o m�todo `Skip()` para ir trazendo
progressivamente mais p�ginas a partir do banco de dados 
toda vez que o loop � repetido.

O la�o termina quando a consulta LINQ retorna uma cole��o vazia.
O usu�rio pressiona uma tecla no final de cada p�gina para passar
para a pr�xima p�gina.

**Comandos Agregados do LINQ**

No contexto dos comandos LINQ, a palavra
**agregado** significa "reunir um certo n�mero de
valores para criar um �nico resultado"

Voc� pode usar operadores sobre os resultados produzidos por
opera��es. Voc� j� usou um operador agregado
em uma consulta LINQ. Voc� usou o
operador `Count()` para contar o n�mero de filmes
em um grupo extra�do pelo diretor.

Isso forneceu o n�mero de filmes atribu�dos a um determinado diretor.
Voc� pode querer obter o comprimento total de todas os filmes
atribu�do a um diretor, e para isso voc� pode usar o
operador agregado `Sum()`.

```csharp
select new
{
    Diretor = resumoDiretorFilme.Key,
    TotalMinutos = resumoDiretorFilme.Sum(x => x.Minutos)
};
```

O par�metro para o operador `Sum` � uma *express�o lambda*
que o grupo usa para obter o valor a ser adicionado � soma total
para esse item.

Para obter a soma das dura��es dos filmes, a express�o lambda apenas 
retorna o valor da propriedade `Length` para o item.

**LISTAGEM LINQ Agregado**

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
Objetos an�nimos que cont�m o nome do diretor
e o comprimento total de todas os filmes dirigidos por esse
diretor. O programa produz a seguinte sa�da:



```
Nome Diretor                         Total Minutos
==================================================
Quentin Tarantino                              430
James Cameron                                  463
Tim Burton                                     261
```


Voc� pode usar `Average`, `Max` e `Min` para gerar
outros itens de informa��o agregada:

```csharp
var resumoDoDiretor2 =
from filme in filmes
join diretor in diretores
on filme.DiretorId equals diretor.Id
group filme by diretor.Nome
into resumoDiretorFilme
select new
{
    Diretor = resumoDiretorFilme.Key,
    TotalMinutos = resumoDiretorFilme.Sum(x => x.Minutos),
    MediaMinutos = (int)resumoDiretorFilme.Average(x => x.Minutos),
    MinMinutos = resumoDiretorFilme.Min(x => x.Minutos),
    MaxMinutos = resumoDiretorFilme.Max(x => x.Minutos)
};

Console.WriteLine($"{"Nome Diretor",-30}{"Total",10}{"M�dia",10}{"M�nimo",10}{"M�ximo",10}");
Console.WriteLine(new string('=', 70));
foreach (var item in resumoDoDiretor2)
{
    Console.WriteLine($"{item.Diretor,-30}{item.TotalMinutos,10}{item.MediaMinutos,10}{item.MinMinutos,10}{item.MaxMinutos,10}");
}
Console.WriteLine();

```

O que d� o resultado:

```
Nome Diretor                       Total     M�dia    M�nimo    M�ximo
======================================================================
Quentin Tarantino                    430       143       111       165
James Cameron                        463       154       107       194
Tim Burton                           261        87        76       108
```

**Criar consultas LINQ baseadas em m�todo**

A primeira consulta LINQ que voc� viu era assim:

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "Tim Burton"
        select filme;
```


Essa instru��o usa um tipo de **sintaxe de consulta**,
que inclui os operadores `from`, `in`,
`where` e `select`. O compilador usa isso para
gerar uma chamada para o m�todo `Where()` na
cole��o de `filmes`. Em outras palavras, o c�digo
que � **realmente criado** para realizar a consulta � a
declara��o abaixo:


```csharp
IEnumerable<Filme> filmesSelecionados =
    filmes.Where(filme => filme.Diretor.Nome == "Tim Burton")
        select filme;

Imprimir(filmesSelecionados);
```



O m�todo `Where()` aceita uma express�o lambda como
um par�metro. Neste caso, a express�o lambda
aceita uma filme como um par�metro e retorna
Verdadeiro se a propriedade `Nome` do elemento `Diretor` no 
`Filme` corresponde ao nome que est� sendo selecionado.

O m�todo `Where()` est� recebendo **um peda�o** de comportamento
que o m�todo pode usar para determinar quais filmes selecionar.

Neste caso, o comportamento � "pegar um filme e ver se o
nome do diretor � Tim Burton".

Voc� pode criar a sua pr�pria **consulta baseada em m�todo** em vez 
de usar **operadores LINQ**.

Abaixo vemos uma consulta LINQ e o
comportamento baseado em m�todo correspondente.

```csharp
IEnumerable<Filme> filmesSelecionados
    = from filme in filmes
        where filme.Diretor.Nome == "James Cameron"
        select filme;
```


Implementa��o baseada em m�todo desta consulta:


```csharp
IEnumerable<Filme> queryMetodo =
filmes.Where(filme => filme.Diretor.Nome == "James Cameron");
```


Programas podem usar os m�todos LINQ (e
executar consultas LINQ) em cole��es de dados, como
listas e matrizes, e tamb�m em conex�es de banco de dados.

> Importante: Os m�todos que implementam os comportamentos do LINQ
> n�o s�o adicionados �s classes que os utilizam (ex.: Listas, Dicion�rios, etc.). 
> Em vez disso, eles s�o implementados como **m�todos de extens�o**.


**Consultar dados usando sintaxe de consulta**

A frase �sintaxe de consulta� refere-se a
a maneira que voc� pode construir consultas LINQ para usar 
operadores C# fornecidos especificamente para expressar 
consultas de dados (from, join, select, etc).

A inten��o � fazer as declara��es C#
que se assemelham �s consultas SQL que executam
a mesma fun��o (como no SQL Server). Isso facilita a vida dos
desenvolvedores familiarizado com a sintaxe SQL para usar o LINQ.

A listagem abaixo mostra uma consulta LINQ complexa que �
feita com base na consulta LINQ usada na listagem para
produzir um resumo, retornando a dura��o da filme por
cada diretor. Ela usa o operador `orderby` para solicitar
a sa�da pelo nome do diretor.

**LISTAGEM Consulta complexa**

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


Esta sa�da foi gerada usando um aplicativo bem �til chamado **LINQPad**.
Ele que permite que os programadores criem consultas LINQ
e ver o SQL gerado a partir do LINQ. o LINQPad pode ser
baixado gratuitamente de http://www.linqpad.net/.

**Selecione dados usando an�nimos tipos**

J� vimos como trabalhar com �Tipos an�nimos�, anteriormente.

Os �ltimos exemplos de programas mostraram o uso de
tipos an�nimos desde a cria��o de valores que
resumir o conte�do de um objeto de dados de origem
exemplo, extraindo apenas informa��es do diretor e t�tulo
de um Filme, para criar tipos completamente novos que cont�m 
dados do banco de dados e os resultados dos operadores agregados.

� importante notar que voc� tamb�m pode criar
inst�ncias de tipo an�nimas no SQL baseado em m�todo
consultas. A listagem mostra o m�todo **baseado em
implementa��o da consulta** da listagem. O tipo an�nimo � 
mostrado em negrito. Observe o uso de um
classe an�nima intermedi�ria que � usada para
implementar a jun��o entre as duas consultas e
gerar objetos que contenham diretor e filme 
em forma��o.

**LISTAGEM Tipos an�nimos complexos**


```csharp
var resumoDiretorPorMetodo =
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


**For�ar a execu��o de uma consulta**

Quando criamos uma consulta LINQ, seu resultado pode ser
percorrido com uma instru��o `foreach`.

Por�m, a avalia��o **real** de uma consulta LINQ normalmente 
s� ocorre quando o programa **come�a a extrair** resultados da consulta.

Isto � chamado de **execu��o adiada**. Se voc� quiser for�ar o
execu��o de uma consulta, voc� pode usar o m�todo `ToArray()`
conforme mostrado abaixo.

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

A consulta � executada e o resultado retornado como **uma matriz**.

O programa foi pausado logo ap�s a vari�vel Resultado do acompanhamento 
do diretor foi definida como resultado da consulta, e o depurador est� 
mostrando o conte�do da vari�vel `diretorFilme`.


![File8](file8.png)


```
James Cameron                 Avatar
James Cameron                 Titanic
James Cameron                 O Exterminador do Futuro
```


**FIGURA Resultados imediatos da consulta**

O resultado da consulta tamb�m fornece os m�todos `ToList()` e
`ToDictionary()`, que **for�ar�o a execu��o** da consulta e gerar 
um **resultado imediato** desse tipo.

Se uma consulta retornar um valor *singleton* (por exemplo,
resultado de uma opera��o de agrega��o, como soma ou
contagem), esse valor **ser� imediatamente avaliado**.

**Ler, filtrar, criar e modificar estruturas de dados usando LINQ para
XML**

Vamos come�ar a investigar recursos que permitem usar constru��es do LINQ
para analisar documentos XML.

As classes que fornecem esses comportamentos est�o no namespace 
Sistem.XML.Linq.

**Exemplo de Documento XML**

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

Este documento XML cont�m dois itens do `Filme` que s�o mantidos dentro
de elemento `Filmes`. O texto do documento � armazenado em 
uma vari�vel string chamada `xmlText`.

**Ler XML com LINQ para XML e XDocument**

Anteriormente neste curso, voc�
aprendeu a consumir dados XML em um programa
usando a classe `XMLDocument`.

Esta classe foi **substitu�da** em vers�es posteriores do .NET 
(vers�o 3.5 em diante) pela classe `XDocument`, que permite o
uso de consultas LINQ para analisar arquivos XML.

Um programa pode criar uma inst�ncia do `XDocument` que
representa o documento anterior usando o m�todo `Parse`
fornecido pela classe XDocument como mostrado
aqui:

```csharp
XDocument documentoFilmes = XDocument.Parse(xmlText);
```

O formato das consultas LINQ � um pouco diferente
quando trabalhamos com XML. Isso porque a fonte
da consulta � um conjunto filtrado de **entradas XML** do
documento de origem:

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

A listagem mostra como isso funciona. A consulta seleciona todos 
os elementos "Filme" de
o documento de origem. O resultado da consulta � um
enumera��o de itens do `XElement` que foram
extra�do do documento.

A classe XElement � um desenvolvimento da classe XMLElement que inclui
Comportamentos XML. O programa usa uma constru��o foreach
para trabalhar atrav�s da cole��o de resultados `XElement`, extraindo 
os valores de texto necess�rios.


**Filtrar dados XML com o LINQ para XML**

O programa na LISTAGEM exibe todo o
conte�do do documento XML. Um programa pode
realizar filtragem na consulta, adicionando um
operador `where`, assim como com o LINQ que vimos antes.

A listagem mostra como isso funciona. Note que o 
operador `Where` tem que extrair o valor de dados do
elemento para que ele possa realizar a compara��o.

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


As consultas LINQ que temos visto at� agora foram criadas 
usando a compreens�o de consulta. Isto �
poss�vel, no entanto, expressar a mesma query na
forma de uma consulta baseada em m�todo.

O m�todo `Descendants` retorna um objeto que fornece o 
comportamento `Where`. O c�digo a seguir mostra a consulta na listagem
implementado como uma *consulta baseada em m�todo*.


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


**Crie XML com o LINQ para XML**

Os recursos LINQ to XML incluem uma maneira muito f�cil de
criar documentos XML. O c�digo a seguir cria um documento exatamente 
como o XML de amostra para esta se��o. Note que o arranjo do
chamadas de construtor para cada item `XElement` espelham
estrutura do documento.

**LISTAGEM Criar XML com LINQ**

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


**Modificar dados com o LINQ para XML**

A classe `XElement` fornece m�todos que podem ser
usado para modificar o conte�do de um determinado elemento XML.
O programa na listagem cria uma consulta que identifica todos os itens 
do filme que t�m t�tulo "meu caminho" e, em seguida, usa o m�todo 
`ReplaceWith` sobre os dados do t�tulo no elemento para mudar o t�tulo
para um outro t�tulo.

**LISTAGEM Modificar XML com LINQ**

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
um `XElement` pode conter uma cole��o de outros
elementos para construir a estrutura da �rvore de um documento XML.

Voc� pode adicionar e remover programaticamente elementos para 
alterar a estrutura do documento XML.

Suponha que voc� decida adicionar um novo elemento de dados
para `Filme`. Voc� quer armazenar o "g�nero" do filme para todos os
elementos `Filme`.

**LISTAGEM: Adicionando XML com LINQ**


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


