using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MonsterFactory
{

    public static Character NewOrc( Vector3 location )
    {
        Monster orc = new Monster();
        orc.Name = "Orc";

        if (GameState.Instance.Rand.NextDouble() > 0.5)
            orc.MaleLayers.Add("TempSprites/Materials/body_o_base");
        else
            orc.MaleLayers.Add("TempSprites/Materials/body_o_leather");

        orc.FemaleLayers = orc.MaleLayers;
        SetStats(orc, 3, 1, 2,25);

        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 1));
        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Tough As Nails"), 2));
        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Cleave"), 2));

        orc.InventoryItems.GoldCoins = 10 + GameState.Instance.Rand.Next(10);

        orc.EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);
        orc.BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        return AttachToGame(orc, location);
    }

    public static Character NewBandit(Vector3 location)
    {
        Monster mon = new Monster();
        mon.Name = "Bandit";

        if (GameState.Instance.Rand.NextDouble() > 0.125)
            mon.MaleLayers.Add("TempSprites/Materials/bandit");
        else
        {
            mon.MaleLayers.Add("TempSprites/Materials/bandit_cheif");
            mon.Name = "Fancy Bandit";
        }

        mon.FemaleLayers = mon.MaleLayers;
        SetStats(mon, 3, 1, 2, 25);

        mon.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 3));

        mon.InventoryItems.GoldCoins = 20 + GameState.Instance.Rand.Next(100);

        mon.EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);
        EquipItem(ItemFactory.FindItemByName("Leather Armor") as Equipment, Equipment.EquipmentLocation.Torso);
        mon.BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        return AttachToGame(mon, location);
    }

    public static void SetStats(Character c, int might, int smarts, int agiligy, int xp)
    {
        c.Attributes.Add(Attribute.AttributeTypes.Might, new AttributeInstance(SkillFactory.Might, might));
        c.Attributes.Add(Attribute.AttributeTypes.Smarts, new AttributeInstance(SkillFactory.Smarts, smarts));
        c.Attributes.Add(Attribute.AttributeTypes.Agility, new AttributeInstance(SkillFactory.Agility, agiligy));
        c.XP = xp;
    }

    public static Character AttachToGame(Character c, Vector3 loc)
    {
        GameState.Instance.ActiveCharacters.Add(c);
        c.WorldObject = MonoBehaviour.Instantiate(GameState.Instance.InputMan.CharacterPrefab) as GameObject;
        c.WorldObject.transform.position = new Vector3(loc.x,loc.y,loc.z);

        c.Init();

        return c;
    }
}
