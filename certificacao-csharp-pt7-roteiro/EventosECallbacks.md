# Criar e implementar enventos e callbacks

Vamos come�ar o curso com um uma simples classe chamada `Campainha`, cuja
�nica fun��o � tocar. 

```csharp
class Campainha
{
    public void Tocar()
    {
        Console.WriteLine("A campainha tocou.");
    }
}
```

Ent�o criamos um programa simples para utilizar essa classe:

```csharp
class Program
{
    static void Main(string[] args)
    {
        Campainha campainha = new Campainha();
        campainha.Tocar();
        Console.ReadKey();
    }
}
```

Note como a execu��o do programa segue uma ordem sequencial:

1. E vamos criar um programa que instanciando essa classe
2. Em seguida, tocamos a campainha, chamando o m�todo da classe
3. Depois, esperamos o usu�rio teclar algo

At� aqui, voc� viu que a execu��o de um programa avan�a de instru��o para
declara��o, processando os dados de acordo com o
declara��es que s�o executadas. Na computa��o tradicional,
o programa flui do in�cio ao fim, come�ando
com os dados de entrada e produzindo alguma sa�da antes
de terminar. 

Agora imagine que voc� precise executar um c�digo dentro da classe `Program`,
que ser� chamado sempre que o m�todo `Campainha`, no lugar do pseudoc�digo abaixo:

```csharp
static void Main(string[] args)
{
    Campainha campainha = new Campainha();
    
    //pseudoc�digo
    //SE A CAMPAINHA TOCAR, EXECUTE
    //    Console.WriteLine("A campainha tocou.");

    campainha.Tocar();
    Console.ReadKey();
}
```

Como podemos ver, isso quebrar� a execu��o sequencial, pois o novo c�digo dever�
ser executado antes da linha seguinte � chamada do m�todo `Tocar()`.

Por isso, aplica��es modernas possuem fluxos que
funcionam de maneira diferente da computa��o tradicional. 
Grande aplica��es envolvem componentes que se comunicam atrav�s 
de mensagens, e isso produz um fluxo que foge da execu��o sequencial
(linha-a-linha).

Para criar solu��es que funcionem dessa maneira,
precisamos de um mecanismo pelo qual um componente possa
enviar mensagens para outro. Com a linguagem C#, podemos realizar isso
atrav�s dos **eventos**.

**Delegado de a��o**

O pseudoc�digo acima precisa � de algum tipo de "gancho" que seja chamado assim
que o m�todo `Tocar` da classe `Campainha` seja chamado.

Em outras palavras, o m�todo `Tocar` precisa "delegar uma a��o", isto �, entregar
o controle da execu��o para o pseudoc�digo que queremos criar na classe `Program`.

Mas a classe Campainha n�o sabe nada sobre a classe Program. Ent�o como podemos
fazer essa comunica��o? Aqui entra o **delegado de a��o**.

A classe Campainha vai **expor** uma propriedade que pode ou n�o ser usada pelos
seus clientes. Essa propriedade � um **delegado de a��o**, ou `Action`. Vamos
chamar essa action de `OnCampainhaTocou`:

```csharp
class Campainha
{
    public Action OnCampainhaTocou { get; set; }

    public void Tocar()
    {
        Console.WriteLine("A campainha tocou.");
    }
}
```

E como fazemos para essa `action` ser executada?

```csharp
public void Tocar()
{
    OnCampainhaTocou();
}
```

Note que `OnCampainhaTocou` � executado como um m�todo. Por qu�? Porque � realmente um m�todo.
Melhor dizendo, podemos pensar num delegado de a��o como um **ponteiro para um m�todo**.

Quando executamos o c�digo neste momento, tomaremos uma **exce��o de refer�ncia nula**. Mas por qu�?

Acontece que `OnCampainhaTocou` � ma propriedade que armazena um **tipo de refer�ncia**.
Como ela nunca foi inicializada, a chamada na linha `OnCampainhaTocou();` lan�a uma exce��o de refer�ncia nula.

Por isso, temos que tomar cuidado: antes de executar a action, � preciso ver se ela foi incializada:

```csharp
public void Tocar()
{
    if (OnCampainhaTocou != null)
    {
        OnCampainhaTocou();
    }
}
```

