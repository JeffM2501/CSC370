using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InGameMenu : GUIPanel 
{
    Texture Logo;
    Texture Continue;
    Texture Exit;
    Texture NewGame;

    GUIElement ContinueButton;
    GUIElement DeadMessage;

    protected bool Dead = false;

    public InGameMenu()
    {
        Logo = Resources.Load("GUI/QuestMenu_logo") as Texture;

        Continue = Resources.Load("GUI/Continue") as Texture;
        Exit = Resources.Load("GUI/Exit") as Texture;
        NewGame = Resources.Load("GUI/NewGame") as Texture;

        Enabled = false;

        Bounds = new Rect(0, 50, Logo.width, Logo.height * 2);
        HAlignement = GUIPanel.Alignments.Center;
        VAlignement = GUIPanel.Alignments.Absolute;

        Skin = Resources.Load("GUI/UI Skin") as GUISkin;
    }

    protected override void Load()
    {
        NewImage(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, 0, Logo);

        NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height, NewGame, GoToMenu);

        float offset = Logo.height;

        DeadMessage = NewLabel(GUIPanel.Alignments.Center, 275, GUIPanel.Alignments.Absolute, offset + Continue.height, Logo.width, 64, "Congratulations, you have died!");
        DeadMessage.Enabled = false;
        DeadMessage.SetFont(Color.white, 32);

        ContinueButton = NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, offset + Continue.height, Continue, ContinueClick);
        if (Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WindowsWebPlayer)
            NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, offset + 2 * Exit.height, Exit, ExitClick);
    }

    public void SetDead()
    {
        Dead = true;
        ContinueButton.Enabled = false;
        DeadMessage.Enabled = true;
    }

    protected void GoToMenu(object sender, EventArgs args)
    {
        GameState.Prefabs.audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip); 
        
        GameState.Instance.CleanUp();
        Application.LoadLevel("MainMenu");
        Dead = false;
        ContinueButton.Enabled = true;
        DeadMessage.Enabled = false;
    }

    protected void ContinueClick(object sender, EventArgs args)
    {
        GameState.Prefabs.audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);

        if (!Dead)
            GameState.Instance.GUI.CloseGameMenu();
    }

    protected void ExitClick(object sender, EventArgs args)
    {
        GameState.Prefabs.audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        Application.Quit();
    }
}
