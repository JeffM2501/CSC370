using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Potion : Item 
{
    public bool Health = true;
    public int Amount = 0;

    public Potion(bool health, int amount)
    {
        Health = health;
        Amount = amount;
    }

    public override bool OnActivate(Character character)
    {
        base.OnActivate(character);
        if (Health)
        {
            if (character.Damage == 0)
                return false;
            character.Heal(Amount);
        }
        else
        {
            if (character.ManaSpent == 0)
                return false;
            character.AddMana(Amount);
        }

        return true;
    }
}
