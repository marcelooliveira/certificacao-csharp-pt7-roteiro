# Criar e implementar enventos e callbacks

Vamos come�ar o curso com um uma simples classe chamada `Campainha`, cuja
�nica fun��o � tocar. 

![Img1](img1.png)

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

1. Vamos criar um programa que instanciando essa classe
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

O delegado **EventHandler** OnCampainhaTocou aceita dois argumentos. 

- O primeiro argumento � uma refer�ncia ao objeto que gera o evento.
- O segundo argumento � uma refer�ncia a um objeto de tipo EventArgs que fornece informa��es sobre oevento.

Note que o segundo argumento � definido
para um EventArgs vazio (empty), para indicar que esse evento
n�o produz nenhum dado, � simplesmente uma notifica��o
de que um evento ocorreu.

```csharp
OnCampainhaTocou(this, EventArgs.Empty);
```

Agora, as assinaturas dos m�todos a serem adicionados a este
delegado devem refletir isso.

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

Somente ap�s essas altera��es, os m�todo CampainhaTocou1 e CampainhaTocou2 
aceita dois par�metros e pode ser usado com este delegado.

**Use EventArgs para entregar informa��es sobre eventos**

A classe Campainha permite que um
assinante receba uma notifica��o de que um evento de campainha
foi gerado, mas n�o fornece ao assinante
qualquer descri��o do evento.

Agora imagine uma campainha do mundo real, onde voc� tem um sistema
de porteiro eletr�nico de um pr�dio, que permite acionar a campainha de um 
apartamento espec�fico.

![Img2](img2.png)

Seria �til se os assinantes pode receber informa��es sobre a 
campainha, como por exemplo, o n�mero do apartamento.

Voc� pode fazer isso criando uma classe que possa entregar
esta informa��o (do n�mero do apartamento) e, em seguida, 
usar um EventHandler para fornecer a informa��o.

Vamos criar ent�o uma subclasse customizada a partir da classe `EventArgs`:

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

Note que agora temos:

- Uma propriedade Apartamento
- um construtor que alimenta essa propriedade

Voc� pode adicionar mais informa��es, conforme necess�rio.

Agora voc� tem seu pr�prio tipo que pode ser usado para
descreve um evento que ocorreu. 

Como usar a nova classe CampainhaEventArgs em nossa aplica��o?

Vamos come�ar trocando o tipo EventHandler (que por padr�o trabalha com EventArgs)
por um EventHandler gen�rico: `EventHandler<CampainhaEventArgs>`

```csharp
public event EventHandler<CampainhaEventArgs> OnCampainhaTocou = delegate { };
```

Quando a campainha � tocada pelo m�todo Tocou, o evento
� uma refer�ncia � campainha e um rec�m-criado
inst�ncia de CampainhaEventArgs que descreve o
evento de campainha.

Vamos modificar tamb�m o m�todo Tocar(), para receber o n�mero do apartamento,
e passar esse n�mero para o objeto CampainhaEventArgs, que ser� o respons�vel por transportar essa informa��o
at� os clientes assinantes.

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

Outra modifica��o necess�ria � adequar os assinantes do evento para que
eles possam ser associados ao evento:

```csharp
static void CampainhaTocou1(object sender, CampainhaEventArgs e)
{
    Console.WriteLine($"A campainha tocou no apartamento {e.Apartamento}.");
}
```

Dessa forma, podemos exibir a informa��o completa: quando uma campainha foi acionada,
e tamb�m para qual apartamento.

**Exce��es nos assinantes do evento**

Agora voc� sabe como os eventos funcionam. V�rios
programas podem se inscrever em um mesmo evento. Eles fazem isso
vinculando um delegado ao evento. O delegado serve como
uma refer�ncia a **um trecho de c�digo C#** que o assinante
quer executar quando o evento acontece. Este trecho de
c�digo � chamado de **manipulador de eventos** (ex.: o m�todo CampainhaTocou1).

Nos nossos programas de exemplo, o evento � uma campainha
sendo acionada. Quando a campainha � tocada, o evento
chamar� **todos os manipuladores de eventos** que se inscreveram
o evento de campainha (os m�todos CampainhaTocou1 e CampainhaTocou2). 

Mas o que acontece se um dos eventos
manipuladores falhar, lan�ando uma exce��o?

Se o c�digo em um dos assinantes lan�ar uma **exce��o n�o identificada**,
o processo de tratamento de exce��o **termina** nesse ponto e
nenhum outro assinante ser� notificado.
Isso n�o � nada bom, pois significa que alguns assinantes n�o seriam notificados.

