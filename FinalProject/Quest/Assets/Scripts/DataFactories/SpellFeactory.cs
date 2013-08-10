using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpellFeactory
{
    public static Dictionary<string, Spell> Spells = new Dictionary<string, Spell>();

    public static void Setup()
    {
        AddSpell("Magic Missile", "Fires off a magical energy blast that does 1 damage per level",
                5, 1, 0, 100, true, 1,
                MagicMissile
                ).MaxLevel = 10;

        AddSpell("Fire burst", "Creates a beam of fire that does 3 damage per level to a target",
                5, 3, 100, 500, true, 2,
                FireBall
                );

        AddSpell("Fireball", "Creates a ball of fire on and around a target that does 5 points of damage per level",
                10, 3, 250, 500, true, 3,
                FireBall
                );

        AddSpell("Shield", "Creates a ball of fire on and around a target that does 5 points of damage per level",
                10, 3, 250, 500, true, 3,
                Shield
                );
    }

    static int MagicMissile(int damage, int level, Spell spell)
    {
        // fire off spell effect
        return damage;
    }

    static int FireBall(int damage, int level, Spell spell)
    {
        // fire off spell effect
        return damage;
    }

    static int Shield(int damage, int level, Spell spell)
    {
        // fire off spell effect
        return damage;
    }

    protected static Spell AddSpell(   string name, string description, int mana, 
                                int damage, int purchase, int upgrade,
                                bool arcane, int requirement,
                                Spell.CastSpellCallback effector)
    {
        Spell spell = new Spell();
        spell.Name = name;
        spell.Description = description;
        spell.MagicCost = mana;
        spell.Damage = damage;
        spell.CastSpell = effector;
        spell.Purchase = purchase;
        spell.Upgrade = upgrade;
        spell.MaxLevel = 5;
        spell.Requirements.Add((arcane ? "Arcane" : "Divine") + requirement.ToString());

        Spells.Add(spell.Name, spell);
        return spell;
    }
}