E agora, para consumir a action, vamos adicionar 2 m�todos � classe Program,
que n�o fazem nada al�m de escrever uma mensagem no console:

```csharp
class Program
{
    static void CampainhaTocou1()
    {
        Console.WriteLine("A campainha tocou.");
    }
```

Falta ainda "amarrar" esses m�todos como a action da classe `Campainha`.

Associamos um m�todo existente com a propriedade "action" da `OnCampainhaTocou`
com o operador **+=** :

```csharp
Campainha campainha = new Campainha();
campainha.OnCampainhaTocou += CampainhaTocou1;
```

Rodando a aplica��o, temos a mensagem:

![File](file.png)

Podemos pensar na action OnCampainhaTocou como um **"evento"**.

Como vimos, um delegado de a��o permite � classe Campainha executar um c�digo que
est� numa classe externa (Program), que a classe Campainha n�o precisa conhecer.

Outra caracter�stica de uma propriedade action � que ela pode ser associada a mais
de um m�todo. Vamos comprovar isso criando mais um m�todo na classe programa:

```csharp
static void CampainhaTocou2()
{
    Console.WriteLine("A campainha tocou.");
}
```

A agora basta associar a action tamb�m a esse novo m�todo

```csharp
Campainha campainha = new Campainha();
campainha.OnCampainhaTocou += CampainhaTocou1;
campainha.OnCampainhaTocou += CampainhaTocou2;
```

Rodando a aplica��o novamente, temos

![File2](file2.png)

Como vimos, ambos os m�todos associados foram executados ap�s o m�todo `Tocar()` invocar a action.

As bibliotecas .NET fornecem um n�mero pr�-definido de
tipos de delegados. O delegado de a��o mais simples
representa uma refer�ncia a um m�todo que n�o
retornar um resultado (o m�todo � do tipo void) e
n�o aceita nenhum par�metro (Como os m�todos CampainhaTocou1() e CampainhaTocou1()).

Abaixo, temos o c�digo completo do programa:

```csharp
using System;

namespace _01_02
{
    class Program
    {
        static void CampainhaTocou1()
        {
            Console.WriteLine("A campainha tocou.");
        }

        static void CampainhaTocou2()
        {
            Console.WriteLine("A campainha tocou.");
        }

        static void Main(string[] args)
        {
            Campainha campainha = new Campainha();
            campainha.OnCampainhaTocou += CampainhaTocou1;
            campainha.OnCampainhaTocou += CampainhaTocou2;

            campainha.Tocar();
            Console.ReadKey();
        }
    }

    class Campainha
    {
        public Action OnCampainhaTocou { get; set; }

        public void Tocar()
        {
            if (OnCampainhaTocou != null)
            {
                OnCampainhaTocou();
            }
        }
    }
}
```

**Inscritos no evento**

Os "assinantes" de um "publicador de evento" (como a action OnCampainhaTocou) 
se ligam a um editor usando o operador **+=**. O operador `+=` � uma sobrecarga para aplicar um comportamento
a um delegado.

Voc� pode simplificar a chamada do delegado usando
o operador condicional nulo. Isso s� executa uma
a��o se o item especificado n�o for nulo.

```csharp
//if (OnCampainhaTocou != null)
//{
//    OnCampainhaTocou();
//}

OnCampainhaTocou?.Invoke();
```

O operador condicional nulo `.?` Significa 
"acesse este membro da classe `OnCampainhaTocou` somente se a refer�ncia n�o for
nula."

Um delegado exp�e um m�todo Invoke para
invocar os m�todos ligados ao delegado. o
O comportamento do c�digo � o mesmo por�m o c�digo fica mais curto e 
mais claro.

**Cancelar inscri��o de um delegado**

Como aprendemos, o operador += foi sobrecarregado
para permitir que m�todos se liguem a eventos.

Mas e se quisermos desassociar o m�todo? Podemos "cancelar a inscri�a�"
de um delegado facilmente, utilizando o operador `-=`.

Isso desvincula um dos m�todos, mantendo o outro que j� estava associado.