Para resolver esse problema, cada manipulador de eventos pode ser
**chamado individualmente** e, em seguida, um �nico agregado
exce��o criada Que cont�m todos os detalhes de qualquer
exce��es que foram lan�adas pelos manipuladores de eventos.

Como podemos implementar esse novo algoritmo?

Em primeiro lugar, vamos refatorar o m�todo Tocar() para criar uma lista de exce��es
que precisam ser tratadas:

```csharp
public void Tocar(string apartamento)
{
    List<Exception> listaExcecoes = new List<Exception>();
}
```

O segundo passo � obter a lista de assinantes.
Usamos o m�todo GetInvocationList para obter uma lista de assinantes do evento.
Vamos criar um la�o e iterar sobre a lista de manipuladores dos assinantes do evento OnCampainhaTocou.

```csharp
public void Tocar(string apartamento)
{
    List<Exception> listaExcecoes = new List<Exception>();
    foreach (Delegate handler in OnCampainhaTocou.GetInvocationList())
    {
    }
}
```

Para executar cada um dos delegados, � necess�rio invocar o m�todo DynamicInvoke da classe Delegate.

Fazemos isso passando como argumentos:

- o objeto Campainha onde ocorreu o evento
- um novo objeto CampainhaEventArgs com informa��es sobre o apartamento

```csharp
public void Tocar(string apartamento)
{
    List<Exception> listaExcecoes = new List<Exception>();
    foreach (Delegate handler in OnCampainhaTocou.GetInvocationList())
    {
        handler.DynamicInvoke(this, new CampainhaEventArgs(apartamento));
    }
}
```

Mas at� agora, n�o temos nenhum c�digo para tratar as exce��es. Vamos introduzir um bloco try-catch
para capturar erros e adicion�-los � lista exceptionList quando necess�rio.

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
}
```

Ao final do m�todo, temos que verificar se alguma exce��o foi adicionada � lista.
Se houver erros na lista, ela � usada para lan�ar uma nova exce��o, do tipo "exce��o agregada".

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

Note que o c�digo acima permite que todos os assinantes recebam notifica��es, mesmo se alguns
deles lan�ar uma exce��o.

E quanto ao c�digo cliente?

Agora falta preparar a classe Program para simular e lidar com exce��es.

Vamos provocar exce��es deliberadamente, em cada um dos manipuladores de evento:

```csharp
static void CampainhaTocou1(object source, CampainhaEventArgs e)
{
    Console.WriteLine("m�todo CampainhaTocou1() foi chamado");
    Console.WriteLine("Apartamento: {0}", e.Apartamento);
    throw new Exception("Erro em CampainhaTocou1");
}

static void CampainhaTocou2(object source, CampainhaEventArgs e)
{
    Console.WriteLine("m�todo CampainhaTocou2() foi chamado");
    Console.WriteLine("Apartamento: {0}", e.Apartamento);
    throw new Exception("Erro em CampainhaTocou2");
}
```

Nenhum desses erros dever� impedir a notifica��o de todos os assinantes do evento.

Caso uma exce��o agregada ocorra, podemos captur�-la, e varrer a lista
de exce��es internas, exibindo suas informa��es:

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

Quando este programa de exemplo � executado, o resultado abaixo � mostrado. 
Observe que as exce��es s�o listadas ap�s a
os m�todos do assinante foram conclu�dos.

```
m�todo CampainhaTocou1() foi chamado
Apartamento: 202
m�todo CampainhaTocou2() foi chamado
Apartamento: 202
Erro em CampainhaTocou1
Erro em CampainhaTocou2
```

**Criar Delegados**

At� agora, usamos actions e tipos EventHandler, que fornecem 
**delegados pr�-definidos**.

Mas se quisermos, podemos criar nossos pr�prios delegados.

At� agora, os delegados que conhecemos mant�m uma cole��o 
de refer�ncias de m�todos. Nosso c�digo usou os operadores += e -= para
adicionar refer�ncias de m�todo a um determinado delegado.

Voc� pode tamb�m criar um delegado que se refere a um �nico m�todo em
um objeto.

Para demonstrar, camos criar dois m�todos que realizam opera��es de somar e subtrair.

```csharp
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
```

Essas duas opera��es podem ser facilmente chamadas no c�digo cliente:

```csharp
Console.WriteLine(Somar(2, 2));
Console.WriteLine(Subtrair(2, 2));
```

Mas e se quisermos armazenar uma **refer�ncia aos dois m�todos**, e executar a **partir 
dessa refer�ncia**, em vez de diretamente pelos m�todos?

```csharp
[Tipo???] op = Somar
Console.WriteLine(op(2, 2));

