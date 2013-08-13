using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreasureSpawner : MonoBehaviour 
{
    public int MinGold = 10;
    public int MaxGold = 1000;

    public bool HaveArmor = false;
    public bool HaveWeapons = false;
    public bool HaveItems = false;

    public List<string> ForcedItems = new List<string>();

	void Start ()
	{

	}

    public void Setup()
    {
        ItemContainer container = GetComponent<ItemContainer>();
        if (container != null)
        {
            container.Items.Clear();

            container.Items.GoldCoins = UnityEngine.Random.Range(MinGold, MaxGold);

            if (HaveArmor)
                container.Items.AddItem(ItemFactory.RandomArmor());

            if (HaveWeapons)
                container.Items.AddItem(ItemFactory.RandomWeapon());

            if (HaveItems)
            {
                int count = UnityEngine.Random.Range(1,3);
                for (int i = 0; i < count; i++)
                    container.Items.AddItem(ItemFactory.RandomItem());
            }

            foreach (string item in ForcedItems)
                container.Items.AddItem(ItemFactory.FindItemByName(item));
        }
    }
	
	void Update ()
	{
	
	}
}
