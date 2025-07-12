using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "New Character", menuName = "Card/Character")]
public class Character : Card
{


    
    public int damage;
    public int health;
    public DamageType damageType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Card Clone()
    {
        return (Character)base.Clone();
    }
}
