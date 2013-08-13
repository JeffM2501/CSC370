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

        if (UnityEngine.Random.value > 0.5)
            orc.MaleLayers.Add("TempSprites/Materials/ork");
        else
            orc.MaleLayers.Add("TempSprites/Materials/orc2");

        orc.FemaleLayers = orc.MaleLayers;
        SetStats(orc, 3, 1, 2,25);

        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 1));
        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Tough As Nails"), 2));
        orc.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Cleave"), 2));

        orc.InventoryItems.GoldCoins = (int)UnityEngine.Random.Range(10,20);

        orc.EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);
        orc.EquipItem(ItemFactory.FindItemByName("Leather Armor") as Equipment, Equipment.EquipmentLocation.Torso);
        orc.BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        orc.Smarts = new FightOrFlight();

        return AttachToGame(orc, location,1.45f);
    }

    public static Character NewBandit(Vector3 location)
    {
        Monster mon = new Monster();
        mon.Name = "Bandit";

        if (UnityEngine.Random.value > 0.125)
            mon.MaleLayers.Add("TempSprites/Materials/bandit");
        else
        {
            mon.MaleLayers.Add("TempSprites/Materials/bandit_cheif");
            mon.Name = "Fancy Bandit";
        }

        mon.FemaleLayers = mon.MaleLayers;
        SetStats(mon, 2, 2, 2, 25);

        mon.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), 3));

        mon.InventoryItems.GoldCoins = (int)UnityEngine.Random.Range(20, 120);

        mon.EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);
        mon.EquipItem(ItemFactory.FindItemByName("Leather Armor") as Equipment, Equipment.EquipmentLocation.Torso);
        mon.BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        mon.Smarts = new FightOrFlight(0.3f,0.5f);

        return AttachToGame(mon, location,1.25f);
    }

    public static Character NewSkellymans(Vector3 location)
    {
        Monster mon = new Monster();
        mon.Name = "Skellyman";

        bool isFancy = UnityEngine.Random.value < 0.25;
        if (!isFancy)
            mon.MaleLayers.Add("TempSprites/Materials/skeleton");
        else
        {
            mon.MaleLayers.Add("TempSprites/Materials/skeleton_chain");
            mon.Name = "Dire " + mon.Name;
        }

        mon.FemaleLayers = mon.MaleLayers;
        if (isFancy)
            SetStats(mon, 2, 1, 1, 25);
        else
            SetStats(mon, 3, 0, 2, 50);

        mon.Skills.Add(new SkillInstance(SkillFactory.FindSkillByName("Swords"), isFancy ? 4 : 2));

        mon.InventoryItems.GoldCoins = 0;

        mon.EquipItem(ItemFactory.FindItemByName("Sword") as Equipment, Equipment.EquipmentLocation.Weapon);
        mon.EquipItem(ItemFactory.FindItemByName( isFancy ? " Mail Armor" : "Leather Armor") as Equipment, Equipment.EquipmentLocation.Torso);
       // mon.BackpackItem(ItemFactory.FindItemByName("Watermelon"));

        mon.Smarts = new FightToDeath(0.75f);

        return AttachToGame(mon, location,1.25f);
    }


    public static void SetStats(Character c, int might, int smarts, int agiligy, int xp)
    {
        c.Attributes.Add(Attribute.AttributeTypes.Might, new AttributeInstance(SkillFactory.Might, might));
        c.Attributes.Add(Attribute.AttributeTypes.Smarts, new AttributeInstance(SkillFactory.Smarts, smarts));
        c.Attributes.Add(Attribute.AttributeTypes.Agility, new AttributeInstance(SkillFactory.Agility, agiligy));
        c.XP = xp;
    }

    public static Character AttachToGame(Character c, Vector3 loc, float scale)
    {
        GameState.Instance.ActiveCharacters.Add(c);
        c.WorldObject = MonoBehaviour.Instantiate(GameState.Instance.InputMan.CharacterPrefab) as GameObject;
        c.WorldObject.transform.position = new Vector3(loc.x,loc.y,loc.z);
        c.WorldObject.transform.localScale = new Vector3(scale, scale, scale);
        c.Init();

        return c;
    }
}