```csharp
static void Main (string [] args)
{
    Campainha campainha = new Campainha();
    campainha.OnCampainhaTocou += CampainhaTocou1;
    campainha.OnCampainhaTocou += CampainhaTocou2;
    Console.WriteLine("Chamando campainha.Tocar()");
    campainha.Tocar();
    campainha.OnCampainhaTocou -= CampainhaTocou1;
    Console.WriteLine("Chamando campainha.Tocar()");
    campainha.Tocar();
    Console.ReadKey();
}
```

Este programa gera o seguinte:

```
Chamando campainha.Tocar()
A campainha tocou.
A campainha tocou.
Chamando campainha.Tocar()
A campainha tocou.
```

Isso significa que um m�todo pode ser associado e desvinculado, e associado
novament mais tarde, assim como desejarmos.

**Usando eventos**

Vamos dar uma olhada na propriedade *OnCampainhaTocou*:

```csharp
class Campainha
{
    public Action OnCampainhaTocou { get; set; }
    ...
}
```

Qual o problema com essa propriedade? Uma falha de **seguran�a**.

Como o delegado do OnCampainhaTocou � **p�blico**, para que 
os assinantes possam se conectar a ele. Por�m, isso significa 
que o c�digo externo para o objeto Campainha pode tocar a campainha 
**diretamente**, chamando o delegado OnCampainhaTocou, por exemplo:

```csharp
Campainha campainha = new Campainha();
campainha.Tocar();
```

Isso n�o � desej�vel.

Al�m disso, algum c�digo externo pode **sobrescrever** o valor de OnCampainhaTocou,
potencialmente removendo assinantes da action:

```csharp
campainha.OnCampainhaTocou = new Action(() => { });
```

E issso n�o � desej�vel.

C# fornece uma constru��o de evento que permite uma
delegado a ser especificado como um **evento**. 

```csharp
class Campainha
{
    public event Action OnCampainhaTocou = delegate { };
    public void Tocar()
    {
        OnCampainhaTocou();
    }
}
```

A palavra-chave **evento** tem que ser adicionada **antes** do
defini��o do delegado.

> public **event** Action OnCampainhaTocou = **delegate { };**

O membro `OnCampainhaTocou` agora � criado como um **campo** na
classe `Campainha`, em vez de uma propriedade.

Note que agora OnCampainhaTocou n�o tem mais os comportamentos get ou set.

Isso permiter **ocultar** esse evento **contra um acesso direto** de fora da classe Campainha.
Agora n�o � poss�vel para o c�digo
externo � classe Campainha atribuir valores a
OnCampainhaTocou. Esse delegate s� pode ser chamado de dentro da classe
onde � declarado. Em outras palavras, adicionando a palavra-chave do evento
transforma um **delegado** em um **evento**.

**Criar eventos com tipos de delega��o internos**

A pr�xima altera��o exige a troca do tipo Action pelo tipo EventHandler:

```csharp
public event EventHandler OnCampainhaTocou = delegate { };
```

Programas que trabalham com eventos devem usar o
EventHandler Class em vez de Action.

Isto ocorre porque a classe EventHandler � a parte do .NET
projetado para permitir que os assinantes recebam dados sobre
um evento. EventHandler � usado em todo o
.NET framework para gerenciar eventos, e serve para fornecer dados, ou sinalizar
que um evento ocorreu. 

Agora vamos chamar o evento OnCampainhaTocou dentro do m�todo Tocar(), mas de outra forma:

```csharp
public void Tocar()
{
    OnCampainhaTocou(this, EventArgs.Empty);
}
```

Como o evento OnCampainhaTocou j� foi inicializado, agora n�o h� 
necessidade de verificar se o delegado tem ou n�o um valor
antes de cham�-lo. Isso simplifica o m�todo `Tocar()`.

Neste ponto, nossa classe Campainha possui o seguinte c�digo:

```csharp
class Campainha
{
    public event EventHandler OnCampainhaTocou = delegate { };
    public void Tocar()
    {
        OnCampainhaTocou(this, EventArgs.Empty);
    }
}
```

O delegado EventHandler refere-se a um assinante
m�todo que aceita dois argumentos. O primeiro
argumento � uma refer�ncia ao objeto que gera o evento.
O segundo argumento � uma refer�ncia a um objeto de
tipo EventArgs que fornece informa��es sobre o
evento. Na Listagem 1-68, o segundo argumento � definido
para EventArgs. Vazio, para indicar que esse evento
n�o produz nenhum dado, � simplesmente uma notifica��o
que um evento ocorreu.
A assinatura dos m�todos a serem adicionados a este
delegado deve refletir isso. O m�todo CampainhaTocou1 aceita dois par�metros e pode ser usado com
este delegado.

