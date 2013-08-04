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

    public Texture2D MaleEquipmentLayer;
    public Texture2D FemaleEquipmentLayer;
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

    public Weapon()
    {
        Location = Equipment.EquipmentLocation.Weapon;
    }
}