op = Subtrair;
Console.WriteLine(op(2, 2));
```

Mas que tipo devemos utilizar para declarar a vari�vel **op**?

Precisamos de um **delegado**.

Um tipo de delegado � declarado usando a
palavra chave **delegate**. A instru��o aqui cria um tipo de delegado
chamado **Operacao** que pode se referir a um m�todo do tipo inteiro, que aceita 
dois par�metros inteiros.

```csharp
delegate int Operacao(int a, int b);
```

Um programa pode agora criar *vari�veis delegate* de
tipo Operacao. Quando uma vari�vel delegada �
declarada, ela pode ser configurada para referenciar um determinado m�todo. 

No c�digo abaixo, abaixo da vari�vel op � feita para se referir
primeiro para um m�todo chamado Somar e

```csharp
var op = new Operacao(Somar);
Console.WriteLine(op(2, 2));
```

Outra forma de declara��o que tem o mesmo efeito �:

```csharp
Operacao op = Somar;
Console.WriteLine(op(2, 2));
```

em seguida, para um m�todo
chamado Subtrair. 

```csharp
op = Subtrair;
Console.WriteLine(op(2, 2));
```

Cada vez que op � chamado, ele
executar� o m�todo ao qual faz refer�ncia.

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

Observe que o c�digo acima tamb�m mostra que um
programa pode criar explicitamente uma inst�ncia do
classe delegada. O compilador C# ser� automaticamente
gerar o c�digo para criar uma inst�ncia delegada Quando um
m�todo � atribu�do � vari�vel delegada.

Os delegados podem ser usados exatamente da mesma maneira que
qualquer outra vari�vel. Voc� pode ter listas e dicion�rios
que cont�m delegados e voc� tamb�m pode us�-los como
par�metros para m�todos.

**Delegado vs delegado**

� importante entender a diferen�a entre delegado (com d min�scula) e Delegado 
(com D mai�scula). A palavra delegate (min�scula) informa o compilador C# para 
criar um tipo de delegado. 

```csharp
delegate int Operacao(int a, int b);
```

A palavra Delegate com um D mai�sculo � o
classe abstrata que define o comportamento de inst�ncias do delegate. 

Depois de usarmos a palavra-chave delegada para criar um tipo de delegado, 
objetos desse tipo delegate ser�o criados como inst�ncias de delegado.

```csharp
Operacao op;
```

**Usando express�es lambda
(m�todos an�nimos)**

Delegados permitem que um programa trate comportamentos
(m�todos em objetos) como *itens de dados*. Um delegado � um
item de dados que serve como refer�ncia a um m�todo em
um objeto. Isso d� uma grande flexibilidade aos programadores.

No entanto, usar delegados d� trabalho. O tipo delegate deve ser declarado primeiro
e, em seguida, armazenar uma refer�ncia a um determinado m�todo contendo o c�digo 
a ser executado.

As **express�es lambda** s�o uma maneira simples de expressar comportamentos
seguindo a l�gica "algo entra, algo acontece e algo sai".

Considere a seguinte declara��o.

```csharp
delegate int Operacao(int a, int b);
```

Esta declara��o declara o delegado Operacao que foi usado na listagem. o
Delegado de Operacao pode se referir a qualquer opera��o
que leva em dois par�metros inteiros e retorna um
resultado inteiro. Agora considere esta afirma��o, que
cria um delegado Operacao chamado adicionar e
atribui-lo a uma express�o lambda que aceita dois
par�metros de entrada e retorna sua soma.

```csharp
Operacao adicionar = (a, b) => a + b;
```

O operador `=>` � chamado de **operador lambda**. Os itens **a e b** na esquerda da 
express�o lambda s�o **par�metros** do m�todo definidos pelo
delegate. A declara��o � direita da express�o lambda
define o **comportamento** da express�o e retorna a soma os dois par�metros.

Esta express�o lambda aceita dois inteiros
par�metros e retorna um inteiro. Lambda
express�es podem aceitar v�rios par�metros e
cont�m v�rias instru��es, em que o caso
declara��es s�o colocadas em um bloco.

O c�digo abaixo mostra como criar uma express�o lambda que imprime uma
mensagem, e realiza um c�lculo.

```csharp
Operacao adicionar = (a, b) =>
{
    Console.WriteLine("Foi chamado: adicionar");
    return a + b;
};
```

**Fechamentos (Closures)**

O c�digo em uma express�o lambda pode acessar vari�veis
no c�digo em torno dele. Essas vari�veis devem ser
dispon�vel quando a express�o lambda � executada,
o compilador estender� a vida �til das vari�veis usadas
express�es lambda.

O c�digo abaixo mostra como isso funciona. O m�todo
SetValorLocal declara uma vari�vel local chamada valorLocal
e define seu valor para 99. Em circunst�ncias normais
a vari�vel valorLocal seria destru�da ap�s
conclus�o do m�todo SetValorLocal.

```csharp
static void SetValorLocal()
{
    int valorLocal = 99;
    getValorLocal = () => valorLocal;
}
```

No entanto, a vari�vel valorLocal � usada em uma express�o lambda,
qu� � atribu�do ao delegado **getValorLocal**. O
compilador garante que a vari�vel valorLocal seja
dispon�vel para uso na express�o lambda quando �
posteriormente chamado do m�todo Main. este
extens�o da vida vari�vel � chamado de **encerramento**.

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

**Tipos nativos para uso com express�es lambda**

Considere as tr�s declara��es a seguir:

```csharp
delegate int Operacao(int a, int b);
Operacao adicionar = (a, b) => a + b;
Console.WriteLine(adicionar(2, 2);
```

A primeira instru��o cria um delegado chamado
Operacao que aceita dois valores inteiros e
retorna um resultado inteiro:

```csharp
delegate int Operacao(int a, int b);
```

A segunda declara��o cria uma Operacao chamada adicionar que usa um
express�o lambda para descrever o que faz, para somar os dois par�metros 
e retornar o resultado.

```csharp
Operacao adicionar = (a, b) => a + b;
```

A terceira declara��o realmente usa a vari�vel adicionar opera��o para calcular 
e imprimir 2 + 2.

```csharp
Console.WriteLine(adicionar(2, 2);
```

Isso funciona, mas tivemos que criar o tipo delegate Operacao para especificar um 
comportamento que aceita dois inteiros e retorna sua soma antes
poder�amos criar algo que se referisse a uma express�o lambda
desse tipo.

Os tipos **Func** fornecem v�rios delegados para
m�todos que aceitam valores e retornam resultados:

```csharp
static Func<int, int, int> adicionar = (a, b) => a + b;
```

```csharp
static void Main()
{
    Console.WriteLine($"adicionar(2, 3): {adicionar(2, 3)}");
    Console.ReadKey();
}
```

Existem vers�es do tipo Func que aceitam
at� 16 itens de entrada. O m�todo *adicionar* aqui aceita
dois inteiros e retorna um inteiro como resultado.

Se a express�o lambda *n�o retornar um resultado*,
voc� pode usar o tipo de `action` que voc� viu anteriormente
quando criamos nossos primeiros delegados.


A declara��o abaixo cria um delegado chamado logMensagem, que
referencia a uma express�o lambda que aceita uma string
e depois a imprime no console.

```csharp
static Action<string> logMensagem = (mensagem) => Console.WriteLine(mensagem);
```

```csharp
static void Main()
{
    Console.WriteLine($"adicionar(2, 3): {adicionar(2, 3)}");
    Console.WriteLine($"logMensagem('esta � uma Action')");
    logMensagem("esta � uma Action");
    Console.ReadKey();
}
```

Para diferentes formas de registrar o delegate logMensagem pode ser anexado ao
outros m�todos que salvam os dados do log em um arquivo.

O predicado incorporado ao tipo delegate permite
criar c�digo que leva um valor de um determinado tipo e
retorna verdadeiro ou falso. O predicado divisivelPor3
abaixo retorna true se o valor for divis�vel por 3.

```csharp
static Predicate<int> divisivelPor3 = (i) => i % 3 == 0;
```

```csharp
static void Main()
{
    var numeros = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    var divisiveis = Array.FindAll(numeros, divisivelPor3);
    Console.WriteLine("Divis�veis por 3: " + string.Join(',', divisiveis));
    Console.ReadKey();
}
```

**M�todos an�nimos**

At� aqui usamos express�es lambda
que est�o ligados aos delegados. O delegado fornece um
nome pelo qual o c�digo na express�o lambda pode
ser acessado. No entanto, uma express�o lambda tamb�m pode
ser usado diretamente em um contexto onde voc� quer apenas
expressar um comportamento particular. 

O programa abaixo usa a tarefa. Ao rodar esse programa, uma nova tarefa � iniciada.
O c�digo executado pela tarefa � expresso diretamente como
**express�o lambda**, que � fornecido como um argumento para
a tarefa.

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
descrito como um **m�todo an�nimo**, porque � um
**c�digo funcional** que n�o tem nome.