```csharp
static void CampainhaTocou1(object sender, EventArgs e)
{
    Console.WriteLine("A campainha tocou.");
}

static void CampainhaTocou2(object sender, EventArgs e)
{
    Console.WriteLine("A campainha tocou.");
}
```

Use EventArgs para entregar informa��es sobre eventos

A classe Campainha criada na Listagem 1�68 permite um
assinante para receber uma notifica��o de que uma campainha
foi criado, mas n�o fornece ao assinante
qualquer descri��o da campainha. � �til se os assinantes
pode receber informa��es sobre a campainha. Talvez um
string descrevendo a localiza��o da campainha seria
�til.

Voc� pode fazer isso criando uma classe que possa entregar
esta informa��o e, em seguida, use um EventHandler para
entregue Isso. Listagem 1�69 mostra o CampainhaEventArgs
class, que � uma subclasse da classe Eve ntArgs,
e adiciona uma propriedade Location a ele. Se mais evento
informa��o � necess�ria, talvez a data e hora da
campainha, estes podem ser adicionados ao
Classe CampainhaEventArgs.

```csharp
class CampainhaEventArgs : EventArgs
{
    public string Apartamento { get; set; }
    public CampainhaEventArgs(string apartamento)
    {
        Apartamento = apartamento;
    }
}
```

Agora voc� tem seu pr�prio tipo que pode ser usado para
descreve um evento que ocorreu. O evento � a
campainha sendo tocada, e o tipo que voc� criou �
chamado CampainhaEventAgs. Quando a campainha � levantado n�s
deseja que o manipulador do evento de campainha aceite
Objetos CampainhaEventArgs para que o manipulador possa ser
detalhes do evento.

O delegado EventHandler para o
Evento OnCampainhaTocou � declarado para entregar
argumentos do tipo CampainhaEventArgs. 'Quando a
campainha � tocada pelo m�todo Tocou o evento
� dada uma refer�ncia � campainha e um rec�m-criado
inst�ncia de CampainhaEventArgs que descreve o
evento de campainha.

```csharp
class Campainha
{
    public event EventHandler<CampainhaEventArgs> OnCampainhaTocou = delegate { };
    public void Tocar(string apartamento)
    {
        OnCampainhaTocou(this, new CampainhaEventArgs(apartamento));
    }
}
```

Assinantes do evento aceitam o
CampainhaEventArgs e pode usar os dados nele. o
m�todo CampainhaTocou1 abaixo exibe o
localiza��o da campainha que obt�m de sua
argumento.

```csharp
static void CampainhaTocou1(object sender, CampainhaEventArgs e)
{
    Console.WriteLine($"A campainha tocou no apartamento {e.Apartamento}.");
}
```

Note que uma refer�ncia ao mesmo
O objeto CampainhaEventArgs � passado para cada um dos
assinantes do evento OnCampainhaTocou. este
significa que se um dos assinantes modifica o
conte�do da descri��o do evento, subseq�ente
Assinantes Ver� o evento modificado. Isso pode ser
�til se os assinantes precisam sinalizar que um dado evento
foi tratado dth, mas tamb�m pode ser uma fonte de
efeitos colaterais indesejados.

Exce��es nos assinantes do evento

Agora voc� sabe como os eventos funcionam. Um n�mero de
programas podem se inscrever em um evento. Eles fazem isso
vinculando um delegado ao evento. O delegado serve como
uma refer�ncia a um peda�o de c�digo C # que o assinante
quer correr Quando o evento ocorre. Este peda�o de
O c�digo � chamado de manipulador de eventos.

Nos nossos programas de exemplo, o evento � uma campainha
sendo acionada. Quando a campainha � tocada, o evento
chamar� todos os manipuladores de eventos que se inscreveram
o evento de campainha. Mas o que acontece se um dos eventos
manipuladores falhar, lan�ando uma exce��o? Se o c�digo em
um dos assinantes lan�a uma exce��o n�o identificada
o processo de tratamento de exce��o termina nesse ponto e
Nenhum outro assinante ser� notificado. Isso seria
significa que alguns assinantes n�o seriam informados
o evento.

