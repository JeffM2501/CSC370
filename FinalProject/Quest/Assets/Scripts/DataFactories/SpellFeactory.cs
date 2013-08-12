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
                ).SpellEffect = CharacterObject.HitType.Fire;

        AddSpell("Fireball", "Creates a ball of fire on and around a target that does 5 points of damage per level",
                10, 5, 250, 500, true, 3,
                FireBall
                ).SpellEffect = CharacterObject.HitType.Fire;

        AddSpell("Shield", "Creates a ball of fire on and around a target that does 5 points of damage per level",
                10, 3, 100, 500, true, 4,
                Shield
                );

        AddSpell("Bless", "Adds 5% dodge to the target per level",
               2, 0, 0, 100, false, 1,
               Bless
               );

        AddSpell("Heal", "Adds 5% dodge to the target per level",
               3, 10, 100, 500, false, 2,
               Heal
               );

        AddSpell("Divine Light", "Does 6 points of damage to a target per level",
               4, 6, 100, 500, false, 4,
               DivineLight
               ).SpellEffect = CharacterObject.HitType.Divine;

        AddSpell("Ring of Protection", "Creates an area on the ground that provides +6 armor per level",
                6, 3, 100, 500, true, 3,
                Shield
                );
    }

    public static Spell FindSpellByName(string name)
    {
        if (Spells.ContainsKey(name))
            return Spells[name];

        Debug.Log("Spell not found " + name);
        return null;
    }

    static int MagicMissile(int damage, int level, Spell spell, Character caster)
    {
        // fire off spell effect
        return damage;
    }

    static int FireBall(int damage, int level, Spell spell, Character caster)
    {
        // fire off spell effect
        return damage;
    }

    static int Shield(int damage, int level, Spell spell, Character caster)
    {
        Character target = caster.Target;
        if (target == null)
            target = caster;

        target.AddBuff(Character.BuffTypes.Defense, damage, 30 * level);
        return 0;
    }

    static int Bless(int damage, int level, Spell spell, Character caster)
    {
        Character target = caster.Target;
        if (target == null)
            target = caster;

        target.AddBuff(Character.BuffTypes.Dodge, 0.05f * level, 15 * level);
        return 0;
    }

    static int Heal(int damage, int level, Spell spell, Character caster)
    {
        Character target = caster.Target;
        if (target == null)
            target = caster;

        target.Heal(damage);
        return 0;
    }

    static int DivineLight(int damage, int level, Spell spell, Character caster)
    {
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

        spell.AnimType = Skill.ActiveAnimationTypes.Casting;

        Spells.Add(spell.Name, spell);
        return spell;
    }
}
