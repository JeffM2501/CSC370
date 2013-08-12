using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LootScreen : GUIPanel 
{
    public ItemContainer Container = null;

    public List<GUIElement> Items = new List<GUIElement>();
    public GUIElement GoldButton = null;
    public GUIElement NameLabel = null;

	public LootScreen()
    {
        Enabled = false;

        Background = Resources.Load("GUI/LootPanel") as Texture;

        Bounds = new Rect(25, 256, 256, 256);
        HAlignement = Alignments.Max;
        VAlignement = Alignments.Max;


        NewImageButton(Alignments.Max, 8, Alignments.Absolute, 8, Resources.Load("GUI/CloseBox") as Texture, Close);

        NameLabel = NewLabel(Alignments.Absolute, 8, Alignments.Absolute, 2, Bounds.width - 24, 32, string.Empty);
        GoldButton = NewButton(Alignments.Absolute, 86, Alignments.Absolute, 49, 100, 24, string.Empty, GoldClick);

        // inventory
        float leftBuffer = 11;
        float yStart = 93;
        float ybuffer = 13;
        float xBuffer = 12;

        float boxSize = 70;

        int id = 0;
        for (int r = 0; r < 2; r++)
        {
            for (int i = 0; i < 3; i++)
            {
                // int id = i + (r*6);

                float x = leftBuffer + (i * (boxSize + xBuffer));
                float y = yStart + (r * (ybuffer + boxSize));
                GUIElement element = NewImageButton(Alignments.Absolute, x, Alignments.Absolute, y, boxSize, boxSize, null, ItemClick);
                element.ID = id;
                element.Tag = null;
                id++;
                Items.Add(element);
            }
        }
    }

    public void Show(ItemContainer container)
    {
        if (container == null)
        {
            Enabled = false;
            return;
        }

        Enabled = true;

        Container = container;

        NameLabel.Name = Container.Name;
        GoldButton.Name = Container.Items.GoldCoins.ToString();

        foreach (GUIElement element in Items)
            SetElementImage(element, Container.Items.GetItem(element.ID));
    }

    protected void SetElementImage(GUIElement element, Item item)
    {
        if (item == null || item.InventoryIcon == null)
            element.BackgroundImage = null;
        else
            element.BackgroundImage = item.InventoryIcon;

        element.Tag = item;
    }

    protected void ItemClick(object sender, EventArgs args)
    {
        GUIElement element = sender as GUIElement;
        if (element == null)
            return;

        int slotID = element.ID;
        Debug.Log("GUI item slot click " + slotID.ToString());

        Item item = Container.Items.GetItem(slotID);
        if (item == null)
            return;

        if (GameState.Instance.PlayerObject.InventoryItems.AddItem(item))
        {
            Container.Items.RemoveItem(slotID);

            Show(Container);
            GameState.Instance.GUI.UpdateInventory();
        }
        CheckEmpty();
    }

    protected void GoldClick(object sender, EventArgs args)
    {
        GameState.Instance.PlayerObject.InventoryItems.GoldCoins += Container.Items.GoldCoins;
        Container.Items.GoldCoins = 0;
        Show(Container);
        GameState.Instance.GUI.UpdateInventory();

        CheckEmpty();
    }

    protected void CheckEmpty()
    {
        if (Container.Items.ItemCount() == 0 && Container.Items.GoldCoins == 0)
        {
            MonoBehaviour.Destroy(Container.gameObject);
            Container = null;
            Enabled = false;
        }
    }
}
