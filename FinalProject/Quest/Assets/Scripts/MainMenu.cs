using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
 {
    protected GUISkin MenuSkin;
    public Texture Logo;

    public Texture NewGameButton;
    public Texture NewGamePlusButton;
    public Texture ControlsButton;
    public Texture ExitButton;

    public Texture ConrolsWindow;
    public Texture BackButton;

    protected float CamWidth = 0;

    protected bool Exit = false;
    protected bool Load = false;

    GUIPanel MenuPanel = null;
    GUIPanel ControlsPanel = null;

	void Start ()
    {
        MenuSkin = Resources.Load("GUI/UI Skin") as GUISkin;

        Exit = false;
        Load = false;

        CamWidth = Camera.main.pixelWidth;

        GUIPanel.Pannels.Clear();

        MenuPanel = new GUIPanel();

        MenuPanel.Enabled = true;
        if (Logo == null)
            return;

        MenuPanel.Bounds = new Rect(0, 50, Logo.width, Logo.height * 2);
        MenuPanel.HAlignement = GUIPanel.Alignments.Center;
        MenuPanel.VAlignement = GUIPanel.Alignments.Absolute;

        MenuPanel.NewImage(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, 0, Logo);

        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height, NewGameButton, NewGameClick);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + NewGameButton.height, NewGamePlusButton, NewGame2Click);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + 2 * NewGameButton.height, ControlsButton, ControlsClick);
        MenuPanel.NewImageButton(GUIPanel.Alignments.Center, 0, GUIPanel.Alignments.Absolute, Logo.height + 3 * NewGameButton.height, ExitButton, ExitClick);

        MenuPanel.Skin = MenuSkin;

        ControlsPanel = new GUIPanel();
        ControlsPanel.Enabled = false;

        ControlsPanel.Bounds = new Rect(0, 50, ConrolsWindow.width, ConrolsWindow.height);
        ControlsPanel.Background = ConrolsWindow;
        ControlsPanel.HAlignement = GUIPanel.Alignments.Center;
        ControlsPanel.VAlignement = GUIPanel.Alignments.Absolute;
        ControlsPanel.Skin = MenuSkin;

        ControlsPanel.NewImageButton(GUIPanel.Alignments.Absolute, 12, GUIPanel.Alignments.Max, 6, BackButton, ControlBack);
	}

    protected void NewGameClick(object sender, EventArgs args)
    {
        audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        Debug.Log("New Game");
        PlayerPrefs.SetInt("PlayType", 1);
        Load = true;
    }

    protected void NewGame2Click(object sender, EventArgs args)
    {
        audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        Debug.Log("New Game Plus");
        PlayerPrefs.SetInt("PlayType", 2);
        Load = true;
    }

    protected void ControlsClick(object sender, EventArgs args)
    {
        audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        Debug.Log("Controls Click");
        ControlsPanel.Enabled = true;
        MenuPanel.Enabled = false;
    }

    protected void ExitClick(object sender, EventArgs args)
    {
        audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        Exit = true;
    }

    protected void ControlBack(object sender, EventArgs args)
    {
        audio.PlayOneShot(Resources.Load("Sounds/sfx_click") as AudioClip);
        ControlsPanel.Enabled = false;
        MenuPanel.Enabled = true;
    }

	void Update ()
	{
        if (Camera.main.pixelWidth != CamWidth)
        {
            GUIPanel.RebuildAll();
            CamWidth = Camera.main.pixelWidth;
        }

        if (Exit)
            Application.Quit();
        else if (Load)
            Application.LoadLevel("DungonGenerator");
	}

    void OnGUI()
    {
        GUI.skin = MenuSkin;
        MenuPanel.Draw();
        ControlsPanel.Draw();
    }
}
