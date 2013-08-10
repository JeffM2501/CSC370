using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Character
{
    public Movement PlayerMovemnt = null;

    public Player()
    {
        HairColor = Color.magenta;
    }

    public override void Init()
    {

        Attributes.Add(Attribute.AttributeTypes.Might, new AttributeInstance(SkillFactory.Might, 2));
        Attributes.Add(Attribute.AttributeTypes.Smarts, new AttributeInstance(SkillFactory.Smarts, 1));
        Attributes.Add(Attribute.AttributeTypes.Agility, new AttributeInstance(SkillFactory.Agility, 4));

        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 2));
        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Tough As Nails"), 1));
        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Cleave"), 2));
        Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Headshot"), 2));

        BaseLayer = this.Gender == Character.Genders.Female ? "Races/Materials/body_f" : "Races/Materials/body_m";
        HairLayer = this.Gender == Character.Genders.Female ? "Races/Hair/Materials/hair0_f" : "Races/Hair/Materials/hair0_m";
        EyeLayer = this.Gender == Character.Genders.Female ? "Races/Materials/eyes_f" : "Races/Materials/eyes_m";

        EquipItem(ItemFactory.FindItemByName("Cloth Shirt") as Equipment, Equipment.EquipmentLocation.Torso);
        EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);

        BackpackItem(ItemFactory.FindItemByName("Leather Hat"));

        BackpackItem(ItemFactory.FindItemByName("Small Health Potion"));
        BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        base.Init();
        Anims.SetSequence("Walk");
    }
}
