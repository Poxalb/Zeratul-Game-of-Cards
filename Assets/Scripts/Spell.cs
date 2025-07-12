using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "New Spell", menuName = "Card/Spell")]
public class Spell : Card
{
    public SpellType spellType;

    public List<AttributeTarget> attributeTargets = new List<AttributeTarget>();
    public int healthChange;
    public int damageChange;
    public DamageType cardElementChangeTo;

    public override Card Clone()
    {
        Spell clone = (Spell)base.Clone();
        clone.attributeTargets = new List<AttributeTarget>(this.attributeTargets); // Deep copy
        return clone;
    }
}
