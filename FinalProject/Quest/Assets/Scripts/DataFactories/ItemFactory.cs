using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemFactory
{
    public static Dictionary<string, Equipment> Equipments = new Dictionary<string, Equipment>();

    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();

    public static Equipment Pants = null;

    public static Item FindItemByName(string name)
    {
        if (Equipments.ContainsKey(name))
            return Equipments[name];

        if (Items.ContainsKey(name))
            return Items[name];

        return null;
    }

    public static void Setup()
    {
        Equipments.Clear();
        Items.Clear();

        AddArmors();
        AddWeapons();
        AddItems();
    }

    public static Item RandomArmor()
    {
        List<Item> items = new List<Item>();
        foreach (Equipment e in Equipments.Values)
        {
            if (e as Armor != null)
                items.Add(e);
        }

        if (items.Count == 0)
            return null;

        return items[UnityEngine.Random.Range(0, items.Count - 1)];
    }

    public static Item RandomWeapon()
    {
        List<Item> items = new List<Item>();
        foreach (Equipment e in Equipments.Values)
        {
            if (e as Weapon != null)
                items.Add(e);
        }

        if (items.Count == 0)
            return null;

        return items[UnityEngine.Random.Range(0, items.Count - 1)];
    }

    public static Item RandomItem()
    {
        List<Item> items = new List<Item>();
        foreach (Item e in Items.Values)
        {
            items.Add(e);
        }

        if (items.Count == 0)
            return null;

        return items[UnityEngine.Random.Range(0, items.Count - 1)];
    }

    public static void AddItems()
    {
        AddItem("Watermelon", "Items/Icons/I_C_Watermellon", new Food(3));
        AddItem("Banana", "Items/Icons/I_C_Banana", new Food(4));
        AddItem("Cheese", "Items/Icons/I_C_Cheese", new Food(2));
        AddItem("Small Mana Potion", "Items/Icons/p_Blue02", new Potion(Potion.EffectedAttributes.Mana, 5));
        AddItem("Large Mana Potion", "Items/Icons/p_Blue01", new Potion(Potion.EffectedAttributes.Mana, 15));
        AddItem("Small Health Potion", "Items/Icons/p_Red02", new Potion(Potion.EffectedAttributes.Health, 5));
        AddItem("Large Health Potion", "Items/Icons/p_Red01", new Potion(Potion.EffectedAttributes.Health, 15));
        AddItem("Small XP Potion", "Items/Icons/P_Pink04", new Potion(Potion.EffectedAttributes.XP, 100));
        AddItem("Large XP Potion", "Items/Icons/P_Pink03", new Potion(Potion.EffectedAttributes.XP, 500));
        AddItem("XP Potion of Testing", "Items/Icons/P_Medicine09", new Potion(Potion.EffectedAttributes.XP, 5000));
    }

    public static void AddArmors()
    {
        AddArmor("Cloth Shirt", "Items/Icons/A_Clothing01", "Items/Armors/Materials/shirt_m", "Items/Armors/Materials/shirt_f", Equipment.EquipmentLocation.Torso, 1);
        AddArmor("Leather Armor", "Items/Icons/A_Armour01", "Items/Armors/Materials/leather_m", "Items/Armors/Materials/leather_f", Equipment.EquipmentLocation.Torso, 3);
        AddArmor("Chain Mail Armor", "Items/Icons/A_Armor04", "Items/Armors/Materials/chain_m", "Items/Armors/Materials/chain_f", Equipment.EquipmentLocation.Torso, 7);
        AddArmor("Plate Armor", "Items/Icons/A_Armour02", "Items/Armors/Materials/plate_m", "Items/Armors/Materials/plate_f", Equipment.EquipmentLocation.Torso, 12);

        AddArmor("Cloth Hood", "Items/Icons/C_Hat03", "Items/Armors/Materials/clothhood_m", "Items/Materials/Armors/clothhood_f", Equipment.EquipmentLocation.Head, 1);
        AddArmor("Leather Hat", "Items/Icons/C_Elm01", "Items/Armors/Materials/cap_m", "Items/Armors/Materials/cap_f", Equipment.EquipmentLocation.Head, 2);
        AddArmor("Skullcap", "Items/Icons/C_Elm02", "Items/Armors/Materials/skullcap_m", "Items/Armors/Materials/skullcap_f", Equipment.EquipmentLocation.Head, 3);
        AddArmor("Plate Helmet", "Items/Icons/C_Elm03", "Items/Armors/Materials/helm_m", "Items/Armors/Materials/helm_f", Equipment.EquipmentLocation.Head, 4);

        Pants = new Equipment();
        Pants.Name = "Pants";
        Pants.InventoryIcon = null;
        Pants.MaleEquipmentLayer = "Items/Armors/Materials/boots_m";
        Pants.FemaleEquipmentLayer = "Items/Armors/Materials/boots_f";
        Pants.Location = Equipment.EquipmentLocation.Unknown;
    }

    public static void AddWeapons()
    {
        AddWeapon("Sword", "Items/Icons/W_Sword002", Weapon.WeaponTypes.Sword, 5, 10);
        AddWeapon("Fancy Sword", "Items/Icons/W_Sword004", Weapon.WeaponTypes.Sword, 7, 15);
        AddWeapon("Flaming Sword of Fire", "Items/Icons/W_Sword016", Weapon.WeaponTypes.Sword, 15, 25);
        AddWeapon("Frigid Sword of Freezing", "Items/Icons/W_Sword018", Weapon.WeaponTypes.Sword, 15, 25);

        AddWeapon("Whacking Stick", "Items/Icons/W_Mace001", Weapon.WeaponTypes.Staff, 5, 10);
        AddWeapon("Spear", "Items/Icons/W_Spear001", Weapon.WeaponTypes.Staff, 7, 15);
        AddWeapon("Pokey Stick", "Items/Icons/W_Spear004", Weapon.WeaponTypes.Staff, 10, 20);
        AddWeapon("Staff of Baddassery", "Items/Icons/W_Spear007", Weapon.WeaponTypes.Staff, 15, 25);

        AddWeapon("Simple Bow", "Items/Icons/W_Bow01", Weapon.WeaponTypes.Bow, 3, 8);
        AddWeapon("Fancy Bow", "Items/Icons/W_Bow04", Weapon.WeaponTypes.Bow, 5, 10);
        AddWeapon("Sweet Recurved Bow", "Items/Icons/W_Bow08", Weapon.WeaponTypes.Bow, 7, 15);
        AddWeapon("The bow that will pierce the heavens", "Items/Icons/W_Bow11", Weapon.WeaponTypes.Bow, 10, 20);

        AddWeapon("Weighted Gloves", "Items/Icons/Ac_Gloves01", Weapon.WeaponTypes.Hand, 3, 8);
        AddWeapon("Steel Knuckles", "Items/Icons/W_Fist02", Weapon.WeaponTypes.Hand, 5, 10);
        AddWeapon("Fistblade", "Items/Icons/W_Fist04", Weapon.WeaponTypes.Hand, 7, 15);
    }

    public static Armor AddArmor(string name, string icon, string maleLayer, string femaleLayer, Equipment.EquipmentLocation location, int value )
    {
        Armor armor = new Armor();
        armor.Name = name;
        armor.InventoryIcon = Resources.Load(icon) as Texture;
        armor.MaleEquipmentLayer = maleLayer;
        armor.FemaleEquipmentLayer = femaleLayer;
        armor.Location = location;
        armor.ArmorValue = value;

        Equipments.Add(name, armor);

        return armor;
    }

    public static Weapon AddWeapon(string name, string icon, Weapon.WeaponTypes weaponType, int minValue, int maxValue)
    {
        Weapon weap = new Weapon();
        weap.Name = name;
        weap.InventoryIcon = Resources.Load(icon) as Texture;
      //  weap.MaleEquipmentLayer = Resources.Load(maleLayer) as Texture;
      //  weap.FemaleEquipmentLayer = Resources.Load(femaleLayer) as Texture;
        weap.Location = Equipment.EquipmentLocation.Weapon;
        weap.WeaponType = weaponType;
        weap.MinDamage = minValue;
        weap.MaxDamage = maxValue;

        Equipments.Add(name, weap);

        return weap;
    }

    public static Item AddItem(string name, string icon )
    {
        Item item = new Item();
        item.Name = name;
        item.InventoryIcon = Resources.Load(icon) as Texture;

        Items.Add(name, item);

        return item;
    }

    public static Item AddItem(string name, string icon, Item item)
    {
        item.Name = name;
        item.InventoryIcon = Resources.Load(icon) as Texture;

        Items.Add(name, item);

        return item;
    }
}
