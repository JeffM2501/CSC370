using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attribute : Skill
{
    public static int MaxAttributeLevel = 20;
    public enum AttributeTypes
    {
        Might,
        Smarts,
        Agility,
    }

    public AttributeTypes AttributeType = AttributeTypes.Might;

    public Attribute()
    {
        MaxLevel = MaxAttributeLevel;
        Purchase = 0;
        Upgrade = 1000;
    }
}

public class AttributeInstance : SkillInstance
{
    public Attribute BaseAttribute = null;

    public AttributeInstance(Attribute s) : base(s)
    {
        BaseAttribute = s;
    }

    public AttributeInstance(Attribute s, int level)
        : base(s,level)
    {
        BaseAttribute = s;
        Level = level;
    }

}
