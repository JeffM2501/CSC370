using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Skill
{
    public string Name = string.Empty;
    
    public enum SkillTypes
    {
        Attribute,
        Weapon,
        Spell,
    }

    public SkillTypes SkillType = SkillTypes.Attribute;
    public bool Active = false;
    public float Cooldown = 0;

    public int Level = 0;
}
