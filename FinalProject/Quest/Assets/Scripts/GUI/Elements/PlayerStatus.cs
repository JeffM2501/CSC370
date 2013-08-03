using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatus : GUIPanel
{
    public Character TheCharacter = null;

    public GUIElement HeaderBar = null;
    public GUIElement StatsFrame = null;
    public GUIElement HealthBar = null;
    public GUIElement ManaBar = null;

    public PlayerStatus()
    {
        Enabled = true;

        Bounds = new Rect(10, 10, 256, 64+32);
        HAlignement = Alignments.Absolute;
        VAlignement = Alignments.Absolute;
    }

    protected override void Load()
    {
        base.Load();
        TheCharacter = GameState.Instance.PlayerObject;

        HeaderBar = this.NewImage(Alignments.Absolute, 0, Alignments.Absolute, 0, Resources.Load("GUI/PlayerNameOval") as Texture2D);

        GUIElement name = HeaderBar.NewLabel(Alignments.Absolute, 32, Alignments.Absolute, 16, 256-64,64-32,TheCharacter.Name);

        StatsFrame = this.NewImage(Alignments.Absolute,32, Alignments.Absolute, 58, Resources.Load("GUI/StatusBarBackgrounds") as Texture2D);

        HealthBar = StatsFrame.NewImage(Alignments.Absolute, 8, Alignments.Absolute, 2, Resources.Load("GUI/HealthStatusFill") as Texture2D);
    }
}