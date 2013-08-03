using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class InventoryScreen : GUIPanel
{
    public Character TheCharacter = null;

    public GUIElement HeadSlot = null;

    public InventoryScreen()
    {
        
    }

    protected override void Load()
    {
        base.Load();
        TheCharacter = GameState.Instance.PlayerObject;

        if (TheCharacter.Gender == Character.Genders.Female)
            this.Background = Resources.Load("GUI/InventoryFemale") as Texture2D;
        else
            this.Background = Resources.Load("GUI/InventoryMale") as Texture2D;

        GameState.Instance.Log(this.Background.ToString());

        HeadSlot = NewImageButton(Alignments.Center, 0, Alignments.Absolute, 20, 80, 80, null, HeadSlotClick);

    }

    protected void HeadSlotClick(object sender, EventArgs args)
    {
    }
}
