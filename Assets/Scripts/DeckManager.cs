using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> playerCards = new List<Card>();
    public List<Card> enemyCards = new List<Card>();
    public int startingHandSize = 6;

    private int currentIndex = 0;
    public int maxHandSize = 9;
    public int currentHandSize;
    public HandManager handManager;

    public HandManager enemyHandManager;
    public DrawPileManager drawPileManager;
    public DrawPileManager enemyDrawPileManager;
    private bool startBattle = true;
    void Start()
    {
        //Load all card assets from the Resources folder
        playerCards.Clear();
        Card[] cards = Resources.LoadAll<Card>("Cards/Artifacts");
        playerCards.AddRange(cards);
        Debug.Log("Player loaded cards: " + cards.Length);

        enemyCards.Clear();
        string[] enemyFolders = new string[] { "Cards/Kobold", "Cards/Rogues", "Cards/Cursed", "Cards/Undead" };
        string chosenEnemyFolder = enemyFolders[Random.Range(0, enemyFolders.Length)];
        Card[] enemyCardsToken = Resources.LoadAll<Card>(chosenEnemyFolder);
        enemyCards.AddRange(enemyCardsToken);
        Debug.Log("Loaded enemy cards from " + chosenEnemyFolder + ": " + enemyCardsToken.Length);

    }

    void Awake()
    {
        if (drawPileManager == null)
            drawPileManager = FindFirstObjectByType<DrawPileManager>();

        // Find and assign enemyDrawPileManager
        DrawPileManager[] allDrawPiles = FindObjectsByType<DrawPileManager>(FindObjectsSortMode.None);
        foreach (var pile in allDrawPiles)
        {
            if (pile.isEnemy)
                enemyDrawPileManager = pile;
            else
                drawPileManager = pile;
        }

        HandManager[] allHands = FindObjectsByType<HandManager>(FindObjectsSortMode.None);
        foreach (var hand in allHands)
        {
            if (hand.gameObject.name == "HandManager")
                handManager = hand;
            else if (hand.gameObject.name == "EnemyHandManager")
                enemyHandManager = hand;
        }
    }
    void Update()
    {
        if (startBattle)
        {
            BattleSetup();
        }
    }
    public void BattleSetup()
    {
        handManager.BattleSetup(maxHandSize);
        enemyHandManager.BattleSetup(maxHandSize);

        drawPileManager.InitializeDrawPile(playerCards);
        enemyDrawPileManager.InitializeDrawPile(enemyCards);

        drawPileManager.BattleSetup(startingHandSize, maxHandSize, handManager);
        enemyDrawPileManager.BattleSetup(startingHandSize, maxHandSize, enemyHandManager);

        startBattle = false;

    }

}
