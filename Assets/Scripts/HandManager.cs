using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab; //asgina el prefab en el inspector
    public Transform handTransform; //para el hand position

    public float fanSpread = 2.5f; //rotacion entre cartas
    public float cardDistance = -100f; //distancia entre cartas
    public int maxHandSize;
    public float verticalSpacing = 120f; 
    public List<GameObject> cardsInHand = new List<GameObject>(); //lista de objetos carta en la mano

    void Start()
    {
    }

    public void AddCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        
        // Force immediate positioning update
        StartCoroutine(UpdatePositionsNextFrame());
    }

    private IEnumerator UpdatePositionsNextFrame()
    {
        yield return null; // Wait one frame
        UpdateCardPositions();
    }
    
    void Update()
    {
        // Clean up null references but don't constantly reposition
        CleanUpNullCards();
    }
    
    private void CleanUpNullCards()
    {
        cardsInHand.RemoveAll(card => card == null);
    }
    
    public void BattleSetup(int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
    }
    
    public void UpdateCardPositions()
    {
        // Clean up null references first
        CleanUpNullCards();
        
        int cardCount = cardsInHand.Count;
        
        if (cardCount == 0) return;

        if (cardCount == 1)
        {
            if (cardsInHand[0] != null)
            {
                cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
                cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            return;
        }
        
        for (int i = 0; i < cardCount; i++)
        {
            if (cardsInHand[i] == null) continue; // Skip null cards
            
            float angleRotation = (i - (cardCount - 1) / 2f) * fanSpread;
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, angleRotation);

            float hOffset = (i - (cardCount - 1) / 2f) * cardDistance;
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float vOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);
            cardsInHand[i].transform.localPosition = new Vector3(hOffset, vOffset, 0f);
        }
    }
    
    // Call this method when a card is removed from hand
    public void OnCardRemoved()
    {
        StartCoroutine(UpdatePositionsNextFrame());
    }
}
