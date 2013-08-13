using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Potion : Item 
{
    public enum EffectedAttributes
    {
        Health,
        Mana,
        XP,
    }

    public EffectedAttributes Attribute = EffectedAttributes.Health;
    public int Amount = 0;

    public Potion(EffectedAttributes attribute, int amount)
    {
        Attribute = attribute;
        Amount = amount;
    }

    public override string ToString()
    {
        return base.ToString() + " +" + Amount.ToString() + " " + Attribute.ToString();
    }

    public override bool OnActivate(Character character)
    {
        base.OnActivate(character);
        switch (Attribute)
        {
            case EffectedAttributes.Health:
                if (character.Damage == 0)
                    return false;
                character.Heal(Amount);
                break;

            case EffectedAttributes.Mana:
                if (character.ManaSpent == 0)
                    return false;
                character.AddMana(Amount);
                break;


            case EffectedAttributes.XP:
                character.XP += Amount;
                break;
        }

        return true;
    }
}

public class Food : Item
{
    public int Amount = 0;

    public Food(int amount)
    {
        Amount = amount;
    }

    public override string ToString()
    {
        return base.ToString() + " +" + Amount.ToString() + " Health";
    }

    public override bool OnActivate(Character character)
    {
        base.OnActivate(character);
        character.Heal(Amount);
        return true;
    }
}
