using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attribute : Skill
{
    public static int MaxLevel = 20;
    public enum AttributeTypes
    {
        Might,
        Smarts,
        Agility,
    }

    public AttributeTypes AttributeType = AttributeTypes.Might;
}
