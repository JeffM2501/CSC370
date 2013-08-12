using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TargetSelection : GUIPanel 
{
    public Character SelectedCharacter = null;

    public GUIElement SelectedName = null;
    public GUIElement SelectedImage = null;
    public GUIElement SelectedInfo = null;

    public TargetSelection()
    {
        Enabled = false;

        Bounds = new Rect(0, 0, 256, 96);
        HAlignement = Alignments.Max;
        VAlignement = Alignments.Absolute;
    }

    protected override void Load()
    {
        base.Load();

        SelectedName = NewLabel(Alignments.Absolute, 2, Alignments.Absolute, 2, Bounds.width - 66, 62, string.Empty);

        SelectedImage = NewImage(Alignments.Max, 2, Alignments.Absolute, 2, Resources.Load("GUI/CreatureSelectionIcon") as Texture);

        SelectedInfo = NewLabel(Alignments.Absolute, 2, Alignments.Max, 2, Bounds.width - 4, 32, string.Empty);
    }

    public void SelectCharacter( Character character )
    {
        SelectedCharacter = character;

        if (character == null)
        {
            this.Enabled = false;
            return;
        }

        this.Enabled = true;
        SelectedImage.BackgroundImage = Resources.Load("GUI/CreatureSelectionIcon") as Texture;

        SelectedName.Name = character.Name;

        SetCharInfo();
    }

    public void SelectItem(Item item)
    {
        SelectedCharacter = null;

        if (item == null)
        {
            this.Enabled = false;
            return;
        }
        this.Enabled = true;
        SelectedImage.BackgroundImage = item.InventoryIcon;
        SelectedName.Name = item.Name;
        SelectedInfo.Name = string.Empty;
    }

    public void SetCharInfo()
    {
        if (SelectedCharacter.Alive)
            SelectedInfo.Name = "HP: " + (SelectedCharacter.HitPoints - SelectedCharacter.Damage).ToString() + "/" + SelectedCharacter.HitPoints.ToString();
        else
        {
            SelectedInfo.Name = "Dead";
            if (SelectedCharacter.WorldObject == null || !SelectedCharacter.WorldObject.activeSelf)
                Clear();
        }

        if (false)
        {
            float dist = Vector3.Distance(GameState.Instance.PlayerObject.WorldObject.transform.position, SelectedCharacter.WorldObject.transform.position);
            SelectedInfo.Name = dist.ToString();
        } 
    }

    public void Clear()
    {
        SelectedCharacter = null;
        this.Enabled = false;
    }

    public override void PreDraw()
    {
        if (SelectedCharacter != null)
            SetCharInfo();
    }
}
