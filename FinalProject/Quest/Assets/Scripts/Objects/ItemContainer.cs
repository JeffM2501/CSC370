using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemContainer : MonoBehaviour
 {
    public Inventory Items = new Inventory();

	void Start ()
	{
	    Items.MaxItems = int.MaxValue;
	}

	void Update ()
	{
        if (Items.ItemCount() != 0)
        {
            // draw the baubble
        }
	}
}
