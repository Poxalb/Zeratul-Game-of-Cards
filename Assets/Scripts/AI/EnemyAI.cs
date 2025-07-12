using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    private DeckManager deckManager;
    private GameManager gameManager;
    private GridManager grid;
    
    // Behavior Tree components
    private BTNode aiDecisionTree;
    
    // AI state tracking
    private List<Card> currentHand = new List<Card>();
    private List<Character> playerThreats = new List<Character>();
    private List<Character> ownCharacters = new List<Character>();
    private int availableMana;
    
    // AI difficulty settings
    [SerializeField] private float aggressionLevel = 0.7f;
    [SerializeField] private float defensiveLevel = 0.5f;
    [SerializeField] private float spellingEfficiency = 0.8f;

    private void Awake()
    {
        deckManager = FindFirstObjectByType<DeckManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        grid = FindFirstObjectByType<GridManager>();
        
        InitializeBehaviorTree();
    }

    public void ExecuteTurn()
    {
        AdjustDifficultySettings();
        DrawIfPossible();
        AnalyzeBoardState();
        
        // Execute the behavior tree for decision making
        aiDecisionTree.Tick();
    }
    
    private void InitializeBehaviorTree()
    {
        // Create a sophisticated decision tree
        aiDecisionTree = new BTSelector(
            // High priority: Emergency response
            new BTSequence(
                new BTAction(() => IsInDanger()),
                new BTAction(() => HandleEmergencyResponse())
            ),
            
            // Medium priority: Offensive plays
            new BTSequence(
                new BTAction(() => CanMakeOffensivePlay()),
                new BTAction(() => ExecuteOffensiveStrategy())
            ),
            
            // Medium priority: Board control
            new BTSequence(
                new BTAction(() => ShouldControlBoard()),
                new BTAction(() => ExecuteBoardControl())
            ),
            
            // Low priority: Value plays
            new BTAction(() => ExecuteValuePlay())
        );
        
        AdjustDifficultySettings();
    }

    private void DrawIfPossible()
    {
        if (deckManager.enemyDrawPileManager.drawPile.Count > 0 && 
            deckManager.enemyHandManager.cardsInHand.Count < deckManager.enemyDrawPileManager.maxHandSize)
        {
            deckManager.enemyDrawPileManager.DrawCard(deckManager.enemyHandManager);
            Debug.Log("EnemyAI: Drew a card");
        }
        else if (deckManager.enemyDrawPileManager.drawPile.Count == 0)
        {
            Debug.LogWarning("EnemyAI: Draw pile is empty! Enemy takes 1 damage.");
            gameManager.EnemyHealth -= 1;
        }
    }

    private bool TryPlayAnyCard()
    {
        var hand = deckManager.enemyHandManager.cardsInHand;
        int mana = gameManager.EnemyMana;

        // Shuffle hand for random play order
        List<int> indexes = new List<int>();
        for (int i = 0; i < hand.Count; i++) indexes.Add(i);
        for (int i = 0; i < indexes.Count; i++)
        {
            int j = Random.Range(i, indexes.Count);
            int tmp = indexes[i];
            indexes[i] = indexes[j];
            indexes[j] = tmp;
        }

        foreach (int idx in indexes)
        {
            CardDisplay cd = hand[idx].GetComponent<CardDisplay>();
            if (cd == null) continue;
            Card card = cd.cardData;
            if (card.cost > mana) continue;

            // Character: find empty cell in enemy row
            if (card is Character)
            {
                List<int> emptyColumns = new List<int>();
                for (int x = 0; x < grid.width; x++)
                {
                    var cell = grid.gridCells[x, 1].GetComponent<GridCell>();
                    if (!cell.isOccupied)
                        emptyColumns.Add(x);
                }
                if (emptyColumns.Count > 0)
                {
                    int chosenCol = emptyColumns[Random.Range(0, emptyColumns.Count)];
                    var cell = grid.gridCells[chosenCol, 1].GetComponent<GridCell>();
                    Card cardInstance = card.Clone();
                    grid.AddOccupantToGrid(cardInstance, new Vector2(chosenCol, 1));
                    deckManager.enemyHandManager.cardsInHand.RemoveAt(idx);
                    Destroy(cd.gameObject);
                    gameManager.EnemyMana -= card.cost;
                    Debug.Log($"EnemyAI: Played character {cardInstance.cardName} at column {chosenCol}, row 1");
                    return true;
                }
            }
            // Spell: just cast it (implement your spell logic here)
            else if (card is Spell)
            {
                // TODO: Implement spell effect logic here
                Debug.Log($"EnemyAI: Cast spell {card.cardName}");
                deckManager.enemyHandManager.cardsInHand.RemoveAt(idx);
                Destroy(cd.gameObject);
                gameManager.EnemyMana -= card.cost;
                // For now, just play one spell per call
                return true;
            }
        }
        return false;
    }
    
    private void AnalyzeBoardState()
    {
        // Update available mana
        availableMana = gameManager.EnemyMana;
        
        // Update current hand
        currentHand.Clear();
        var hand = deckManager.enemyHandManager.cardsInHand;
        foreach (var cardObj in hand)
        {
            var cardDisplay = cardObj.GetComponent<CardDisplay>();
            if (cardDisplay != null)
                currentHand.Add(cardDisplay.cardData);
        }
        
        // Analyze player threats (characters in player row)
        playerThreats.Clear();
        for (int x = 0; x < grid.width; x++)
        {
            var playerCard = grid.GetCardAtGridIndex(new Vector2(x, 0));
            if (playerCard is Character character)
                playerThreats.Add(character);
        }
        
        // Analyze own characters (characters in enemy row)
        ownCharacters.Clear();
        for (int x = 0; x < grid.width; x++)
        {
            var enemyCard = grid.GetCardAtGridIndex(new Vector2(x, 1));
            if (enemyCard is Character character)
                ownCharacters.Add(character);
        }
    }
    
    private BTNode.State IsInDanger()
    {
        // Check if enemy health is critically low (scales with defensive level)
        int criticalHealthThreshold = Mathf.RoundToInt(8 * defensiveLevel);
        if (gameManager.EnemyHealth <= criticalHealthThreshold)
            return BTNode.State.Success;
            
        // Check if player has strong board presence
        int playerTotalDamage = playerThreats.Sum(c => c.damage);
        if (playerTotalDamage >= gameManager.EnemyHealth)
            return BTNode.State.Success;
            
        // Check if we have no board presence against multiple threats (defensive AI reacts faster)
        int threatThreshold = defensiveLevel > 0.6f ? 1 : 2;
        if (ownCharacters.Count == 0 && playerThreats.Count >= threatThreshold)
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State HandleEmergencyResponse()
    {
        // Priority 1: Play defensive spells on our characters
        if (TryPlayDefensiveSpells())
            return BTNode.State.Success;
            
        // Priority 2: Play debuff spells on enemy threats
        if (TryPlayDebuffSpells())
            return BTNode.State.Success;
            
        // Priority 3: Play any character to block
        if (TryPlayCharacterForDefense())
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State CanMakeOffensivePlay()
    {
        // Aggressive AI looks for lower damage thresholds
        int damageThreshold = aggressionLevel > 0.7f ? 2 : 3;
        
        // Check if we can deal significant damage to player
        var affordableCharacters = GetAffordableCharacters();
        if (affordableCharacters.Any(c => c.damage >= damageThreshold))
            return BTNode.State.Success;
            
        // Check if we can finish the game
        int directDamage = CalculateDirectDamageToPlayer();
        if (directDamage >= gameManager.PlayerHealth)
            return BTNode.State.Success;
            
        // Aggressive AI is more willing to attack
        if (aggressionLevel > 0.8f && affordableCharacters.Any())
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State ExecuteOffensiveStrategy()
    {
        // Try to play high-damage characters
        if (TryPlayHighDamageCharacters())
            return BTNode.State.Success;
            
        // Try to buff our existing characters
        if (TryPlayBuffSpells())
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State ShouldControlBoard()
    {
        // Check if we need more board presence
        if (ownCharacters.Count < playerThreats.Count)
            return BTNode.State.Success;
            
        // Check if we have efficient plays available
        if (GetAffordableCharacters().Any(c => c.damage >= 2 && c.cost <= 3))
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State ExecuteBoardControl()
    {
        // Play efficient characters
        if (TryPlayEfficientCharacters())
            return BTNode.State.Success;
            
        // Use spells to control the board
        if (TryPlayControlSpells())
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    private BTNode.State ExecuteValuePlay()
    {
        // Try to play any affordable card
        if (TryPlayAnyAffordableCard())
            return BTNode.State.Success;
            
        return BTNode.State.Failure;
    }
    
    // Helper methods for card evaluation and playing
    private List<Character> GetAffordableCharacters()
    {
        return currentHand.OfType<Character>()
            .Where(c => c.cost <= availableMana)
            .ToList();
    }
    
    private List<Spell> GetAffordableSpells()
    {
        return currentHand.OfType<Spell>()
            .Where(s => s.cost <= availableMana)
            .ToList();
    }
    
    private int CalculateDirectDamageToPlayer()
    {
        int damage = 0;
        for (int x = 0; x < grid.width; x++)
        {
            var playerCard = grid.GetCardAtGridIndex(new Vector2(x, 0));
            var enemyCard = grid.GetCardAtGridIndex(new Vector2(x, 1));
            
            // If enemy has character and player doesn't, it deals direct damage
            if (enemyCard is Character enemyCharacter && playerCard == null)
                damage += enemyCharacter.damage;
        }
        return damage;
    }
    
    private bool TryPlayDefensiveSpells()
    {
        var buffSpells = GetAffordableSpells()
            .Where(s => s.spellType == Card.SpellType.Buff)
            .OrderByDescending(s => s.healthChange + s.damageChange)
            .ToList();
            
        foreach (var spell in buffSpells)
        {
            // Find our weakest character to buff
            var targetCharacter = FindWeakestOwnCharacter();
            if (targetCharacter != null)
            {
                if (TryPlaySpell(spell, targetCharacter))
                    return true;
            }
        }
        return false;
    }
    
    private bool TryPlayDebuffSpells()
    {
        var debuffSpells = GetAffordableSpells()
            .Where(s => s.spellType == Card.SpellType.Debuff)
            .OrderByDescending(s => Mathf.Abs(s.healthChange) + Mathf.Abs(s.damageChange))
            .ToList();
            
        foreach (var spell in debuffSpells)
        {
            // Find strongest enemy character to debuff
            var targetCharacter = FindStrongestPlayerCharacter();
            if (targetCharacter != null)
            {
                if (TryPlaySpell(spell, targetCharacter))
                    return true;
            }
        }
        return false;
    }
    
    private bool TryPlayCharacterForDefense()
    {
        var characters = GetAffordableCharacters()
            .OrderByDescending(c => c.health + c.damage) // Prioritize overall stats
            .ToList();
            
        foreach (var character in characters)
        {
            if (TryPlayCharacter(character))
                return true;
        }
        return false;
    }
    
    private bool TryPlayHighDamageCharacters()
    {
        var characters = GetAffordableCharacters()
            .Where(c => c.damage >= 3)
            .OrderByDescending(c => c.damage)
            .ToList();
            
        foreach (var character in characters)
        {
            if (TryPlayCharacter(character))
                return true;
        }
        return false;
    }
    
    private bool TryPlayBuffSpells()
    {
        var buffSpells = GetAffordableSpells()
            .Where(s => s.spellType == Card.SpellType.Buff)
            .OrderByDescending(s => s.damageChange) // Prioritize damage buffs
            .ToList();
            
        foreach (var spell in buffSpells)
        {
            // Find our character that can benefit most
            var targetCharacter = FindBestBuffTarget();
            if (targetCharacter != null)
            {
                if (TryPlaySpell(spell, targetCharacter))
                    return true;
            }
        }
        return false;
    }
    
    private bool TryPlayEfficientCharacters()
    {
        var characters = GetAffordableCharacters()
            .OrderByDescending(c => (float)(c.damage + c.health) / c.cost) // Best stats per cost
            .ToList();
            
        foreach (var character in characters)
        {
            if (TryPlayCharacter(character))
                return true;
        }
        return false;
    }
    
    private bool TryPlayControlSpells()
    {
        var controlSpells = GetAffordableSpells()
            .Where(s => s.spellType == Card.SpellType.Debuff)
            .ToList();
            
        foreach (var spell in controlSpells)
        {
            var target = FindBestDebuffTarget();
            if (target != null)
            {
                if (TryPlaySpell(spell, target))
                    return true;
            }
        }
        return false;
    }
    
    private bool TryPlayAnyAffordableCard()
    {
        // Try characters first
        var characters = GetAffordableCharacters();
        foreach (var character in characters)
        {
            if (TryPlayCharacter(character))
                return true;
        }
        
        // Then try spells
        var spells = GetAffordableSpells();
        foreach (var spell in spells)
        {
            Character target = null;
            if (spell.spellType == Card.SpellType.Buff)
                target = FindWeakestOwnCharacter();
            else if (spell.spellType == Card.SpellType.Debuff)
                target = FindStrongestPlayerCharacter();
                
            if (target != null && TryPlaySpell(spell, target))
                return true;
        }
        
        return false;
    }
    
    private bool TryPlayCharacter(Character character)
    {
        // Find empty cells in enemy row (row 1)
        List<int> emptyColumns = new List<int>();
        for (int x = 0; x < grid.width; x++)
        {
            var cell = grid.gridCells[x, 1].GetComponent<GridCell>();
            if (!cell.isOccupied)
                emptyColumns.Add(x);
        }
        
        if (emptyColumns.Count > 0)
        {
            // Choose optimal column based on strategy
            int chosenCol = ChooseOptimalColumn(character, emptyColumns);
            
            // Play the character
            var handIndex = FindCardInHand(character);
            if (handIndex >= 0)
            {
                var hand = deckManager.enemyHandManager.cardsInHand;
                var cardObj = hand[handIndex];
                var cardInstance = character.Clone();
                
                grid.AddOccupantToGrid(cardInstance, new Vector2(chosenCol, 1));
                hand.RemoveAt(handIndex);
                Destroy(cardObj);
                gameManager.EnemyMana -= character.cost;
                availableMana = gameManager.EnemyMana;
                
                Debug.Log($"EnemyAI: Played character {character.cardName} at column {chosenCol}");
                return true;
            }
        }
        return false;
    }
    
    private bool TryPlaySpell(Spell spell, Character target)
    {
        var handIndex = FindCardInHand(spell);
        if (handIndex >= 0 && target != null)
        {
            // Find the grid position of the target
            Vector2 targetPosition = FindCharacterPosition(target);
            if (targetPosition.x >= 0) // Valid position found
            {
                var hand = deckManager.enemyHandManager.cardsInHand;
                var cardObj = hand[handIndex];
                
                // Apply spell effect
                SpellEffectApplier.ApplySpellEffect(spell, target);
                
                hand.RemoveAt(handIndex);
                Destroy(cardObj);
                gameManager.EnemyMana -= spell.cost;
                availableMana = gameManager.EnemyMana;
                
                Debug.Log($"EnemyAI: Cast spell {spell.cardName} on {target.cardName} at position {targetPosition}");
                return true;
            }
        }
        return false;
    }
    
    private Vector2 FindCharacterPosition(Character character)
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                var card = grid.GetCardAtGridIndex(new Vector2(x, y));
                if (card == character)
                    return new Vector2(x, y);
            }
        }
        return new Vector2(-1, -1); // Not found
    }
    
    private int FindCardInHand(Card card)
    {
        var hand = deckManager.enemyHandManager.cardsInHand;
        for (int i = 0; i < hand.Count; i++)
        {
            var cardDisplay = hand[i].GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData == card)
                return i;
        }
        return -1;
    }
    
    private int ChooseOptimalColumn(Character character, List<int> availableColumns)
    {
        // Strategy 1: Block strongest player character
        int bestColumn = -1;
        int highestThreat = -1;
        
        foreach (int col in availableColumns)
        {
            var playerCard = grid.GetCardAtGridIndex(new Vector2(col, 0));
            if (playerCard is Character playerCharacter)
            {
                if (playerCharacter.damage > highestThreat)
                {
                    highestThreat = playerCharacter.damage;
                    bestColumn = col;
                }
            }
        }
        
        // Strategy 2: If no direct threats, choose column for maximum future damage
        if (bestColumn == -1)
        {
            foreach (int col in availableColumns)
            {
                var playerCard = grid.GetCardAtGridIndex(new Vector2(col, 0));
                if (playerCard == null) // Empty column = direct damage to player
                {
                    bestColumn = col;
                    break;
                }
            }
        }
        
        // Fallback: Random column
        return bestColumn != -1 ? bestColumn : availableColumns[Random.Range(0, availableColumns.Count)];
    }
    
    private Character FindWeakestOwnCharacter()
    {
        return ownCharacters.OrderBy(c => c.health).FirstOrDefault();
    }
    
    private Character FindStrongestPlayerCharacter()
    {
        return playerThreats.OrderByDescending(c => c.damage).FirstOrDefault();
    }
    
    private Character FindBestBuffTarget()
    {
        // Prioritize characters that are in combat and need help
        return ownCharacters.OrderByDescending(c => c.damage).FirstOrDefault();
    }
    
    private Character FindBestDebuffTarget()
    {
        // Prioritize strongest enemy that threatens us most
        return playerThreats.OrderByDescending(c => c.damage + c.health).FirstOrDefault();
    }
    
    private void AdjustDifficultySettings()
    {
        int difficulty = gameManager.DifficultyLevel;
        
        // Scale AI parameters based on difficulty (1-10 scale)
        aggressionLevel = Mathf.Clamp01(0.3f + (difficulty * 0.07f));
        defensiveLevel = Mathf.Clamp01(0.2f + (difficulty * 0.08f));
        spellingEfficiency = Mathf.Clamp01(0.4f + (difficulty * 0.06f));
        
        Debug.Log($"EnemyAI Difficulty {difficulty}: Aggression={aggressionLevel:F2}, Defense={defensiveLevel:F2}, Spell={spellingEfficiency:F2}");
    }
}