# Alfabeto dos Sinais (CatLibras)

Ajude o gatinho a encontrar a letra certa! Um jogo educativo em 2D para praticar o alfabeto
manual da Libras (Língua Brasileira de Sinais): os sinais das mãos chegam pela tela e você
precisa coletar exatamente aquele que corresponde à letra sorteada.

![Logo CatLibras](Assets/New%20Assets/logofinal.png)

## Sobre o jogo

O jogador controla um gato que se move livremente pela parte inferior da tela. A cada rodada
uma **letra-alvo** é sorteada e exibida na interface. Os sinais em Libras (A–Z) aparecem à
direita e atravessam a tela da direita para a esquerda. Coletar o sinal correto soma pontos;
tocar em um sinal errado — ou deixar o sinal correto passar — custa uma vida.

O ritmo acelera com o tempo e o placar também cresce continuamente, então a melhor estratégia
é ser rápido e preciso. Ao acertar uma sequência de sinais, o jogador recupera uma vida.

## Como jogar

| Ação | Tecla |
| :--- | :--- |
| Mover o gato | Setas direcionais ou **W A S D** |
| Interagir com os menus | Mouse |
| Pausar | Botão de pause na interface |

Regras:

* **Acerto**: coletar o sinal da letra exibida → +100 pontos e nova letra sorteada.
* **Erro**: coletar a letra errada → perde uma vida (3 no total).
* **Sinal perdido**: deixar o sinal correto ultrapassar o gato → perde uma vida.
* **Bônus**: uma sequência de acertos devolve uma vida ao jogador.
* **Fim de jogo**: quando as vidas acabam, a pontuação final é exibida.

## Estrutura do projeto

```
Assets/
├── Animations/          # Animações e Animators (gato, letra atual, fundo, botões)
├── Audio/               # Trilha sonora (intro e loop do gameplay)
├── CatLibrasButtons/    # Sprites de botões e ícones de som do jogo
├── Prefabs/
│   ├── Cat.prefab           # Jogador
│   ├── Level Controller.prefab  # Gerencia estados, placar e vidas
│   ├── Level.prefab         # Interface e cenário do nível
│   ├── Spawner.prefab       # Gerador de sinais
│   └── Libras/              # Variante de cada letra (A–Z)
├── Scenes/
│   └── GameScene.unity      # Única cena do jogo
├── Scripts/             # Código-fonte em C#
├── Settings/            # Configuração do URP e templates de renderização
├── Sprites/
│   ├── Maos/                # Sinais em Libras (LibrasA.png … LibrasZ.png)
│   └── ...                  # Logo, fundo, corações e ícones de UI
└── TextMesh Pro/        # Fontes e recursos de texto
```

### Scripts principais

| Script | Responsabilidade |
| :--- | :--- |
| `LevelController.cs` | Regras de jogo, placar, vidas, sorteio da letra-alvo e troca de estados (menu, tutorial, jogo, derrota). |
| `PlayerController.cs` | Movimento do gato, física 2D e detecção de coleta. |
| `SinalsSpawner.cs` | Instancia os sinais e controla a velocidade crescente da partida. |
| `CollectableSignal.cs` | Comportamento de um sinal individual (letra e deslocamento pela tela). |
| `CheckCollider.cs` | Detecta o sinal correto que passa sem ser coletado. |
| `MusicController.cs` / `SoundControl.cs` | Controle de música e efeitos sonoros, incluindo mudo. |
| `ToggleImage.cs` | Troca o sprite de botões do tipo *toggle* (ex.: som ligado/desligado). |
| `MatchWidth.cs` | Ajusta a câmera ortográfica para manter a largura de cena em qualquer resolução. |

## Como executar

1. Instale o **Unity 2021.3.11f1 [LTS]** (versão usada no projeto).
2. Abra o projeto pela Unity Hub, apontando para a pasta raiz deste repositório.
3. Abra a cena `Assets/Scenes/GameScene.unity`.
4. Pressione **Play** no editor.
5. Para gerar um executável, use **File → Build Settings** e selecione a plataforma desejada
   (o projeto foi preparado para **Standalone Windows** e **WebGL**).

> O projeto usa o Unity Render Pipeline (URP) e depende dos pacotes listados em
> `Packages/manifest.json`, que são resolvidos automaticamente pelo Unity.

## Guia para programação

Abaixo estão algumas indicações de como deve ser escrito o código com a finalidade de garantir
clareza, eficiência e organização. Ajude na legibilidade do código que muito provavelmente terá
de ser alterado no futuro.

**VERSÃO DO UNITY: 2021.3.11f1 [LTS]**

### Dicas gerais

* Utilize funções pontuais que atinjam um objetivo consistentemente: o requisito pode mudar,
  mas com um código bem preparado você poderá alterar apenas parâmetros para atingir um novo
  objetivo, sem necessidade de reescrever o código.
