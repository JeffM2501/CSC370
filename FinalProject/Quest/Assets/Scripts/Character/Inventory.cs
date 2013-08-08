using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Inventory
{
    protected List<Item> Items = new List<Item>();
    public int MaxItems = 12;

    public Inventory() { }

    public Inventory(int maxItems)
    {
        MaxItems = maxItems;
    }

    public Item[] GetItems()
    {
        return Items.ToArray();
    }

    public Item GetItem( int index )
    {
        if (Items.Count <= index)
            return null;

        return Items[index];
    }

    public bool AddItem(Item item)
    {
        if (Items.Count == MaxItems)
            return false;

        Items.Add(item);
        return true;
    }

    public Item RemoveItem( int index )
    {
        if (index < 0 || index >= Items.Count)
            return null;

        Item ret = Items[index];
        Items.RemoveAt(index);
        return ret;
    }
}
