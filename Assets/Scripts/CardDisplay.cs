using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    //ALL card elements
    public Card cardData;

    public Image cardImage;

    public TMP_Text nameText;

    public TMP_Text costText;

    public TMP_Text descText;

    public GameObject characterCardElements;
    public GameObject spellCardElements;

    public GameObject characterCardLabel;
    public GameObject spellCardLabel;


    //Character card elements
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image[] typeImages;

    public Image damageImage;

    //Spell card elements
    public GameObject[] spellTypeLabels;
    public GameObject[] attributeTargetSymbols;

    public float attributeSymbolSpacing = 10f;

    public TMP_Text attributeValueText;
    private Color[] typeColors = {
        new Color(0.55f, 0.27f, 0.07f), // ITEM
        Color.cyan, // PERSON
        Color.green, // CREATURE
        Color.yellow, // LEADER
        new Color(1.0f, 0.27f, 0.0f), // SUNCURSED
        new Color(1.0f, 1.0f, 0.88f), // HOLY
        new Color(0.5f, 0.0f, 0.5f), // Undead
    };



    void Start()
    {
        UpdateCardDisplay();
    }

    void Update()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {


        //All card change
        cardImage.sprite = cardData.cardImage;
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        descText.text = cardData.description;

        //Update type
        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else
                typeImages[i].gameObject.SetActive(false);
        }

        //Specific card changes
        Debug.Log("Card Display --- Card Subtype: " + cardData.GetType().Name);
        if (cardData is Character characterData)
        {
            UpdateDisplayCharacter(cardData as Character);
        }
        else if (cardData is Spell spellData)
        {
            UpdateDisplaySpell(cardData as Spell);
        }
    }
    private void UpdateDisplayCharacter(Character characterData)
    {
        //spellCardElements.SetActive(false);
        //characterCardElements.SetActive(true);

        //spellCardLabel.SetActive(false);
        //characterCardLabel.SetActive(true);

        Debug.Log("Character Display --- Card Subtype: " + characterData.GetType().Name + " --- Character: " + characterData.cardName);
        //Update character specific elements
        healthText.text = characterData.health.ToString();
        damageText.text = characterData.damage.ToString();
        damageImage.color = typeColors[(int)characterData.damageType];

        Debug.Log("Character Display Finished");
    }

    private void UpdateDisplaySpell(Spell spellData)
    {
        //characterCardElements.SetActive(false);
        //spellCardElements.SetActive(true);

        //characterCardLabel.SetActive(false);
        //spellCardLabel.SetActive(true);

        Debug.Log("Spell Display --- Card Subtype: " + spellData.GetType().Name + " --- Spell: " + spellData.cardName);

        foreach (GameObject label in spellTypeLabels)
        {
            label.SetActive(false);
        }
        //spellTypeLabels[(int)spellData.spellType].SetActive(true);


        foreach (GameObject symbol in attributeTargetSymbols)
        {
            symbol.SetActive(false);
        }
        //Update spell specific elements
        for (int i = 0; i < spellData.attributeTargets.Count; i++)
        {
            //GameObject symbol = attributeTargetSymbols[(int)spellData.attributeTargets[i]];
            //symbol.SetActive(true);
            //float newYPos = i * attributeSymbolSpacing;
            //symbol.transform.localPosition = new Vector3(0, newYPos, 0);


            int changeAmount = spellData.attributeTargets[i] == Card.AttributeTarget.Health ? spellData.healthChange : spellData.damageChange;
            int finalChangeAmount = spellData.spellType == Card.SpellType.Buff ? changeAmount : -changeAmount;

            switch (spellData.attributeTargets[i])
            {
                case Spell.AttributeTarget.Health:
                    healthText.text = finalChangeAmount.ToString();
                    break;
                case Spell.AttributeTarget.Damage:
                    damageText.text = finalChangeAmount.ToString();
                    break;
            }
        }
        Debug.Log("Spell Display Finished --- Spell: " + spellData.cardName);
    }

}
