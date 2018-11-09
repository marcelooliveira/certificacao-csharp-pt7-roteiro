# Criar e implementar enventos e callbacks

Vamos começar o curso com um uma simples classe chamada `Campainha`, cuja
única função é tocar. 

```csharp
class Campainha
{
    public void Tocar()
    {
        Console.WriteLine("A campainha tocou.");
    }
}
```

Então criamos um programa simples para utilizar essa classe:

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

Note como a execução do programa segue uma ordem sequencial:

1. E vamos criar um programa que instanciando essa classe
2. Em seguida, tocamos a campainha, chamando o método da classe
3. Depois, esperamos o usuário teclar algo

Até aqui, você viu que a execução de um programa avança de instrução para
declaração, processando os dados de acordo com o
declarações que são executadas. Na computação tradicional,
o programa flui do início ao fim, começando
com os dados de entrada e produzindo alguma saída antes
de terminar. 

Agora imagine que você precise executar um código dentro da classe `Program`,
que será chamado sempre que o método `Campainha`, no lugar do pseudocódigo abaixo:

```csharp
static void Main(string[] args)
{
    Campainha campainha = new Campainha();
    
    //pseudocódigo
    //SE A CAMPAINHA TOCAR, EXECUTE
    //    Console.WriteLine("A campainha tocou.");

    campainha.Tocar();
    Console.ReadKey();
}
```

Como podemos ver, isso quebrará a execução sequencial, pois o novo código deverá
ser executado antes da linha seguinte à chamada do método `Tocar()`.

Por isso, aplicações modernas possuem fluxos que
funcionam de maneira diferente da computação tradicional. 
Grande aplicações envolvem componentes que se comunicam através 
de mensagens, e isso produz um fluxo que foge da execução sequencial
(linha-a-linha).

Para criar soluções que funcionem dessa maneira,
precisamos de um mecanismo pelo qual um componente possa
enviar mensagens para outro. Com a linguagem C#, podemos realizar isso
através dos **eventos**.

**Delegado de ação**

O pseudocódigo acima precisa é de algum tipo de "gancho" que seja chamado assim
que o método `Tocar` da classe `Campainha` seja chamado.

Em outras palavras, o método `Tocar` precisa "delegar uma ação", isto é, entregar
o controle da execução para o pseudocódigo que queremos criar na classe `Program`.

Mas a classe Campainha não sabe nada sobre a classe Program. Então como podemos
fazer essa comunicação? Aqui entra o **delegado de ação**.

A classe Campainha vai **expor** uma propriedade que pode ou não ser usada pelos
seus clientes. Essa propriedade é um **delegado de ação**, ou `Action`. Vamos
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

Note que `OnCampainhaTocou` é executado como um método. Por quê? Porque é realmente um método.
Melhor dizendo, podemos pensar num delegado de ação como um **ponteiro para um método**.

Quando executamos o código neste momento, tomaremos uma **exceção de referência nula**. Mas por quê?

Acontece que `OnCampainhaTocou` é ma propriedade que armazena um **tipo de referência**.
Como ela nunca foi inicializada, a chamada na linha `OnCampainhaTocou();` lança uma exceção de referência nula.

Por isso, temos que tomar cuidado: antes de executar a action, é preciso ver se ela foi incializada:

```csharp
public void Tocar()
{
    if (OnCampainhaTocou != null)
    {
        OnCampainhaTocou();
    }
}
```

E agora, para consumir a action, vamos adicionar 2 métodos à classe Program,
que não fazem nada além de escrever uma mensagem no console:

```csharp
class Program
{
    static void CampainhaTocou1()
    {
        Console.WriteLine("A campainha tocou.");
    }
```

Falta ainda "amarrar" esses métodos como a action da classe `Campainha`.

Associamos um método existente com a propriedade "action" da `OnCampainhaTocou`
com o operador **+=** :

```csharp
Campainha campainha = new Campainha();
campainha.OnCampainhaTocou += CampainhaTocou1;
```

Rodando a aplicação, temos a mensagem:

![File](file.png)

Podemos pensar na action OnCampainhaTocou como um **"evento"**.

Como vimos, um delegado de ação permite à classe Campainha executar um código que
está numa classe externa (Program), que a classe Campainha não precisa conhecer.

Outra característica de uma propriedade action é que ela pode ser associada a mais
de um método. Vamos comprovar isso criando mais um método na classe programa:

