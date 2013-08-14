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

    public GUIElement XPDisplay = null;

    public PlayerStatus()
    {
        Enabled = true;

        Bounds = new Rect(0, 0, 204, 60+40+32);
        HAlignement = Alignments.Absolute;
        VAlignement = Alignments.Absolute;
    }

    protected override void Load()
    {
        base.Load();
        TheCharacter = GameState.Instance.PlayerObject;

        HeaderBar = this.NewImage(Alignments.Absolute, 0, Alignments.Absolute, 0, Resources.Load("GUI/PlayerNameOval") as Texture);

        HeaderBar.NewLabel(Alignments.Absolute, 32, Alignments.Absolute, 16, 256-64,64-32,TheCharacter.Name);

        StatsFrame = this.NewImage(Alignments.Absolute, 32, Alignments.Absolute, 60, Resources.Load("GUI/StatusBarBackgrounds") as Texture);
        StatsFrame.Name = "status Frame";

        HealthBar = StatsFrame.NewImage(Alignments.Absolute, 25, Alignments.Absolute, 6, Resources.Load("GUI/HealthStatusFill") as Texture);
        HealthBar.Name = "Health bar";

        ManaBar = StatsFrame.NewImage(Alignments.Absolute, 24, Alignments.Absolute, 26, Resources.Load("GUI/MagicStatusFill") as Texture);
        ManaBar.Name = "Mana bar";

        XPDisplay = NewLabel(Alignments.Absolute, 32, Alignments.Max, 0, 204,30,"XP:0");

        SetHealth(TheCharacter.GetHealthParam());
        SetMana(TheCharacter.GetManaParam());
    }

    public override void PreDraw()
    {
        base.PreDraw();

        if (TheCharacter != null)
        {
            SetHealth(TheCharacter.GetHealthParam());
            SetMana(TheCharacter.GetManaParam());

            XPDisplay.Name = "XP:" + TheCharacter.XP.ToString();
        }
    }

    protected static float BarWidth = 102;
    protected static float BarOffset = 0;

    public void SetMana(float param)
    {
        ManaBar.Bounds.width = BarOffset + (BarWidth * param);
        ManaBar.ForceRebuild();
    }

    public void SetHealth(float param)
    {
        HealthBar.Bounds.width = BarOffset + (BarWidth * param);
        HealthBar.ForceRebuild();
    }
}