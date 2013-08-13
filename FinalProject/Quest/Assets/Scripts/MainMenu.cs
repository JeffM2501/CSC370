using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
 {
    public GUISkin MenuSkin;
    public Texture Logo;

    public Texture NewGameButton;
    public Texture NewGamePlusButton;
    public Texture ControlsButton;
    public Texture ExitButton;

    protected float CamWidth = 0;

    GUIPanel MenuPanel = null;

	void Start ()
    {
        CamWidth = Camera.main.pixelWidth;

        GUIPanel.Pannels.Clear();

        MenuPanel = new GUIPanel();

        MenuPanel.Enabled = true;
        if (Logo == null)
            return;

        MenuPanel.Bounds = new Rect(0, 10, Logo.width, Logo.height * 2);
        MenuPanel.HAlignement = GUIPanel.Alignments.Center;
        MenuPanel.VAlignement = GUIPanel.Alignments.Absolute;

        MenuPanel.NewImage(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, 0, Logo);

        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height, NewGameButton, NewGameClick);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + NewGameButton.height, NewGamePlusButton, NewGame2Click);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + 2 * NewGameButton.height, ControlsButton, ControlsClick);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + 3 * NewGameButton.height, ExitButton, ExitClick);
	}

    protected void NewGameClick(object sender, EventArgs args)
    {
        Debug.Log("New Game");
    }

    protected void NewGame2Click(object sender, EventArgs args)
    {
        Debug.Log("New Game 2");
    }

    protected void ControlsClick(object sender, EventArgs args)
    {
        Debug.Log("Controls Click");
    }

    protected void ExitClick(object sender, EventArgs args)
    {
        Debug.Log("Exit");
    }

	void Update ()
	{
        if (Camera.main.pixelWidth != CamWidth)
        {
            GUIPanel.RebuildAll();
            CamWidth = Camera.main.pixelWidth;
        }
	}

    void OnGUI()
    {
        GUI.skin = MenuSkin;
        MenuPanel.Draw();
    }
}