```csharp
static void CampainhaTocou2()
{
    Console.WriteLine("A campainha tocou.");
}
```

A agora basta associar a action também a esse novo método

```csharp
Campainha campainha = new Campainha();
campainha.OnCampainhaTocou += CampainhaTocou1;
campainha.OnCampainhaTocou += CampainhaTocou2;
```

Rodando a aplicação novamente, temos

![File2](file2.png)

Como vimos, ambos os métodos associados foram executados após o método `Tocar()` invocar a action.

As bibliotecas .NET fornecem um número pré-definido de
tipos de delegados. O delegado de ação mais simples
representa uma referência a um método que não
retornar um resultado (o método é do tipo void) e
não aceita nenhum parâmetro (Como os métodos CampainhaTocou1() e CampainhaTocou1()).

Abaixo, temos o código completo do programa:

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
se ligam a um editor usando o operador **+=**. O operador `+=` é uma sobrecarga para aplicar um comportamento
a um delegado.

Você pode simplificar a chamada do delegado usando
o operador condicional nulo. Isso só executa uma
ação se o item especificado não for nulo.

```csharp
//if (OnCampainhaTocou != null)
//{
//    OnCampainhaTocou();
//}

OnCampainhaTocou?.Invoke();
```

O operador condicional nulo `.?` Significa 
"acesse este membro da classe `OnCampainhaTocou` somente se a referência não for
nula."

Um delegado expõe um método Invoke para
invocar os métodos ligados ao delegado. o
O comportamento do código é o mesmo porém o código fica mais curto e 
mais claro.

**Cancelar inscrição de um delegado**

Como aprendemos, o operador += foi sobrecarregado
para permitir que métodos se liguem a eventos.

Mas e se quisermos desassociar o método? Podemos "cancelar a inscriçaõ"
de um delegado facilmente, utilizando o operador `-=`.

Isso desvincula um dos métodos, mantendo o outro que já estava associado.

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

Isso significa que um método pode ser associado e desvinculado, e associado
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

Qual o problema com essa propriedade? Uma falha de **segurança**.

Como o delegado do OnCampainhaTocou é **público**, para que 
os assinantes possam se conectar a ele. Porém, isso significa 
que o código externo para o objeto Campainha pode tocar a campainha 
**diretamente**, chamando o delegado OnCampainhaTocou, por exemplo:

```csharp
Campainha campainha = new Campainha();
campainha.Tocar();
```

Isso não é desejável.

Além disso, algum código externo pode **sobrescrever** o valor de OnCampainhaTocou,
potencialmente removendo assinantes da action:

```csharp
campainha.OnCampainhaTocou = new Action(() => { });
```

E issso não é desejável.

C# fornece uma construção de evento que permite uma
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
definição do delegado.

> public **event** Action OnCampainhaTocou = **delegate { };**

O membro `OnCampainhaTocou` agora é criado como um **campo** na
classe `Campainha`, em vez de uma propriedade.

Note que agora OnCampainhaTocou não tem mais os comportamentos get ou set.

Isso permiter **ocultar** esse evento **contra um acesso direto** de fora da classe Campainha.
Agora não é possível para o código
externo à classe Campainha atribuir valores a
OnCampainhaTocou. Esse delegate só pode ser chamado de dentro da classe
onde é declarado. Em outras palavras, adicionando a palavra-chave do evento
transforma um **delegado** em um **evento**.

**Criar eventos com tipos de delegação internos**

A próxima alteração exige a troca do tipo Action pelo tipo EventHandler:

```csharp
public event EventHandler OnCampainhaTocou = delegate { };
```

Programas que trabalham com eventos devem usar o
EventHandler Class em vez de Action.

Isto ocorre porque a classe EventHandler é a parte do .NET
projetado para permitir que os assinantes recebam dados sobre
um evento. EventHandler é usado em todo o
.NET framework para gerenciar eventos, e serve para fornecer dados, ou sinalizar
que um evento ocorreu. 

Agora vamos chamar o evento OnCampainhaTocou dentro do método Tocar(), mas de outra forma:

```csharp
public void Tocar()
{
    OnCampainhaTocou(this, EventArgs.Empty);
}
```

Como o evento OnCampainhaTocou já foi inicializado, agora não há 
necessidade de verificar se o delegado tem ou não um valor
antes de chamá-lo. Isso simplifica o método `Tocar()`.

