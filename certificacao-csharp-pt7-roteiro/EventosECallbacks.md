# Criar e implementar enventos e callbacks

Até aqui, você viu que a execução de um programa avança de instrução para
declaração, processando os dados de acordo com o
declarações que são executadas. Na computação tradicional,
o programa fluiria do início ao fim, começando
com os dados de entrada e produzindo alguma saída antes
de terminar. Entretanto, aplicações modernas possuem fluxos que
funcionam de maneira diferente da computação tradicional. 
Grande aplicações envolvem componentes que se comunicam através 
de mensagens, e isso produz um fluxo que foge da execução sequencial
(linha-a-linha).

Para criar soluções que funcionem dessa maneira
precisamos de um mecanismo pelo qual um componente possa
enviar mensagens para outro. Com a linguagem C#, podemos realizar isso
através dos **eventos**.

**Manipuladores de eventos**

Nos dias anteriores, async e await foram adicionados
a linguagem C#, um programa seria forçado a usar
eventos para gerenciar operações assíncronas. Antes
iniciar uma tarefa assíncrona, como buscar um
página da web de um servidor, um programa precisaria
ligar um método a um evento que seria gerado
Quando a ação foi concluída. Hoje, os eventos são
mais freqüentemente usado para processos
comunicação.

Vimos que um objeto pode fornecer um serviço
para outros objetos, expondo um método público que pode
ser chamado para executar esse serviço. Por exemplo, em um
aplicativo de console, um programa pode usar o
Método WriteLine exposto pelo console para
exibir mensagens para o usuário do programa.
Eventos são usados no reverso dessa situação,
Quando você deseja que um objeto notifique outro objeto
Aconteceu alguma coisa. Um objeto pode ser feito para
publicar eventos aos quais outros objetos podem se inscrever.
Componentes de uma solução que se comunicam usando
eventos deste modo são descritos como fracamente acoplados.
A única coisa que um componente tem que saber sobre o
outro é o design da publicação e se inscrever
mecanismo.

Delegados e eventos

Para entender como os eventos são implementados, você
para entender o conceito de um delegado C #. Isto é um
peça de dados que contém uma referência a um determinado
método em uma classe. Quando você pega seu carro por um
serviço que você dá ao atendente de garagem seu telefone
número para que eles possam te chamar Quando seu carro estiver pronto para
ser apanhada. Você pode pensar em um delegado como o
“Número de telefone” de um método em uma classe. Um evento
editor é dado um delegado que descreve o
método no assinante. O editor pode então ligar
esse delegado Quando o evento dado ocorre e o
método será executado no assinante.

Delegado de ação

As bibliotecas .NET fornecem um número pré-definido de
tipos de delegados. Na seção "Criar delegados", você
vai descobrir como criar seus próprios tipos de delegados.
Há uma série de ações pré-definidas
tipos de delegados. O delegado de ação mais simples
representa uma referência a um método que não
retornar um resultado (o método é do tipo void) e
não aceita nenhum parâmetro. Você pode usar um
Ação para criar um ponto de ligação para assinantes.
A Listagem 1-64 mostra como um delegado da Action pode ser
usado para criar um editor de eventos. Ele contém um
Classe de campainha que publica para assinantes Quando um
campainha é levantado. O evento Action delegate é chamado
OnCampainhaTocou. Um processo interessado em campainhas pode
associar assinantes a este evento. O método Tocar é chamado na campainha para tocar a campainha. Quando
O Tocar executa primeiro verificações para ver se algum
métodos de assinante foram vinculados ao
Delegado OnCampainhaTocou. Se existirem ouvintes, o delegado
é chamado.


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

Este programa gera a seguinte saída:

```
A campainha tocou.
A campainha tocou.
```

Inscritos no evento

Os assinantes se ligam a um editor usando o sinal + =
operador. O operador + = está sobrecarregado para aplicar
entre um delegado e um comportamento. Significa "adicionar isto
comportamento àqueles para este delegado. ”Os métodos
em um delegado não é garantido para ser chamado no
ordenar que eles foram adicionados ao delegado. Você pode
saiba mais sobre sobrecarga no “Create Types”
seção.

Os delegados adicionados a um evento publicado são chamados
o mesmo thread que o segmento publicando o evento. E se
um delegado bloqueia esse segmento, a publicação inteira
mecanismo está bloqueado. Isto significa que um malicioso ou
assinante mal escrito tem a capacidade de bloquear o
publicação de eventos. Isto é abordado pelo
editora iniciando uma tarefa individual para executar cada
os assinantes do evento. O objetivo do Delegado em um
editor expõe um método chamado
GetlnvokcationList, que pode ser usado para obter uma
lista de todos os assinantes. Você pode ver este método em
usar mais tarde nas “Exceções nos assinantes do evento”
seção.

Você pode simplificar a chamada do delegado usando
o operador condicional nulo. Isso só executa uma
ação se o item especificado não for nulo.
OnCampainhaTocou? . Invoke ();
O operador condicional nulo “.?” Significa “somente
acesse este membro da turma se a referência não for
nulo. ”Um delegado expõe um método Invoke para
invoque os métodos ligados ao delegado. o
O comportamento do código é o mesmo da Listagem 1.
64, mas o código é mais curto e mais claro.

