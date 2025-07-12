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

        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform); //instancia el prefab en la mano
        cardsInHand.Add(newCard); //agrega la carta a la lista de cartas en la mano
        //Debug.Log("Card Subtype: " + cardData.GetType().Name);
        newCard.GetComponent<CardDisplay>().cardData = cardData; //asigna los datos de la carta al prefab
        UpdateCardPositions(); //actualiza la posicion de las cartas
    }

    
    void Update()
    {
        UpdateCardPositions(); //actualiza la posicion de las cartas
    }
    
    public void BattleSetup(int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize; //asigna el maximo de cartas en la mano
        
    }
    public void UpdateCardPositions()
    {

        int cardCount = cardsInHand.Count; //cuenta las cartas en la mano

        if (cardCount == 1) //si hay una carta en la mano la coloca en el centro
        {
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardCount; i++)
        {
            float angleRotation = (i - (cardCount - 1) / 2f) * fanSpread; //calcula el angulo de la 
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, angleRotation); //coloca la carta en el centro de la mano

            float hOffset = (i - (cardCount - 1) / 2f) * cardDistance; //calcula la distancia entre cartas

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //normaliza la posicion de la carta
            float vOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition); //calcula la altura de las cartas normalizadas
            cardsInHand[i].transform.localPosition = new Vector3(hOffset, vOffset, 0f); //coloca la carta en la posicion correcta


        }

    }
}