Neste ponto, nossa classe Campainha possui o seguinte código:

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
método que aceita dois argumentos. O primeiro
argumento é uma referência ao objeto que gera o evento.
O segundo argumento é uma referência a um objeto de
tipo EventArgs que fornece informações sobre o
evento. Na Listagem 1-68, o segundo argumento é definido
para EventArgs. Vazio, para indicar que esse evento
não produz nenhum dado, é simplesmente uma notificação
que um evento ocorreu.
A assinatura dos métodos a serem adicionados a este
delegado deve refletir isso. O método CampainhaTocou1 aceita dois parâmetros e pode ser usado com
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

Use EventArgs para entregar informações sobre eventos

A classe Campainha criada na Listagem 1—68 permite um
assinante para receber uma notificação de que uma campainha
foi criado, mas não fornece ao assinante
qualquer descrição da campainha. É útil se os assinantes
pode receber informações sobre a campainha. Talvez um
string descrevendo a localização da campainha seria
útil.

Você pode fazer isso criando uma classe que possa entregar
esta informação e, em seguida, use um EventHandler para
entregue Isso. Listagem 1—69 mostra o CampainhaEventArgs
class, que é uma subclasse da classe Eve ntArgs,
e adiciona uma propriedade Location a ele. Se mais evento
informação é necessária, talvez a data e hora da
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

Agora você tem seu próprio tipo que pode ser usado para
descreve um evento que ocorreu. O evento é a
campainha sendo tocada, e o tipo que você criou é
chamado CampainhaEventAgs. Quando a campainha é levantado nós
deseja que o manipulador do evento de campainha aceite
Objetos CampainhaEventArgs para que o manipulador possa ser
detalhes do evento.

O delegado EventHandler para o
Evento OnCampainhaTocou é declarado para entregar
argumentos do tipo CampainhaEventArgs. 'Quando a
campainha é tocada pelo método Tocou o evento
é dada uma referência à campainha e um recém-criado
instância de CampainhaEventArgs que descreve o
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
método CampainhaTocou1 abaixo exibe o
localização da campainha que obtém de sua
argumento.

```csharp
static void CampainhaTocou1(object sender, CampainhaEventArgs e)
{
    Console.WriteLine($"A campainha tocou no apartamento {e.Apartamento}.");
}
```

Note que uma referência ao mesmo
O objeto CampainhaEventArgs é passado para cada um dos
assinantes do evento OnCampainhaTocou. este
significa que se um dos assinantes modifica o
conteúdo da descrição do evento, subseqüente
Assinantes Verá o evento modificado. Isso pode ser
útil se os assinantes precisam sinalizar que um dado evento
foi tratado dth, mas também pode ser uma fonte de
efeitos colaterais indesejados.

Exceções nos assinantes do evento

Agora você sabe como os eventos funcionam. Um número de
programas podem se inscrever em um evento. Eles fazem isso
vinculando um delegado ao evento. O delegado serve como
uma referência a um pedaço de código C # que o assinante
quer correr Quando o evento ocorre. Este pedaço de
O código é chamado de manipulador de eventos.

Nos nossos programas de exemplo, o evento é uma campainha
sendo acionada. Quando a campainha é tocada, o evento
chamará todos os manipuladores de eventos que se inscreveram
o evento de campainha. Mas o que acontece se um dos eventos
manipuladores falhar, lançando uma exceção? Se o código em
um dos assinantes lança uma exceção não identificada
o processo de tratamento de exceção termina nesse ponto e
Nenhum outro assinante será notificado. Isso seria
significa que alguns assinantes não seriam informados
o evento.

Para resolver esse problema, cada manipulador de eventos pode ser
chamado individualmente e, em seguida, um único agregado
exceção criada Que contém todos os detalhes de qualquer
exceções que foram lançadas pelos manipuladores de eventos.
A Listagem 1—70 mostra como isso é feito. o
O método GetInvocationList é usado no
delegar para obter uma lista de assinantes do evento.
Esta lista é então iterada e o Dynamiclnvo ke
método chamado para cada assinante. Quaisquer exceções
lançados por assinantes são capturados e adicionados a uma lista
de exceções. Observe que a exceção lançada pelo
assinante é entregue por um
TypelnvocationException, e é o interior
exceção disto que deve ser salvo.

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

Quando este programa de amostra é executado, o resultado é
Segue. Observe que as exceções são listadas após a
os métodos do assinante foram concluídos.