Cancelar inscrição de um delegado

Você viu que o operador += foi sobrecarregado
para permitir que métodos se liguem a eventos. O método - = é
usado para cancelar a inscrição de eventos. O programa em
Listagem 1-66 liga dois métodos à campainha, levanta
a campainha, desvincula um dos métodos e toca a campainha novamente.


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

Se o mesmo assinante for adicionado mais de uma vez para
o mesmo editor, ele será chamado de um correspondente
número de vezes Quando o evento ocorre.

Usando eventos

O objeto de braço Al que criamos não é
particularmente seguro. O delegado do OnAlarmRais-ed
foi tornada pública para que os assinantes possam
conecte-se a ele. No entanto, isso significa que o código externo
para o objeto Alarme pode disparar o alarme diretamente
chamando o delegado OnAlarmRaised. Código externo
pode sobrescrever o valor de OnAlarmRai sed,
potencialmente removendo assinantes.

C # fornece uma construção de evento que permite uma
delegado a ser especificado como um evento. Isso é mostrado em
Listagem 1-67. O evento da palavra-chave é adicionado antes do
definição do delegado. O membro
OnAlarmRaised agora é criado como um campo de dados no
Classe de alarme, em vez de uma propriedade.
OnAlarmRaised não tem mais get ou set
comportamentos. No entanto, agora não é possível para o código
externo à classe Alarm para atribuir valores a
OnAl armRai sed, e o OnAl armRa i sed delegate
só pode ser chamado de dentro da classe Onde é
declarado. Em outras palavras, adicionando a palavra-chave do evento
transforma um delegado em um evento adequadamente útil.

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

O código na listagem 1-67 acima tem outro
melhoria em relação às versões anteriores. Cria uma
delegar instância e atribui quando
OnAlarmRaised é criado, então agora não há necessidade
verificar se o delegado tem ou não um valor
antes de chamá-lo. Isso simplifica o RaiseAlarm
método.

Criar eventos com tipos de delegação internos
Os delegados do evento criados até agora usaram o
Classe de ação como o tipo de cada evento. Isso vai
trabalho, mas programas que usam eventos devem usar o
EventHandler Class em vez de Action. Isto é
porque a classe EventHandler é a parte do .NET
projetado para permitir que os assinantes recebam dados sobre
um evento. EventHandler é usado em todo o
.NET framework para gerenciar eventos. A
EventE-Iandler pode fornecer dados, ou pode apenas sinalizar
que um evento ocorreu. Listagem 1—68 mostra como
a classe Alarm pode usar um EventI-Iandler para
indicam que um alarme foi disparado.

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
delegado deve refletir isso. O AlarmListenerl
método aceita dois parâmetros e pode ser usado com
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

A classe Alarm criada na Listagem 1—68 permite um
assinante para receber uma notificação de que um alarme
foi criado, mas não fornece ao assinante
qualquer descrição do alarme. É útil se os assinantes
pode receber informações sobre o alarme. Talvez um
string descrevendo a localização do alarme seria
útil.

Você pode fazer isso criando uma classe que possa entregar
esta informação e, em seguida, use um EventHandler para
entregue Isso. Listagem 1—69 mostra o AlarmEventArgs
class, que é uma subclasse da classe Eve ntArgs,
e adiciona uma propriedade Location a ele. Se mais evento
informação é necessária, talvez a data e hora de
alarme, estes podem ser adicionados ao
Classe AlarmEventArgs.

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
descreve um evento que ocorreu. O evento é o
alarme sendo gerado, e o tipo que você criou é
chamado AlarmEventAgs. Quando o alarme é levantado nós
deseja que o manipulador do evento de alarme aceite
Objetos AlarmEventArgs para que o manipulador possa ser
detalhes do evento.

O delegado EventHandler para o
Evento OnAlarmRai sed é declarado para entregar
argumentos do tipo AlarmEventArgs. 'Quando o
alarme é gerado pelo método Rai seAlarm o evento
é dada uma referência ao alarme e um recém-criado
instância de AlarmEventArgs que descreve o
evento de alarme.

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
AlarmEventArgs e pode usar os dados nele. o
método AlarmLi stenerl abaixo exibe o
localização do alarme que obtém de sua
argumento.

```csharp
static void CampainhaTocou1(object sender, CampainhaEventArgs e)
{
    Console.WriteLine($"A campainha tocou no apartamento {e.Apartamento}.");
}
```

Note que uma referência ao mesmo
O objeto AlarmEventArgs é passado para cada um dos
assinantes do evento OnAlarmRaised. este
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

Nos nossos programas de exemplo, o evento é um alarme
sendo desencadeada. Quando o alarme é acionado, o evento
chamará todos os manipuladores de eventos que se inscreveram
o evento de alarme. Mas o que acontece se um dos eventos
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