using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;

    [TextArea]
    public string description;
    public int cost;
    public Sprite cardImage;

    public List<ElementType> cardType;


    public enum ElementType
    {

        Item,
        Person,
        Creature,
        Leader,
        SunCursed,
        Holy,
        Undead
    }

    public enum DamageType
    {
        Physical,
        Psychic,
        Holy,
        Fire,
        Cursed,
        Force,
        Poison
    }

    public enum SpellType
    {
        Buff,
        Debuff,
        Enchantment,
        //the rest is future work
        Summon,
        Ritual,
        Trap
    }

    public enum AttributeTarget
    {
        Health,
        Damage,
        DamageType
    }

    public virtual Card Clone()
    {
        return (Card)this.MemberwiseClone();
    }

}

