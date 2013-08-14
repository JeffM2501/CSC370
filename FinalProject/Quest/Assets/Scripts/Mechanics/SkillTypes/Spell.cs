﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Spell : Skill 
{
    public bool Arcane = false;

    public CharacterObject.HitType SpellEffect = CharacterObject.HitType.Fire;

    public int MagicCost = 0;

    public int Damage = 0;

    public delegate int CastSpellCallback(int damage, int level, Spell spell, Character caster);

    public CastSpellCallback CastSpell;

    public Spell(bool arcane)
    {
        SkillType = Skill.SkillTypes.Spell;
        Arcane = arcane;
    }

    public override bool Useable(Character character)
    {
        return base.Useable(character);
    }
}

public class SpellInstance : SkillInstance
{
    public Spell BaseSpell = null;

    public SpellInstance(Spell spell)
        : base(spell as Skill)
    {
        BaseSpell = BaseSkill as Spell;
    }

    public SpellInstance(Spell spell, int level)
        : base(spell, level)
    {
        BaseSpell = BaseSkill as Spell;
    }

    public override void OnAcivate(Character character)
    {
        base.OnAcivate(character);

        character.ManaSpent += BaseSpell.MagicCost;

        int damage = BaseSpell.Damage * Level * (BaseSpell.Arcane ? character.ArcaneSkill : character.DivineSkill);

        if (BaseSpell.CastSpell != null)
            damage = BaseSpell.CastSpell(damage, Level, BaseSpell, character);

        if (damage > 0)
            GameState.Instance.BattleMan.SpellAttack(character, character.Target, BaseSpell.SpellEffect, BaseSpell.Range, damage);
    }
}
