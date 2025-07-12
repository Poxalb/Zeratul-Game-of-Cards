using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DrawPileManager : MonoBehaviour
{

    public List<Card> drawPile = new List<Card>();

    public int startingHandSize = 6;

    private int currentIndex = 0;
    public int maxHandSize;
    public int currentHandSize;
    private HandManager handManager;

    private DiscardManager discardManager;

    public TextMeshProUGUI drawPileText;

    public bool isEnemy = false; // Set this in the Inspector for the enemy's DrawPileManager

    void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void InitializeDrawPile(List<Card> deck)
    {
        drawPile.Clear();
        drawPile.AddRange(deck);
        Utility.Shuffle(drawPile);
        if (!isEnemy) UpdateDrawPileText();
    }

    public void BattleSetup(int numberOfCardsToDraw, int setMaxHandSize = 6)
    {
        maxHandSize = setMaxHandSize;
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            RefillDeckFromDiscard();
            return;
        }

        if (currentHandSize <= maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);
            if (drawPile.Count > 0) currentIndex %= drawPile.Count;
            //Debug.Log("Card Subtype: " + nextCard.GetType().Name);
            if (!isEnemy) UpdateDrawPileText();

            
        }
    }

    private void UpdateDrawPileText()
    {
        if (drawPileText != null)
        {
            drawPileText.text = drawPile.Count.ToString();
        }
        else
        {
            Debug.LogWarning("Draw Pile Text is not assigned.");
        }
    }

    private void RefillDeckFromDiscard()
    {
        if (discardManager == null)
        {
            discardManager = FindFirstObjectByType<DiscardManager>();
        }

        if (discardManager != null && discardManager.discardedCards.Count > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utility.Shuffle(drawPile);
            currentIndex = 0;
            if (!isEnemy) UpdateDrawPileText();
        }
        else
        {
            Debug.LogWarning("No cards in discard to refill draw pile.");
        }
    }
}
