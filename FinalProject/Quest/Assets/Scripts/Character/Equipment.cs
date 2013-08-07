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
        Unknown,
    }

    public EquipmentLocation Location = EquipmentLocation.Torso;

    public Texture MaleEquipmentLayer;
    public Texture FemaleEquipmentLayer;

    public Color LayerColor = Color.white;

    public Texture GetTextureForGender(Character.Genders gender)
    {
        return gender == Character.Genders.Male ? MaleEquipmentLayer : FemaleEquipmentLayer;
    }
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

