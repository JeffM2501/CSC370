using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class InventoryScreen : GUIPanel
{
    public Character TheCharacter = null;

    public GUIElement HeadSlot = null;
    public GUIElement BodySlot = null;
    public GUIElement LeftSlot = null;
    public GUIElement RightSlot = null;

    public GUIStyle TextStyle = new GUIStyle();

    public List<GUIElement> InventorySlots = new List<GUIElement>();

    public GUIElement GoldText = null;

    public InventoryScreen()
    {
        Enabled = false;

        Bounds = new Rect(10, 10, 512, 512);
        HAlignement = Alignments.Absolute;
        VAlignement = Alignments.Max;
    }

    protected override void Load()
    {
        base.Load();
        TheCharacter = GameState.Instance.PlayerObject;

        if (TheCharacter.Gender == Character.Genders.Female)
            this.Background = Resources.Load("GUI/InventoryFemale") as Texture;
        else
            this.Background = Resources.Load("GUI/InventoryMale") as Texture;

        NewImageButton(Alignments.Max, 25, Alignments.Absolute, 25, Resources.Load("GUI/CloseBox") as Texture, Close);

        // equipped items
        HeadSlot = NewImageButton(Alignments.Center, -3, Alignments.Absolute, 20, 86, 80, null, SlotClick);
        BodySlot = NewImageButton(Alignments.Center, -3, Alignments.Absolute, 112, 86, 80, null, SlotClick);
        LeftSlot = NewImageButton(Alignments.Center, 100, Alignments.Absolute, 112, 86, 80, null, SlotClick);
        RightSlot = NewImageButton(Alignments.Center, -107, Alignments.Absolute, 112, 86, 80, null, SlotClick);

        // inventory
        float leftBuffer = 13;
        float yStart = 316;
        float ybuffer = 14;
        float xBuffer = 13;

        float boxSize = 70;

        int id = 0;
        for (int r = 0; r < 2; r++)
        {
            for (int i = 0; i < 6; i++)
            {
               // int id = i + (r*6);

                float x = leftBuffer + (i * (boxSize + xBuffer));
                float y = yStart + (r * (ybuffer + boxSize));
                GUIElement element = NewImageButton(Alignments.Absolute, x, Alignments.Absolute, y, boxSize, boxSize, null, InventoryClick);
                element.ID = id;
                id++;
                InventorySlots.Add(element);
            }
        }

        // gold label
        NewImage(Alignments.Absolute, 155, Alignments.Absolute, 245, Resources.Load("Items/GoldCoin") as Texture);
        GoldText = NewLabel(Alignments.Absolute, 190, Alignments.Absolute, 245, 100, 35, "000");

        SetInventoryItems();
    }

    protected void SlotClick(object sender, EventArgs args)
    {
        if (HeadSlot == sender)
            Debug.Log("HeadSlot click");
        else if (BodySlot == sender)
            Debug.Log("BodySlot click");
        else if (LeftSlot == sender)
            Debug.Log("LeftSlot click");
        else if (RightSlot == sender)
            Debug.Log("RightSlot click");

        GameState.Instance.GUI.SelectItem((sender as GUIElement).Tag as Item);
    }

    protected void InventoryClick(object sender, EventArgs args)
    {
        GUIElement element = sender as GUIElement;
        if (element == null)
            return;

        int slotID = element.ID;
        Debug.Log("GUI item slot click " + slotID.ToString());

        Item item = TheCharacter.InventoryItems.GetItem(slotID);

        GameState.Instance.GUI.SelectItem(item);

        Equipment equipment = item as Equipment;
        if (equipment != null)
            TheCharacter.EquipItem(TheCharacter.InventoryItems.RemoveItem(slotID) as Equipment);
        else
        {
            if (item.OnActivate(TheCharacter))
                TheCharacter.InventoryItems.RemoveItem(slotID);
        }

        SetInventoryItems();
    }

    protected void SetElementImage(GUIElement element, Item item)
    {
        if (item == null || item.InventoryIcon == null)
            element.BackgroundImage = null;
        else
            element.BackgroundImage = item.InventoryIcon;

        element.Tag = item;
    }

    public void SetInventoryItems()
    {
        SetElementImage(HeadSlot, TheCharacter.EquipedItems.Head);
        SetElementImage(BodySlot, TheCharacter.EquipedItems.Torso);
        SetElementImage(LeftSlot, TheCharacter.EquipedItems.LeftHand);
        SetElementImage(RightSlot, TheCharacter.EquipedItems.RightHand);

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            Item invItem = TheCharacter.InventoryItems.GetItem(i);
//             Debug.Log("SetElementImage  " + i.ToString() + " to ");
//             Debug.Log(invItem);
            SetElementImage(InventorySlots[i], invItem);
        }

        GoldText.Name = TheCharacter.InventoryItems.GoldCoins.ToString();
    }
}
