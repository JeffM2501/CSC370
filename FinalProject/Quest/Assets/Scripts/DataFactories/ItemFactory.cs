using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemFactory
{
    public static Dictionary<string, Equipment> Equipments = new Dictionary<string, Equipment>();

    public static void Setup()
    {
        Armors();
        Weapons();
    }

    public static void Armors()
    {
        AddArmor("Cloth Shirt", "GUI/Items/Icons/A_Clothing01","GUI/Items/Armors/shirt_m","GUI/Items/Armors/shirt_f",Equipment.EquipmentLocation.Torso, 1);
        AddArmor("Leather Armor", "GUI/Items/Icons/A_Armor01", "GUI/Items/Armors/leather_m", "GUI/Items/Armors/leather_f", Equipment.EquipmentLocation.Torso, 3);
        AddArmor("Chain Mail Armor", "GUI/Items/Icons/A_Armor04", "GUI/Items/Armors/chain_m", "GUI/Items/Armors/chain_f", Equipment.EquipmentLocation.Torso, 7);
        AddArmor("Plate Armor", "GUI/Items/Icons/A_Armor02", "GUI/Items/Armors/plate_m", "GUI/Items/Armors/plate_f", Equipment.EquipmentLocation.Torso, 12);

        AddArmor("Cloth Hood", "GUI/Items/Icons/C_Hat03", "GUI/Items/Armors/clothhood_m", "GUI/Items/Armors/clothhood_f", Equipment.EquipmentLocation.Head, 1);
        AddArmor("Leather Hat", "GUI/Items/Icons/C_Elm01", "GUI/Items/Armors/cap_m", "GUI/Items/Armors/cap_f", Equipment.EquipmentLocation.Head, 2);
        AddArmor("Skullcap", "GUI/Items/Icons/C_Elm02", "GUI/Items/Armors/skullcap_m", "GUI/Items/Armors/skullcap_f", Equipment.EquipmentLocation.Head, 3);
        AddArmor("Plate Helmet", "GUI/Items/Icons/C_Elm03", "GUI/Items/Armors/helm_m", "GUI/Items/Armors/helm_f", Equipment.EquipmentLocation.Head, 4);
    }

    public static void Weapons()
    {
        AddWeapon("Sword", "GUI/Items/Icons/W_Sword002", Weapon.WeaponTypes.Sword, 5, 10);
        AddWeapon("Fancy Sword", "GUI/Items/Icons/W_Sword004", Weapon.WeaponTypes.Sword, 7, 15);
        AddWeapon("Flaming Sword of Fire", "GUI/Items/Icons/W_Sword016", Weapon.WeaponTypes.Sword, 15, 25);
        AddWeapon("Frigid Sword of Freezing", "GUI/Items/Icons/W_Sword018", Weapon.WeaponTypes.Sword, 15, 25);

        AddWeapon("Whacking Stick", "GUI/Items/Icons/W_Mace001", Weapon.WeaponTypes.Staff, 5, 10);
        AddWeapon("Spear", "GUI/Items/Icons/W_Spear001", Weapon.WeaponTypes.Staff, 7, 15);
        AddWeapon("Pokey Stick", "GUI/Items/Icons/W_Spear004", Weapon.WeaponTypes.Staff, 15, 25);
        AddWeapon("Staff of Baddassery", "GUI/Items/Icons/W_Spear007", Weapon.WeaponTypes.Staff, 15, 25);

        AddWeapon("Simple Bow", "GUI/Items/Icons/W_Bow01", Weapon.WeaponTypes.Bow, 3, 8);
        AddWeapon("Fancy Bow", "GUI/Items/Icons/W_Bow04", Weapon.WeaponTypes.Bow, 5, 10);
        AddWeapon("Sweet Recurved Bow", "GUI/Items/Icons/W_Bow08", Weapon.WeaponTypes.Bow, 7, 15);
        AddWeapon("The bow that will pierce the heavens", "GUI/Items/Icons/W_Bow11", Weapon.WeaponTypes.Bow, 10, 20);

        AddWeapon("Weighted Gloves", "GUI/Items/Icons/Ac_Gloves01", Weapon.WeaponTypes.Bow, 3, 8);
        AddWeapon("Steel Knuckles", "GUI/Items/Icons/W_Fist02", Weapon.WeaponTypes.Bow, 5, 10);
        AddWeapon("Fistblade", "GUI/Items/Icons/W_Fist04", Weapon.WeaponTypes.Bow, 7, 15);
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
