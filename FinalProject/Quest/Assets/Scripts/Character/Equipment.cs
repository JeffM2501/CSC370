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

    public string MaleEquipmentLayer;
    public string FemaleEquipmentLayer;

    public Color LayerColor = Color.white;

    public string GetTextureForGender(Character.Genders gender)
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

    public override string ToString()
    {
        return base.ToString() + " Damage: " + MinDamage.ToString() + " to " + MaxDamage.ToString();
    }

    public Weapon()
    {
        Location = Equipment.EquipmentLocation.Weapon;
    }
}

public class Armor : Equipment
{
    public int ArmorValue = 0;

    public override string ToString()
    {
        return base.ToString() + " Armor: " + ArmorValue.ToString();
    }

    public Armor()
    {
    }
}