Para resolver esse problema, cada manipulador de eventos pode ser
chamado individualmente e, em seguida, um �nico agregado
exce��o criada Que cont�m todos os detalhes de qualquer
exce��es que foram lan�adas pelos manipuladores de eventos.
A Listagem 1�70 mostra como isso � feito. o
O m�todo GetInvocationList � usado no
delegar para obter uma lista de assinantes do evento.
Esta lista � ent�o iterada e o Dynamiclnvo ke
m�todo chamado para cada assinante. Quaisquer exce��es
lan�ados por assinantes s�o capturados e adicionados a uma lista
de exce��es. Observe que a exce��o lan�ada pelo
assinante � entregue por um
TypelnvocationException, e � o interior
exce��o disto que deve ser salvo.

```csharp
public void Tocar(string apartamento)
{
    List<Exception> listaExcecoes = new List<Exception>();
    foreach (Delegate handler in OnCampainhaTocou.GetInvocationList())
    {
        try
        {
            handler.DynamicInvoke(this, new CampainhaEventArgs(apartamento));
        }
        catch (TargetInvocationException e)
        {
            listaExcecoes.Add(e.InnerException);
        }
    }

    if (listaExcecoes.Count > 0)
        throw new AggregateException(listaExcecoes);
}
```


```csharp
Campainha campainha = new Campainha();
campainha.OnCampainhaTocou += CampainhaTocou1;
campainha.OnCampainhaTocou += CampainhaTocou2;
try
{
    campainha.Tocar("202");
}
catch (AggregateException agg)
{
    foreach (Exception ex in agg.InnerExceptions)
        Console.WriteLine(ex.Message);
}
Console.ReadKey();
```

Quando este programa de amostra � executado, o resultado �
Segue. Observe que as exce��es s�o listadas ap�s a
os m�todos do assinante foram conclu�dos.

```
m�todo CampainhaTocou1() foi chamado
Apartamento: 202
m�todo CampainhaTocou2() foi chamado
Apartamento: 202
Erro em CampainhaTocou1
Erro em CampainhaTocou2
```

Criar Delegados

At� agora, usamos o Acti e
Tipos EventHandler, que fornecem pr�-definidos
delegados. Podemos, no entanto, criar nossos pr�prios delegados.
At� agora, os delegados que temos visto
Mantinha uma cole��o de refer�ncias de m�todos. Nosso
aplicativos usaram os operadores + = e - = para
adicione refer�ncias de m�todo a um determinado delegado. Voc� pode
tamb�m criar um delegado que se refere a um �nico m�todo em
um objeto.

Um tipo de delegado � declarado usando o delegado
palavra chave. A instru��o aqui cria um tipo de delegado
chamado IntOperation que pode se referir a um m�todo de
digite inteiro que aceita dois par�metros inteiros.

```csharp
delegate int Operacao(int a, int b);
```

Um programa pode agora criar vari�veis delegadas de
tipo IntOperation. Quando uma vari�vel delegada �
declarou que ele pode ser configurado para referenciar um determinado m�todo. Em
Listagem 1-71 abaixo da vari�vel op � feita para se referir
primeiro para um m�todo chamado Add e, em seguida, para um m�todo
chamado subtrair. Cada vez que op � chamado de "doente
execute o m�todo para o qual foi feito refer�ncia.
usando o sistema;

```csharp
using System;

namespace _01_03
{
    class Program
    {
        delegate int Operacao(int a, int b);

        static int Somar(int a, int b)
        {
            Console.WriteLine("Foi chamado: Somar");
            return a + b;
        }

        static int Subtrair(int a, int b)
        {
            Console.WriteLine("Foi chamado: Subtrair");
            return a - b;
        }

        static void Main(string[] args)
        {
            var op = new Operacao(Somar);
            Console.WriteLine(op(2, 2));

            op = Subtrair;
            Console.WriteLine(op(2, 2));
            Console.ReadKey();
        }
    }
}
```

