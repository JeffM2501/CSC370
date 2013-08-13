using UnityEngine;
using System.Collections;
using System;

public class Item
{
    public UInt64 ItemID = UInt64.MinValue;
    public string Name = string.Empty;
    public Texture InventoryIcon;
    public Texture LootIcon;

    public override string ToString()
    {
        return Name;
    }

    public virtual bool OnActivate(Character character)
    {
        return false;
    }
}
