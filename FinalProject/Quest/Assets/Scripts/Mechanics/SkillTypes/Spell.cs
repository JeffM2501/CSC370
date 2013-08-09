using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Spell : Skill 
{
    public int MagicCost = 0;

    public int Damage = 0;

    public delegate int CastSpellCallback(int damage, int level, Spell spell);

    public CastSpellCallback CastSpell;

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

        int Damage = BaseSpell.Damage * Level;

        if (BaseSpell.CastSpell != null)
            BaseSpell.CastSpell(Damage, Level, BaseSpell);

        // tell the combat system to damage the target
    }
}
