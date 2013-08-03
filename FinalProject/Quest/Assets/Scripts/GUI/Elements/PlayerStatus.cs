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

        Bounds = new Rect(0, 0, 204, 60+40);
        HAlignement = Alignments.Absolute;
        VAlignement = Alignments.Absolute;
    }

    protected override void Load()
    {
        base.Load();
        TheCharacter = GameState.Instance.PlayerObject;

        HeaderBar = this.NewImage(Alignments.Absolute, 0, Alignments.Absolute, 0, Resources.Load("GUI/PlayerNameOval") as Texture2D);

       // GUIElement name = HeaderBar.NewLabel(Alignments.Absolute, 32, Alignments.Absolute, 16, 256-64,64-32,TheCharacter.Name);

        StatsFrame = this.NewImage(Alignments.Absolute,32, Alignments.Absolute, 60, Resources.Load("GUI/StatusBarBackgrounds") as Texture2D);
        StatsFrame.Name = "Satus Frame";

        HealthBar = StatsFrame.NewImage(Alignments.Absolute, 24, Alignments.Absolute, 4, Resources.Load("GUI/HealthStatusFill") as Texture2D);
        HealthBar.Name = "health bar";    
    
    }
}