using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Skill
{
    public string Name = string.Empty;
    public string IconImage = string.Empty;

    public string Description = string.Empty;
    
    public enum SkillTypes
    {
        Attribute,
        Passive,
        Active,
        MagicArcane,
        MagicDivine,
        Weapon,
        Spell,
    }

    public SkillTypes SkillType = SkillTypes.Attribute;
    public bool Active = false;
    public float Cooldown = 0;

    public int MaxLevel = 0;

    public int Purchase = 0;
    public int Upgrade = 0;
    public List<string> Requirements = new List<String>();

    public List<string> AdditionalSkillsGranted = new List<string>();

    public virtual bool CharacterHasRequirements(Character character)
    {
        foreach (string requirement in Requirements)
        {
            string[] parts = requirement.Split(" ".ToCharArray());
            int level = 1;
            if (parts.Length > 1)
                int.TryParse(parts[1], out level);

            string stat = parts[0];

            if (stat.Contains("Might") || stat.Contains("Smarts") || stat.Contains("Agility"))
            {
                try
                {
                    Attribute.AttributeTypes att = (Attribute.AttributeTypes)Enum.Parse(typeof(Attribute.AttributeTypes), parts[0]);

                    if (character.Attributes[att].Level < level)
                        return false;
                }
                catch (System.Exception /*ex*/)
                {
                }
            }
            else
            {
                SkillInstance skill = character.GetSkillByName(stat);
                if (skill == null || skill.Level < level)
                    return false;
            }
        }

        return true;
    }

    // called when ever we need to modify the character
    public virtual void OnApply(Character character, int level)
    {

    }

    public virtual void OnAcivate(Character character, int level)
    {

    }

    public virtual void OnUpgrade(Character character, int level)
    {

    }
}

public class SkillInstance
{
    public Skill BaseSkill = null;

    public int Level = 0;

    public float LastUse = -99999;

    public SkillInstance(Skill s)
    {
        BaseSkill = s;
    }

    public SkillInstance(Skill s, int level)
    {
        BaseSkill = s;
        Level = level;
    }

    // called when ever we need to modify the character
    public void OnApply(Character character)
    {
        BaseSkill.OnApply(character, Level);
    }

    public void OnAcivate(Character character)
    {
        BaseSkill.OnAcivate(character, Level);
    }

    public void OnUpgrade(Character character)
    {
        BaseSkill.OnUpgrade(character, Level);
    }
}
