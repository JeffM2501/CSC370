using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EquipmentList
{
    public Equipment Head;
    public Equipment Torso;
    public Equipment Ring;
    public Equipment Amulet;
    public Weapon LeftHand;
    public Weapon RightHand;

    public Weapon EquipWeapon(Weapon item, bool leftHand)
    {
        if (item == null)
            return item;

        if (item.Location != Equipment.EquipmentLocation.Weapon  && item.Location != Equipment.EquipmentLocation.OffHand)
            return item;

        // no dual wielding
        if (item.Location == Equipment.EquipmentLocation.Weapon)
        {
            if (leftHand && RightHand != null && RightHand.Location == Equipment.EquipmentLocation.Weapon)
                return item;

            if (!leftHand && LeftHand != null && LeftHand.Location == Equipment.EquipmentLocation.Weapon)
                return item;
        }

        Weapon returnedItem = leftHand ? LeftHand : RightHand;
        if (leftHand)
            LeftHand = item;
        else
            RightHand = item;

        return returnedItem;
    }

    public Equipment EquipArmor(Equipment item)
    {
        if (item == null)
            return null;

        Equipment returnedItem = null;

        switch (item.Location)
        {
            case Equipment.EquipmentLocation.Head:
                returnedItem = Head;
                Head = item;
                break;

            case Equipment.EquipmentLocation.Torso:
                returnedItem = Torso;
                Torso = item;
                break;

            case Equipment.EquipmentLocation.Ring:
                returnedItem = Ring;
                Ring = item;
                break;

            case Equipment.EquipmentLocation.Amulet:
                returnedItem = Amulet;
                Amulet = item;
                break;

            default:
                returnedItem = item;
                break;
        }

        return returnedItem;
    }

    public bool IsWielding(Weapon.WeaponTypes weapon)
    {
        return (LeftHand != null && LeftHand.WeaponType == weapon) || (RightHand != null && RightHand.WeaponType == weapon);
    }

    public Weapon.WeaponTypes WeaponType()
    {
        if (LeftHand != null)
            return LeftHand.WeaponType;
        if (RightHand != null)
            return RightHand.WeaponType;

        return Weapon.WeaponTypes.Hand;
    }

    public int ArmorValue()
    {
        int value = 0;

        if (Head as Armor != null)
            value += (Head as Armor).ArmorValue;
        if (Torso as Armor != null)
            value += (Torso as Armor).ArmorValue;
        return value;
    }
}
