using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DiscardManager : MonoBehaviour
{
    [SerializeField] public List<Card> discardedCards = new List<Card>();

    public TextMeshProUGUI discardText;

    public int discardCardCount;

    void Awake()
    {
        UpdateDiscardText();
    }

    private void UpdateDiscardText()
    {
        discardText.text = discardedCards.Count.ToString();
        discardCardCount = discardedCards.Count;
    }

    public void AddCardToDiscard(Card card)
    {
        if (card != null)
        {
            discardedCards.Add(card);
            UpdateDiscardText();
            Debug.Log("Card added to discard: " + card.name);
        }
        else
        {
            Debug.LogError("Attempted to add a null card to discard.");
        }
    }


    public Card PullFromDiscard()
    {
        if (discardedCards.Count > 0)
        {
            Card card = discardedCards[discardedCards.Count - 1];
            discardedCards.RemoveAt(discardedCards.Count - 1);
            UpdateDiscardText();
            Debug.Log("Card pulled from discard: " + card.name);
            return card;
        }
        else
        {
            Debug.LogWarning("No cards in discard to pull.");
            return null;
        }
    }

    public bool PullSelectedCardFromDiscard(Card card)
    {
        if (discardedCards.Count > 0 && discardedCards.Contains(card))
        {
            discardedCards.Remove(card);
            UpdateDiscardText();
            Debug.Log("Selected card pulled from discard: " + card.name);
            return true;
        }
        else
        {
            Debug.LogWarning("Selected card not found in discard: " + card.name);
            return false;
        }
    }

    public List<Card> PullAllFromDiscard()
    {
        if (discardedCards.Count <= 0)
        {
            Debug.LogWarning("No cards in discard to pull.");
            return new List<Card>();
        }
        List<Card> pulledCards = new List<Card>(discardedCards);
        discardedCards.Clear();
        UpdateDiscardText();
        Debug.Log("All cards pulled from discard.");
        return pulledCards;
    }
}
