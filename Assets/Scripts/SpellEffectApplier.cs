using UnityEngine;

public class SpellEffectApplier
{

    //Already done if i want to create more complex spells
    private static void ApplyAttributeChange(Spell spell, Spell.SpellType spellType, Spell.AttributeTarget attributeTarget, int changeAmount, Character targetCard)
    {
        int finalChangeAmount = spellType == Card.SpellType.Buff ? changeAmount : -changeAmount;
        switch (attributeTarget)
        {
            case Spell.AttributeTarget.Health:
                targetCard.health += finalChangeAmount;
                break;
            case Spell.AttributeTarget.Damage:
                targetCard.damage += finalChangeAmount;
                break;
            case Spell.AttributeTarget.DamageType:
                targetCard.damageType = spell.cardElementChangeTo;
                break;
            default:
                Debug.LogWarning("Unknown attribute target: " + attributeTarget);
                break;
        }

    }

    //can be upgraded to handle a list of target cards
    public static void ApplySpellEffect(Spell spell, Card targetCards)
    {
        for (int i = 0; i < spell.attributeTargets.Count; i++)
        {
            Spell.AttributeTarget attributeTarget = spell.attributeTargets[i];
            int changeAmount = attributeTarget == Card.AttributeTarget.Health ? spell.healthChange : spell.damageChange;

            // Apply the attribute change
            ApplyAttributeChange(spell, spell.spellType, attributeTarget, changeAmount, (Character)targetCards);
        }
    }

    /* before
    public static void ApplySpellEffect(Spell spell, Card targetCard)
    {
        // Apply health change if not null
        if (spell.HealthChange != null)
        {
            targetCard.health += spell.spellType == Spell.SpellType.Buff ? spell.HealthChange.Value : -spell.HealthChange.Value;
        }

        // Apply damage change if not null
        if (spell.DamageChange != null)
        {
            targetCard.damage += spell.spellType == Spell.SpellType.Buff ? spell.DamageChange.Value : -spell.DamageChange.Value;
        }

        // Apply element type change if not null
        if (spell.cardElementChangeTo != null)
        {
            targetCard.DamageType = spell.cardElementChangeTo.Value;
        }
    }
    */
}