```
método CampainhaTocou1() foi chamado
Apartamento: 202
método CampainhaTocou2() foi chamado
Apartamento: 202
Erro em CampainhaTocou1
Erro em CampainhaTocou2
```

Criar Delegados

Até agora, usamos o Acti e
Tipos EventHandler, que fornecem pré-definidos
delegados. Podemos, no entanto, criar nossos próprios delegados.
Até agora, os delegados que temos visto
Mantinha uma coleção de referências de métodos. Nosso
aplicativos usaram os operadores + = e - = para
adicione referências de método a um determinado delegado. Você pode
também criar um delegado que se refere a um único método em
um objeto.

Um tipo de delegado é declarado usando o delegado
palavra chave. A instrução aqui cria um tipo de delegado
chamado IntOperation que pode se referir a um método de
digite inteiro que aceita dois parâmetros inteiros.

```csharp
delegate int Operacao(int a, int b);
```

Um programa pode agora criar variáveis delegadas de
tipo IntOperation. Quando uma variável delegada é
declarou que ele pode ser configurado para referenciar um determinado método. Em
Listagem 1-71 abaixo da variável op é feita para se referir
primeiro para um método chamado Add e, em seguida, para um método
chamado subtrair. Cada vez que op é chamado de "doente
execute o método para o qual foi feito referência.
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

Observe que o código na Listagem 1—71 também mostra que um
programa pode criar explicitamente uma instância do
classe delegada. O compilador C # será automaticamente
gerar o código para criar uma instância delegada Quando um
método é atribuído à variável delegada.
Os delegados podem ser usados exatamente da mesma maneira que
qualquer outra variável. Você pode ter listas e dicionários
que contêm delegados e você também pode usá-los como
parâmetros para métodos.

Delegado vs delegado

