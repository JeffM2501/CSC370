﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Character
{
    public Movement PlayerMovemnt = null;

    public int GoldCount = 0;

    public Player()
    {
        HairColor = Color.magenta;

       
    }

    public override void Init()
    {
        Attributes.Add(Attribute.AttributeTypes.Might, new AttributeInstance(SkillFactory.Might, 2));
        Attributes.Add(Attribute.AttributeTypes.Smarts, new AttributeInstance(SkillFactory.Smarts, 1));
        Attributes.Add(Attribute.AttributeTypes.Agility, new AttributeInstance(SkillFactory.Agility, 2));

        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 2));
        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Tough As Nails"), 1));
        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Cleave"), 2));

        EquipItem(ItemFactory.FindItemByName("Cloth Shirt"), Equipment.EquipmentLocation.Torso);
        EquipItem(ItemFactory.FindItemByName("Sword"), Equipment.EquipmentLocation.Weapon);

        Debug.Log(EquipedItems);
    }
}
