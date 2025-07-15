# Zeratul - Game of Cards

A Unity-based card game with strategic grid-based combat and AI opponents.

## Project Structure

### Architecture Overview

This project follows a modular architecture with the following main systems:

- **Card System**: Core card mechanics with Character and Spell types
- **Grid System**: Strategic board-based gameplay
- **AI System**: Behavior tree-based enemy AI
- **UI System**: Card display, movement, and interface management
- **Audio System**: Sound effects and music management

### Class Diagram

```mermaid
classDiagram
    %% Core Card Classes
    class Card {
        <<ScriptableObject>>
        +string cardName
        +string description
        +int cost
        +Sprite cardImage
        +List~ElementType~ cardType
        +Clone() Card
        <<enumeration>> ElementType
        <<enumeration>> DamageType
        <<enumeration>> SpellType
        <<enumeration>> AttributeTarget
    }

    class Character {
        <<ScriptableObject>>
        +int damage
        +int health
        +DamageType damageType
        +Clone() Card
    }

    class Spell {
        <<ScriptableObject>>
        +SpellType spellType
        +List~AttributeTarget~ attributeTargets
        +int healthChange
        +int damageChange
        +DamageType cardElementChangeTo
        +Clone() Card
    }

    %% Behavior Tree AI System
    class BTNode {
        <<abstract>>
        +Tick() State
        <<enumeration>> State
    }

    class BTComposite {
        <<abstract>>
        #List~BTNode~ children
        +BTComposite(BTNode[]) BTComposite
    }

    class BTAction {
        -Func~State~ action
        +BTAction(Func~State~) BTAction
        +Tick() State
    }

    class BTSequence {
        +BTSequence(BTNode[]) BTSequence
        +Tick() State
    }

    %% Core Managers
    class GameManager {
        <<Singleton>>
        +static GameManager Instance
        -int playerHealth
        -int difficultyLevel
        +bool playingCard
        +OptionsManager optionsManager
        +DeckManager deckManager
        +AudioManager audioManager
        +TextMeshProUGUI HPPlayerText
        +TextMeshProUGUI HPEnemyText
        +TextMeshProUGUI ManaPlayerText
        +TextMeshProUGUI ManaEnemyText
        +StartPlayerTurn() void
        +EndPlayerTurn() void
        +StartEnemyTurn() void
        +EndEnemyTurn() void
        +UpdateUI() void
        +PlayerHealth int
        +PlayerMana int
        +EnemyHealth int
        +EnemyMana int
    }

    class DeckManager {
        <<MonoBehaviour>>
        +List~Card~ playerCards
        +List~Card~ enemyCards
        +int startingHandSize
        +int maxHandSize
        +HandManager handManager
        +HandManager enemyHandManager
        +DrawPileManager drawPileManager
        +DrawPileManager enemyDrawPileManager
        +BattleSetup() void
    }

    class HandManager {
        <<MonoBehaviour>>
        +GameObject cardPrefab
        +Transform handTransform
        +float fanSpread
        +float cardDistance
        +int maxHandSize
        +float verticalSpacing
        +List~GameObject~ cardsInHand
        +AddCardToHand(Card) void
        +UpdateCardPositions() void
        +OnCardRemoved() void
        +BattleSetup(int) void
    }

    class DrawPileManager {
        <<MonoBehaviour>>
        +List~Card~ drawPile
        +int startingHandSize
        +int maxHandSize
        +TextMeshProUGUI drawPileText
        +bool isEnemy
        +InitializeDrawPile(List~Card~) void
        +DrawCard(HandManager) void
        +BattleSetup(int, int) void
        +BattleSetup(int, int, HandManager) void
    }

    class DiscardManager {
        <<MonoBehaviour>>
        +List~Card~ discardedCards
        +TextMeshProUGUI discardText
        +int discardCardCount
        +AddCardToDiscard(Card) void
        +PullFromDiscard() Card
        +PullSelectedCardFromDiscard(Card) bool
        +PullAllFromDiscard() List~Card~
    }

    %% Grid System
    class GridManager {
        <<MonoBehaviour>>
        +int width
        +int height
        +GameObject cardPrefab
        +GameObject cellPrefab
        +List~GameObject~ occupiedCells
        +GameObject[,] gridCells
        +InitializeGrid() void
        +AddOccupantToGrid(Card, Vector2) bool
        +IsCellOccupied(Vector2) bool
        +GetCardAtGridIndex(Vector2) Card
    }

    class GridCell {
        <<MonoBehaviour>>
        +Vector2 gridIndex
        +bool isOccupied
        +GameObject occupant
    }

    class GridCellHighlighter {
        <<MonoBehaviour>>
        +Color posColor
        +Color negColor
        +Color highlightColor
        +GridCell gridCell
        +OnMouseEnter() void
        +OnMouseExit() void
    }

    %% Card Display and Movement
    class CardDisplay {
        <<MonoBehaviour>>
        +Card cardData
        +Image cardImage
        +TMP_Text nameText
        +TMP_Text costText
        +TMP_Text descText
        +GameObject characterCardElements
        +GameObject spellCardElements
        +TMP_Text healthText
        +TMP_Text damageText
        +Image[] typeImages
        +GameObject[] spellTypeLabels
        +UpdateCardDisplay() void
    }

    class CardMovement {
        <<MonoBehaviour>>
        +bool hasBeenPlayed
        +float selectScale
        +GameObject glowEffect
        +GameObject playArrow
        +OnPointerEnter(PointerEventData) void
        +OnPointerExit(PointerEventData) void
        +OnPointerDown(PointerEventData) void
        +OnDrag(PointerEventData) void
        +InitializeCardState(int, bool) void
    }

    %% Support Managers
    class OptionsManager {
        <<MonoBehaviour>>
        +bool isMusicEnabled
        +List~TMP_FontAsset~ fonts
        +static event Action OnFontChanged
        +GetFontClass(string) TMP_FontAsset
        +UpdateFont() void
    }

    class AudioManager {
        <<MonoBehaviour>>
        +AudioSource musicSource
        +AudioSource SFXSource
        +AudioClip backgroundMusic
        +AudioClip deathSound
        +playSFX(AudioClip) void
    }

    class FontSetter {
        <<MonoBehaviour>>
        +string fontClass
        +SetFont() void
        +OnEnable() void
        +OnDisable() void
    }

    %% UI Components
    class UIPositionObject {
        <<MonoBehaviour>>
        +RectTransform objectToPosition
        +int widthDivider
        +int heightDivider
        +float widthMultiplier
        +float heightMultiplier
        +bool updatePosition
        +SetUIObjectPosition() void
    }

    class DragUIObject {
        <<MonoBehaviour>>
        +float movementSensitivity
        +OnPointerDown(PointerEventData) void
        +OnDrag(PointerEventData) void
    }

    %% Menu Classes
    class MainMenu {
        <<MonoBehaviour>>
        +GameObject volumeMenuUI
        +NewGame() void
        +LoadLevel1() void
        +Quit() void
    }

    class EndScreen {
        <<MonoBehaviour>>
        +float delayBeforeClickable
        +EnableClick() void
    }

    %% Utility Classes
    class SpellEffectApplier {
        <<static>>
        +static ApplySpellEffect(Spell, Card) void
        +static ApplyAttributeChange(Spell, SpellType, AttributeTarget, int, Character) void
    }

    %% AI
    class EnemyAI {
        <<MonoBehaviour>>
        +ExecuteTurn() void
    }

    %% Relationships
    Card <|-- Character
    Card <|-- Spell
    
    %% Behavior Tree Relationships
    BTNode <|-- BTComposite
    BTNode <|-- BTAction
    BTComposite <|-- BTSequence
    BTComposite o-- BTNode : contains
    
    GameManager --> OptionsManager
    GameManager --> DeckManager
    GameManager --> AudioManager
    GameManager --> EnemyAI
    
    %% EnemyAI likely uses the Behavior Tree system
    EnemyAI --> BTNode : uses
    
    DeckManager --> HandManager
    DeckManager --> DrawPileManager
    
    HandManager --> Card
    DrawPileManager --> Card
    DrawPileManager --> HandManager
    DrawPileManager --> DiscardManager
    DiscardManager --> Card
    
    GridManager --> GridCell
    GridManager --> Card
    GridCell --> GridCellHighlighter
    
    CardDisplay --> Card
    CardDisplay --> Character
    CardDisplay --> Spell
    CardMovement --> CardDisplay
    CardMovement --> HandManager
    CardMovement --> DiscardManager
    CardMovement --> GridManager
    
    FontSetter --> OptionsManager
    SpellEffectApplier --> Spell
    SpellEffectApplier --> Character
    
    %% Unity Component Dependencies
    CardMovement --|> IPointerDownHandler
    CardMovement --|> IDragHandler
    CardMovement --|> IPointerEnterHandler
    CardMovement --|> IPointerExitHandler
    
    DragUIObject --|> IPointerDownHandler
    DragUIObject --|> IDragHandler
```

## Key Systems

### Card System
- **Card**: Base ScriptableObject for all cards
- **Character**: Combat units with health and damage
- **Spell**: Special effect cards with various targets

### AI System
- **Behavior Trees**: Modular AI decision making
- **BTNode**: Base class for all behavior tree nodes
- **BTSequence**: Executes child nodes in sequence
- **BTAction**: Leaf nodes that perform specific actions

### Game Management
- **GameManager**: Singleton controlling game state
- **DeckManager**: Handles player and enemy card collections
- **HandManager**: Manages cards in hand positioning and display

## Getting Started

1. Open the project in Unity 2022.3 or later
2. Load the `StartMenu` scene to begin
3. Main gameplay occurs in the `CombatScene`

## Development Notes

- All card data is stored as ScriptableObjects in `Assets/Cards/`
- AI behaviors are defined using the behavior tree system
- Grid-based combat uses a 10x5 grid system
- UI elements are responsive and support multiple screen sizes
