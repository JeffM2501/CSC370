using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemFactory
{
    public static Dictionary<string, Equipment> Equipments = new Dictionary<string, Equipment>();

    public static Equipment Pants = null;

    public static Equipment FindItemByName(string name)
    {
        Debug.Log(Equipments);

        if (Equipments.ContainsKey(name))
            return Equipments[name];

        return null;
    }

    public static void Setup()
    {
        Armors();
        Weapons();
    }

    public static void Armors()
    {
        AddArmor("Cloth Shirt", "Items/Icons/A_Clothing01","Items/Armors/shirt_m","Items/Armors/shirt_f",Equipment.EquipmentLocation.Torso, 1);
        AddArmor("Leather Armor", "Items/Icons/A_Armor01", "Items/Armors/leather_m", "Items/Armors/leather_f", Equipment.EquipmentLocation.Torso, 3);
        AddArmor("Chain Mail Armor", "Items/Icons/A_Armor04", "Items/Armors/chain_m", "Items/Armors/chain_f", Equipment.EquipmentLocation.Torso, 7);
        AddArmor("Plate Armor", "Items/Icons/A_Armor02", "Items/Armors/plate_m", "Items/Armors/plate_f", Equipment.EquipmentLocation.Torso, 12);

        AddArmor("Cloth Hood", "Items/Icons/C_Hat03", "Items/Armors/clothhood_m", "Items/Armors/clothhood_f", Equipment.EquipmentLocation.Head, 1);
        AddArmor("Leather Hat", "Items/Icons/C_Elm01", "Items/Armors/cap_m", "Items/Armors/cap_f", Equipment.EquipmentLocation.Head, 2);
        AddArmor("Skullcap", "Items/Icons/C_Elm02", "Items/Armors/skullcap_m", "Items/Armors/skullcap_f", Equipment.EquipmentLocation.Head, 3);
        AddArmor("Plate Helmet", "Items/Icons/C_Elm03", "Items/Armors/helm_m", "Items/Armors/helm_f", Equipment.EquipmentLocation.Head, 4);

        Pants = new Equipment();
        Pants.Name = "Pants";
        Pants.InventoryIcon = null;
        Pants.MaleEquipmentLayer = Resources.Load("Items/Armors/boots_m") as Texture;
        Pants.FemaleEquipmentLayer = Resources.Load("Items/Armors/boots_f") as Texture;
        Pants.Location = Equipment.EquipmentLocation.Unknown;
    }

    public static void Weapons()
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

        AddWeapon("Weighted Gloves", "Items/Icons/Ac_Gloves01", Weapon.WeaponTypes.Bow, 3, 8);
        AddWeapon("Steel Knuckles", "Items/Icons/W_Fist02", Weapon.WeaponTypes.Bow, 5, 10);
        AddWeapon("Fistblade", "Items/Icons/W_Fist04", Weapon.WeaponTypes.Bow, 7, 15);
    }

    public static Armor AddArmor(string name, string icon, string maleLayer, string femaleLayer, Equipment.EquipmentLocation location, int value )
    {
        Armor armor = new Armor();
        armor.Name = name;
        armor.InventoryIcon = Resources.Load(icon) as Texture;
        armor.MaleEquipmentLayer = Resources.Load(maleLayer) as Texture;
        armor.FemaleEquipmentLayer = Resources.Load(femaleLayer) as Texture;
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
}