* Organize as pastas de forma a ser fácil de encontrar quais assets e scripts são usados em
  cada cena.
* Siga padrões de nomenclatura e use nomes claros para variáveis e métodos, valorizando mais
  uma função que execute uma tarefa do que um código monolítico com comentários.
* No caso de um código não estar mais sendo utilizado, retire-o ou comente-o. Evitando códigos
  redundantes achar bugs é muito mais direto.
* Escreva todo o código (nomes de variáveis, funções, classes, etc.) e comentários em inglês.

### Layout de arquivo

Sempre organize seu arquivo em seções de acordo com a tabela abaixo.

| Seção | Linha vazia entre membros |
| :---: | :---: |
| using directive | Não usar |
| Eventos | Opcional |
| SerializeFields | Opcional |
| Variáveis | Opcional |
| Properties | Opcional |
| Métodos/Funções | Obrigatório |

### Naming Conventions

Para a maioria dos casos usaremos a naming convention **PascalCase**. Abaixo há uma tabela com
exceções a isto:

| Tipo | Convenção |
| :---: | :---: |
| Private fields | _camelCase |
| Protected fields | PascalCase |
| const fields | PascalCase |
| Variáveis de métodos/funções | camelCase |
| Eventos | OnPascalCase |
| Interfaces | IPascalCase |
| Argumentos genéricos | TPascalCase |

### Nomeação de eventos

Para eventos, sempre inicie com o prefixo **On**. Para o resto, use **pretérito** para eventos
que são chamados ao final da lógica, como abaixo:

```cs
class Player
{
    public event Action OnDied;   // Past tense (pretérito)

    public void Die()
    {
        // Dying logic...
        OnDied?.Invoke();       // Chamado ao final da lógica de Dying
    }
}
```

Use **presente simples** para eventos que são chamados anteriormente na lógica, como abaixo:

```cs
class Player
{
    public event Action OnDie;   // Presente simples

    public void Die()
    {
        OnDie?.Invoke();         // Evento chamado antes da lógica de dying
        // Dying logic...
    }
}
```

### Nomeações de manipuladores de eventos (Event Handlers)

Para métodos que manipulam eventos particulares, use o prefixo **Handle**, **NÃO USE O PREFIXO
On**. Exemplo:

```cs
class SomeUI
{
    public void Setup(Player player)
    {
        player.OnDied += HandlePlayerDied;    // Handler method starts with Handle
    }

    private void HandlePlayerDied() { }
}
```

### Namespaces

Uso de namespaces é recomendado no projeto a fim de facilitar a visualização de dependências, e
evitar conflitos com classes que podem ter o mesmo nome, porém estarem presentes em namespaces
distintas. Ao usar namespace, garanta que o seu nome corresponda ao caminho das pastas. Exemplo:
um script `CharacterController.cs` localizado na pasta `Scripts/Game/Character` deve ter a
seguinte namespace:

```cs
namespace Game.Character // corresponde ao caminho nas pastas
{
    public class CharacterController
    {
    }
}
```

### Variáveis e declarações

Sempre atribua variáveis como privadas nas classes. Modificar o valor de uma variável fora do
escopo da classe torna o processo de debug difícil. Se for necessário acessar o valor dessa
variável externamente, use o recurso de Properties do C#. Se for necessário alterar o valor da
variável externamente, configure o `set` da property.

```cs
private int _sum;
public int Sum { get { return _sum; } set { _sum = value; } }
```

Somente acesso de leitura, sem poder escrever:

```cs
private int _sum;
public int Sum => _sum;
```

Não declare múltiplas variáveis em uma mesma linha. Isso pode causar complicações no
versionamento e controle do git, além de tornar a legibilidade ruim.

```cs
// Incorreto
private int _a, _b, _c;

// Correto
private int _a;
private int _b;
private int _c;
```

### Comentários

Comentários, assim como o código, necessitam de manutenção. Isto dá trabalho, e frequentemente
pode ser esquecido, deixando-o desatualizado. Por essa razão, evite o uso de comentários tanto
quanto possível. O código deve sempre ser auto-explicativo, favorecendo sua legibilidade e
clareza. Práticas de código limpo (Clean Code) ajudam a alcançarmos esta meta.

### Exemplo de uso desse guia

```cs
using System;
using UnityEngine;

namespace MyNamespace
{
    public class SampleClass
    {
        public event Action OnSomethingHappened;

        private float _myFloat;
        private float _myOtherFloat;

        public SampleClass() { }

        public float MyFloat => _myFloat;

        public void Public() { }

        public int OtherPublic() { }

        protected void Protected() { }

        private void Private() { }

        private void HandleSomethingHappened() { }
    }
}
```

## Créditos

Desenvolvido pelo **CEGI — Centro de Estudos em Games e Internet (Unifesp)**.

* Site: [cegi.unifesp.br](https://cegi.unifesp.br/)
* Organização no GitHub: [cegiunifesp](https://github.com/cegiunifesp)