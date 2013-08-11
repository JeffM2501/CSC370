using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharcterScreen : GUIPanel
 {
    public Character TheCharacter = null;

    protected GUIElement XPLabel = null;

    protected List<GUIPanel> Owned = new List<GUIPanel>();
    protected List<GUIPanel> Purchaseable = new List<GUIPanel>();

    protected GUIElement OwnedFrame = null;
    protected GUIElement PurchasableFrame = null;

    public CharcterScreen()
    {
        Enabled = false;

        Bounds = new Rect(10, 10, 512, 512);
        HAlignement = Alignments.Max;
        VAlignement = Alignments.Max;
    }

    protected override void Load()
    {
        base.Load();

        TheCharacter = GameState.Instance.PlayerObject;
        this.Background = Resources.Load("GUI/PlainBackground") as Texture;

        NewImageButton(Alignments.Max, 25, Alignments.Absolute, 25, Resources.Load("GUI/CloseBox") as Texture, Close);

        NewLabel(Alignments.Absolute, 25, Alignments.Absolute, 25, 256, 32, TheCharacter.Name + ":Stats");

        BuildSkills();
        SetPlayerData();
    }

    public void BuildSkills()
    {
    } 

    public void SetPlayerData()
    {
        XPLabel = NewLabel(Alignments.Center, 0, Alignments.Absolute, 25, 256, 32, TheCharacter.Name + "XP:" + TheCharacter.XP.ToString());

    }
}