É importante entender a diferença entre
delegado (com minúscula d) e Delegado (com
maiúscula D). A palavra delegado com um menor
case (1 é a palavra-chave usada em um programa c # que informa
o compilador para criar um tipo de delegado. É usado em
Listagem 1—71 para criar o tipo de delegado
IntOperation.

```csharp
delegate int Operacao(int a, int b);
```

A palavra Delegado com um D maiúsculo é o
classe abstrata que define o comportamento do delegado
instâncias. Depois que a palavra-chave delegada tiver sido
usado para criar um tipo de delegado, objetos desse delegado
type Será realizado como instâncias de delegado.

```csharp
Operacao op;
```

Esta declaração cria um valor IntOperation
chamado op. A variável op é uma instância do
System.MultiCastDelegatetyn ?? ChiSaCh? D
da classe Delegado. Um programa pode usar o
variável op para manter uma coleção de assinantes
ou para se referir a um único método.

Use expressões lambda
(métodos anônimos)

Delegados permitem que um programa trate comportamentos
(métodos em objetos) como itens de dados. Um delegado é um
item de dados que serve como referência a um método em
um objeto. Isso adiciona uma quantidade enorme de
flexibilidade para programadores. No entanto, os delegados são
trabalho duro para usar. O tipo de delegado real deve primeiro
ser declarado e, em seguida, feito para se referir a um determinado
método contendo o código que descreve a ação
a ser executado.

As expressões lambda são uma maneira pura de expressar
o “algo entra, algo acontece e
algo sai ”parte de comportamentos. Os tipos de
os elementos e o resultado a ser devolvido são
inferido a partir do contexto em que o lambda
expressão é usada. Considere a seguinte declaração.

```csharp
delegate int Operacao(int a, int b);
```

Esta declaração declara o delegado IntOperation que foi usado na Listagem. o
Delegado de IntOperation pode se referir a qualquer operação
que leva em dois parâmetros inteiros e retorna um
resultado inteiro. Agora considere esta afirmação, que
cria um delegado IntOperation chamado adicionar e
atribui-lo a uma expressão lambda que aceita dois
parâmetros de entrada e retorna sua soma.

```csharp
Operacao adicionar = (a, b) => a + b;
```

O operador `=>` é chamado de operador lambda. o
os itens aeb na esquerda da expressão lambda são
mapeado nos parâmetros do método definidos pelo
delegar. A declaração 011 o direito do lambda
expressão dá o comportamento da expressão, e
neste caso, adiciona os dois parâmetros juntos.
Ao descrever o comportamento do lambda
expressão você pode usar a frase "entra em" para
descreva o que está acontecendo. Neste caso, você poderia dizer
“Aeb entrem em um plus b.” O nome lambda vem
de lambda calculus, um ramo da matemática que
diz respeito à "abstração funcional".
Esta expressão lambda aceita dois inteiros
parâmetros e retorna um inteiro. Lambda
expressões podem aceitar vários parâmetros e
contém várias instruções, em que o caso
declarações são colocadas em um bloco. Listagem 1-72 mostra
como criar uma expressão lambda que imprime um
mensagem, bem como realizar um cálculo.

```csharp
Operacao adicionar = (a, b) =>
{
    Console.WriteLine("Foi chamado: adicionar");
    return a + b;
};
```

Fechamentos

O código em uma expressão lambda pode acessar variáveis
no código em torno dele. Essas variáveis devem ser
disponível Quando a expressão lambda é executada,
o compilador estenderá a vida útil das variáveis usadas
expressões lambda.
Listagem 1—73 mostra como isso funciona. O método
SetLo cal declara uma variável local chamada 1 ocal Int
e define seu valor para 99. Em circunstâncias normais
a variável local Int seria destruída após
conclusão do método SetLocal. No entanto, o
A variável localInt é usada em uma expressão lambda,
Qual é atribuído ao delegado getLocal. o
compilador garante que a variável lo cal Int seja
disponível para uso na expressão lambda Quando é
posteriormente chamado do método Main. este
extensão da vida variável é chamado de encerramento.

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

Tipos nativos para uso com expressões lambda 

Considere as três declarações a seguir:

```csharp
delegate int Operacao(int a, int b);
Operacao adicionar = (a, b) => a + b;
Console.WriteLine(adicionar(2, 2);
```

A primeira instrução cria um delegado chamado
IntOperation que aceita dois valores inteiros e
retorna um resultado inteiro. A segunda declaração
cria uma IntOperation chamada add que usa um
expressão lambda para descrever o que faz, que é
para adicionar os dois parâmetros juntos e retornar o
resultado. A terceira declaração realmente usa o add
operação para calcular e imprimir 2 + 2.
Isso funciona, mas tivemos que criar o
Tipo delegado IntOperation para especificar um comportamento
que aceita dois inteiros e retorna sua soma antes
poderíamos criar algo que se referisse a um lambda
expressão desse tipo. Há um certo número de
em "tipos delegados que podemos usar para fornecer um contexto
para uma expressão lambda.

Os tipos Func fornecem uma gama de delegados para
métodos que aceitam valores e retornam resultados. Listagem
1—74 mostra como o tipo Func é usado para criar um
adicionar comportamento que tenha o mesmo tipo de retorno e
parâmetros como o delegado IntOperation na Listagem
1-71. Existem versões do tipo Func que aceitam
até 16 itens de entrada. O método add aqui aceita
dois inteiros e retorna um inteiro como resultado.

```csharp
static Func<int, int, int> adicionar = (a, b) => a + b;
```

Se a expressão lambda não retornar um resultado,
você pode usar o tipo de ação que você viu anteriormente
quando criamos nossos primeiros delegados. A declaração
abaixo cria um delegado chamado logM-es sage que
refere-se a uma expressão lambda que aceita uma string
e depois imprime no console. Para diferentes formas
de registrar o logMes 3 idade delegado pode ser anexado ao
outros métodos que salvam os dados do log em um arquivo.

```csharp
static Action<string> logMensagem = (mensagem) => Console.WriteLine(mensagem);
```

O predicado incorporado ao tipo de delegado permite
criar código que leva um valor de um determinado tipo e
retorna verdadeiro ou falso. The divideByThree
predicado abaixo retorna true se o valor for divisível
por 3.

```csharp
static Predicate<int> divisivelPor3 = (i) => i % 3 == 0;
```

Métodos anônimos

Até agora temos usado expressões lambda
que estão ligados aos delegados. O delegado fornece um
name pelo qual o código na expressão lambda pode
ser acessado. No entanto, uma expressão lambda também pode
ser usado diretamente em um contexto onde você quer apenas
expressar um comportamento particular. O programa na listagem
1-75 usa a tarefa. Corra para iniciar uma nova tarefa. O código
realizada pela tarefa é expressa diretamente como
expressão lambda, que é dada como um argumento para
a tarefa . Execute o método. Em nenhum momento esse código
já tem um nome.

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
        Console.WriteLine("A tarefa está executando...");
        Console.ReadKey();
    }
}
```

Uma expressão lambda usada desta maneira pode ser
descrito como um método anônimo; porque é um
código funcional que não tem nome.