using UnityEngine;
using System.Collections;
using System;

public class Equipment : Item
{
    public enum EquipmentLocation
    {
        Head,
        Torso,
        Ring,
        Amulet,
        Weapon,
        OffHand,
    }

    public EquipmentLocation Location = EquipmentLocation.Torso;

    public Texture MaleEquipmentLayer;
    public Texture FemaleEquipmentLayer;
}

public class Weapon : Equipment
{
    public enum WeaponTypes
    {
        Hand,
        Sword,
        Staff,
        Bow,
    }

    public WeaponTypes WeaponType = WeaponTypes.Hand;

    public int MinDamage = 0;
    public int MaxDamage = 0;

    public Weapon()
    {
        Location = Equipment.EquipmentLocation.Weapon;
    }
}

public class Armor : Equipment
{
    public int ArmorValue = 0;

    public Armor()
    {
    }
}

