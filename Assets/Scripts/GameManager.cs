using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Add this if not present

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int playerHealth = 20;

    private int difficultyLevel = 5;


    public OptionsManager optionsManager { get; private set; }
    public DeckManager deckManager { get; private set; }
    public AudioManager audioManager { get; private set; }


    public bool playingCard = false; // Flag to indicate if a card is being played

    public TextMeshProUGUI HPPlayerText;
    public TextMeshProUGUI HPEnemyText;
    public TextMeshProUGUI ManaPlayerText;
    public TextMeshProUGUI ManaEnemyText;

    private EnemyAI enemyAI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
            InitializeManagers();
            enemyAI = FindFirstObjectByType<EnemyAI>();

        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void InitializeManagers()
    {

        
        optionsManager = FindFirstObjectByType<OptionsManager>();
        if (optionsManager == null)
        {
            GameObject optionsManagerPrefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (optionsManagerPrefab != null)
            {
                Instantiate(optionsManagerPrefab,transform.position, Quaternion.identity,transform);
                optionsManager = GetComponentInChildren<OptionsManager>();
            }
            else
            {
                Debug.LogError("OptionsManager prefab not found in Resources.");
            }
        }   

        deckManager = FindFirstObjectByType<DeckManager>();
        if (deckManager == null)
        {
            GameObject deckManagerPrefab = Resources.Load<GameObject>("Prefabs/DeckManager");
            if (deckManagerPrefab != null)
            {
                Instantiate(deckManagerPrefab,transform.position, Quaternion.identity,transform);
                deckManager = GetComponentInChildren<DeckManager>();
            }
            else
            {
                Debug.LogError("DeckManager prefab not found in Resources.");
            }
        }

        audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab,transform.position, Quaternion.identity,transform);
                audioManager = GetComponentInChildren<AudioManager>();
            }
            else
            {
                Debug.LogError("AudioManager prefab not found in Resources.");
            }
        }
    }

    public int PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            playerHealth = Mathf.Max(0, value);
            UpdateUI();
            if (playerHealth <= 0)
            {
                LoseGame(true);
            }
        }
    }

    public int DifficultyLevel
    {
        get { return difficultyLevel; }
        set { difficultyLevel = Mathf.Max(1, value); } // Ensure difficulty level is at least 1
    }

    private int playerMana;
    private int playerManaLimit = 0;
    private int enemyMana;
    private int enemyManaLimit;
    private int enemyHealth = 20; // Add this field at the top with your other fields

    private enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }

    private TurnState turnState;



    public void StartPlayerTurn()
    {
        if (playerManaLimit < 10) playerManaLimit++;
        playerMana = playerManaLimit;
        UpdateUI(); // Update mana UI when player turn starts

        // Draw a card from the player's deck if available
        if (deckManager.playerCards.Count > 0 && deckManager.handManager.cardsInHand.Count < deckManager.handManager.maxHandSize)
        {
            int drawIndex = Random.Range(0, deckManager.playerCards.Count);
            Card drawnCard = deckManager.playerCards[drawIndex];
            deckManager.handManager.AddCardToHand(drawnCard);
            deckManager.playerCards.RemoveAt(drawIndex); // Remove to prevent duplicates
        }
        else
        {
            Debug.LogWarning("Player deck is empty! Player takes 1 damage.");
            PlayerHealth -= 1;
            UpdateUI(); // Update health UI after player takes damage 
        }

        Debug.Log("Player turn started. Mana: " + playerMana + "/" + playerManaLimit);
    }

    public void EndPlayerTurn()
    {
        ResolveCombat();
        turnState = TurnState.EnemyTurn;
        StartEnemyTurn();
    }

    private void EnemyPlayRandomCard()
    {
        var grid = FindFirstObjectByType<GridManager>();
        var hand = deckManager.enemyHandManager.cardsInHand;

        // Filter for cards the enemy can afford AND are Characters
        List<int> affordableIndexes = new List<int>();
        for (int i = 0; i < hand.Count; i++)
        {
            CardDisplay cd = hand[i].GetComponent<CardDisplay>();
            // Only allow cards that are Characters and affordable
            if (cd != null && cd.cardData is Character && cd.cardData.cost <= enemyMana)
                affordableIndexes.Add(i);
        }

        if (affordableIndexes.Count > 0)
        {
            // Pick a random affordable character card
            int randomAffordableIdx = affordableIndexes[Random.Range(0, affordableIndexes.Count)];
            GameObject cardObj = hand[randomAffordableIdx];
            CardDisplay cardDisplay = cardObj.GetComponent<CardDisplay>();
            Card cardData = cardDisplay.cardData;

            // Find all empty cells in enemy row (row 1)
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

                // Place the card (clone it so asset is not modified)
                Card cardInstance = cardData.Clone();
                grid.AddOccupantToGrid(cardInstance, new Vector2(chosenCol, 1));

                // Remove from hand visually and logically
                deckManager.enemyHandManager.cardsInHand.RemoveAt(randomAffordableIdx);
                Destroy(cardObj);

                // Subtract mana
                enemyMana -= cardData.cost;

                Debug.Log($"Enemy played {cardInstance.cardName} at column {chosenCol}, row 1");
            }
            else
            {
                Debug.Log("Enemy has no empty cells to play a card.");
            }
        }
        else
        {
            Debug.Log("Enemy has no character cards it can afford to play.");
        }
    }

    public void StartEnemyTurn()
    {
        if(enemyManaLimit < 10) enemyManaLimit++;
        enemyMana = enemyManaLimit;
        UpdateUI(); // Update mana UI when enemy turn starts

        if (enemyAI == null)
        enemyAI = FindFirstObjectByType<EnemyAI>();        

        // TODO: Add logic for enemy to use/draw this card
        enemyAI.ExecuteTurn();
        UpdateUI(); // Update UI after enemy AI logic
        //EnemyPlayRandomCard(); // AI logic to play a random card

        Debug.Log("Enemy turn started. Mana: " + enemyMana + "/" + enemyManaLimit);
        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        ResolveCombat();
        turnState = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    private void ResolveCombat()
    {
        var grid = FindFirstObjectByType<GridManager>();
        for (int x = 0; x < grid.width; x++)
        {
            var playerCell = grid.gridCells[x, 0].GetComponent<GridCell>();
            var enemyCell = grid.gridCells[x, 1].GetComponent<GridCell>();

            bool playerOccupied = playerCell.isOccupied && playerCell.occupant != null;
            bool enemyOccupied = enemyCell.isOccupied && enemyCell.occupant != null;

            var playerCardDisplay = playerOccupied ? playerCell.occupant.GetComponent<CardDisplay>() : null;
            var enemyCardDisplay = enemyOccupied ? enemyCell.occupant.GetComponent<CardDisplay>() : null;

            var playerCard = playerCardDisplay != null ? playerCardDisplay.cardData as Character : null;
            var enemyCard = enemyCardDisplay != null ? enemyCardDisplay.cardData as Character : null;

            if (playerCard != null && enemyCard != null)
            {
                // Both occupied: cards hit each other
                enemyCard.health -= playerCard.damage;
                playerCard.health -= enemyCard.damage;
                Debug.Log($"Combat at column {x}: Player {playerCard.cardName} hits Enemy {enemyCard.cardName}");
            }
            else if (playerCard != null && enemyCard == null)
            {
                Debug.Log($"Player card {playerCard.cardName} at column {x} hits ENEMY PLAYER for {playerCard.damage}");
                EnemyHealth -= playerCard.damage;
            }
            else if (enemyCard != null && playerCard == null)
            {
                Debug.Log($"Enemy card {enemyCard.cardName} at column {x} hits PLAYER for {enemyCard.damage}");
                PlayerHealth -= enemyCard.damage;
            }

            // Remove dead player card
            if (playerCard != null && playerCard.health <= 0)
            {
                Destroy(playerCell.occupant);
                playerCell.occupant = null;
                playerCell.isOccupied = false;
            }
            // Remove dead enemy card
            if (enemyCard != null && enemyCard.health <= 0)
            {
                Destroy(enemyCell.occupant);
                enemyCell.occupant = null;
                enemyCell.isOccupied = false;
            }
        }
        UpdateUI(); // Update health UI after combat resolution
    }

    void Start()
    {
        StartPlayerTurn();
    }

    public int PlayerMana
    {
        get => playerMana;
        set 
        {
            playerMana = value;
            UpdateUI();
        }
    }

    public int PlayerManaLimit
    {
        get => playerManaLimit;
        set 
        {
            playerManaLimit = value;
            UpdateUI();
        }
    }

    public int EnemyHealth
    {
        get { return enemyHealth; }
        set
        {
            enemyHealth = Mathf.Max(0, value);
            UpdateUI();
            if (enemyHealth <= 0)
            {
                LoseGame(false);
            }
        }
    }

    public int EnemyMana
    {
        get => enemyMana;
        set 
        {
            enemyMana = value;
            UpdateUI();
        }
    }

    public int EnemyManaLimit
    {
        get => enemyManaLimit;
        set 
        {
            enemyManaLimit = value;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (HPPlayerText != null)
            HPPlayerText.text = PlayerHealth.ToString();
        if (HPEnemyText != null)
            HPEnemyText.text = EnemyHealth.ToString();
        if (ManaPlayerText != null)
            ManaPlayerText.text = playerMana + "/" + playerManaLimit;
        if (ManaEnemyText != null)
            ManaEnemyText.text = enemyMana + "/" + enemyManaLimit;
    }

    private void LoseGame(bool isPlayer)
    {
        // TODO: Implement game over logic
        if (isPlayer)
        {
            Debug.Log("Player has lost the game.");
            // Show player lose UI, stop game, etc.
        }
        else
        {
            Debug.Log("Enemy has lost the game.");
            // Show enemy lose UI, stop game, etc.
        }
    }
}