Observe que o c�digo na Listagem 1�71 tamb�m mostra que um
programa pode criar explicitamente uma inst�ncia do
classe delegada. O compilador C # ser� automaticamente
gerar o c�digo para criar uma inst�ncia delegada Quando um
m�todo � atribu�do � vari�vel delegada.
Os delegados podem ser usados exatamente da mesma maneira que
qualquer outra vari�vel. Voc� pode ter listas e dicion�rios
que cont�m delegados e voc� tamb�m pode us�-los como
par�metros para m�todos.

Delegado vs delegado

� importante entender a diferen�a entre
delegado (com min�scula d) e Delegado (com
mai�scula D). A palavra delegado com um menor
case (1 � a palavra-chave usada em um programa c # que informa
o compilador para criar um tipo de delegado. � usado em
Listagem 1�71 para criar o tipo de delegado
IntOperation.

```csharp
delegate int Operacao(int a, int b);
```

A palavra Delegado com um D mai�sculo � o
classe abstrata que define o comportamento do delegado
inst�ncias. Depois que a palavra-chave delegada tiver sido
usado para criar um tipo de delegado, objetos desse delegado
type Ser� realizado como inst�ncias de delegado.

```csharp
Operacao op;
```

Esta declara��o cria um valor IntOperation
chamado op. A vari�vel op � uma inst�ncia do
System.MultiCastDelegatetyn ?? ChiSaCh? D
da classe Delegado. Um programa pode usar o
vari�vel op para manter uma cole��o de assinantes
ou para se referir a um �nico m�todo.

Use express�es lambda
(m�todos an�nimos)

Delegados permitem que um programa trate comportamentos
(m�todos em objetos) como itens de dados. Um delegado � um
item de dados que serve como refer�ncia a um m�todo em
um objeto. Isso adiciona uma quantidade enorme de
flexibilidade para programadores. No entanto, os delegados s�o
trabalho duro para usar. O tipo de delegado real deve primeiro
ser declarado e, em seguida, feito para se referir a um determinado
m�todo contendo o c�digo que descreve a a��o
a ser executado.

As express�es lambda s�o uma maneira pura de expressar
o �algo entra, algo acontece e
algo sai �parte de comportamentos. Os tipos de
os elementos e o resultado a ser devolvido s�o
inferido a partir do contexto em que o lambda
express�o � usada. Considere a seguinte declara��o.

```csharp
delegate int Operacao(int a, int b);
```

Esta declara��o declara o delegado IntOperation que foi usado na Listagem. o
Delegado de IntOperation pode se referir a qualquer opera��o
que leva em dois par�metros inteiros e retorna um
resultado inteiro. Agora considere esta afirma��o, que
cria um delegado IntOperation chamado adicionar e
atribui-lo a uma express�o lambda que aceita dois
par�metros de entrada e retorna sua soma.

```csharp
Operacao adicionar = (a, b) => a + b;
```

O operador `=>` � chamado de operador lambda. o
os itens aeb na esquerda da express�o lambda s�o
mapeado nos par�metros do m�todo definidos pelo
delegar. A declara��o 011 o direito do lambda
express�o d� o comportamento da express�o, e
neste caso, adiciona os dois par�metros juntos.
Ao descrever o comportamento do lambda
express�o voc� pode usar a frase "entra em" para
descreva o que est� acontecendo. Neste caso, voc� poderia dizer
�Aeb entrem em um plus b.� O nome lambda vem
de lambda calculus, um ramo da matem�tica que
diz respeito � "abstra��o funcional".
Esta express�o lambda aceita dois inteiros
par�metros e retorna um inteiro. Lambda
express�es podem aceitar v�rios par�metros e
cont�m v�rias instru��es, em que o caso
declara��es s�o colocadas em um bloco. Listagem 1-72 mostra
como criar uma express�o lambda que imprime um
mensagem, bem como realizar um c�lculo.

```csharp
Operacao adicionar = (a, b) =>
{
    Console.WriteLine("Foi chamado: adicionar");
    return a + b;
};
```

Fechamentos

O c�digo em uma express�o lambda pode acessar vari�veis
no c�digo em torno dele. Essas vari�veis devem ser
dispon�vel Quando a express�o lambda � executada,
o compilador estender� a vida �til das vari�veis usadas
express�es lambda.
Listagem 1�73 mostra como isso funciona. O m�todo
SetLo cal declara uma vari�vel local chamada 1 ocal Int
e define seu valor para 99. Em circunst�ncias normais
a vari�vel local Int seria destru�da ap�s
conclus�o do m�todo SetLocal. No entanto, o
A vari�vel localInt � usada em uma express�o lambda,
Qual � atribu�do ao delegado getLocal. o
compilador garante que a vari�vel lo cal Int seja
dispon�vel para uso na express�o lambda Quando �
posteriormente chamado do m�todo Main. este
extens�o da vida vari�vel � chamado de encerramento.

```csharp
class Program
{
    delegate int GetValor();
    static GetValor getValorLocal;
    static void SetValorLocal()
    {
        int valorLocal = 99;
        getValorLocal = () => valorLocal;
    }
    static void Main(string[] args)
    {
        SetValorLocal();
        Console.WriteLine("Valor de valorLocal: {0}", getValorLocal());
            Console.ReadKey();
    }
}
```

Tipos nativos para uso com express�es lambda 

Considere as tr�s declara��es a seguir:

```csharp
delegate int Operacao(int a, int b);
Operacao adicionar = (a, b) => a + b;
Console.WriteLine(adicionar(2, 2);
```

A primeira instru��o cria um delegado chamado
IntOperation que aceita dois valores inteiros e
retorna um resultado inteiro. A segunda declara��o
cria uma IntOperation chamada add que usa um
express�o lambda para descrever o que faz, que �
para adicionar os dois par�metros juntos e retornar o
resultado. A terceira declara��o realmente usa o add
opera��o para calcular e imprimir 2 + 2.
Isso funciona, mas tivemos que criar o
Tipo delegado IntOperation para especificar um comportamento
que aceita dois inteiros e retorna sua soma antes
poder�amos criar algo que se referisse a um lambda
express�o desse tipo. H� um certo n�mero de
em "tipos delegados que podemos usar para fornecer um contexto
para uma express�o lambda.

Os tipos Func fornecem uma gama de delegados para
m�todos que aceitam valores e retornam resultados. Listagem
1�74 mostra como o tipo Func � usado para criar um
adicionar comportamento que tenha o mesmo tipo de retorno e
par�metros como o delegado IntOperation na Listagem
1-71. Existem vers�es do tipo Func que aceitam
at� 16 itens de entrada. O m�todo add aqui aceita
dois inteiros e retorna um inteiro como resultado.

```csharp
static Func<int, int, int> adicionar = (a, b) => a + b;
```

Se a express�o lambda n�o retornar um resultado,
voc� pode usar o tipo de a��o que voc� viu anteriormente
quando criamos nossos primeiros delegados. A declara��o
abaixo cria um delegado chamado logM-es sage que
refere-se a uma express�o lambda que aceita uma string
e depois imprime no console. Para diferentes formas
de registrar o logMes 3 idade delegado pode ser anexado ao
outros m�todos que salvam os dados do log em um arquivo.

```csharp
static Action<string> logMensagem = (mensagem) => Console.WriteLine(mensagem);
```

O predicado incorporado ao tipo de delegado permite
criar c�digo que leva um valor de um determinado tipo e
retorna verdadeiro ou falso. The divideByThree
predicado abaixo retorna true se o valor for divis�vel
por 3.

```csharp
static Predicate<int> divisivelPor3 = (i) => i % 3 == 0;
```

M�todos an�nimos

At� agora temos usado express�es lambda
que est�o ligados aos delegados. O delegado fornece um
name pelo qual o c�digo na express�o lambda pode
ser acessado. No entanto, uma express�o lambda tamb�m pode
ser usado diretamente em um contexto onde voc� quer apenas
expressar um comportamento particular. O programa na listagem
1-75 usa a tarefa. Corra para iniciar uma nova tarefa. O c�digo
realizada pela tarefa � expressa diretamente como
express�o lambda, que � dada como um argumento para
a tarefa . Execute o m�todo. Em nenhum momento esse c�digo
j� tem um nome.

```csharp
class Program
{
    static void Main(string[] args)
    {
        Task.Run(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }
        });
        Console.WriteLine("A tarefa est� executando...");
        Console.ReadKey();
    }
}
```

Uma express�o lambda usada desta maneira pode ser
descrito como um m�todo an�nimo; porque � um
c�digo funcional que n�o tem nome.