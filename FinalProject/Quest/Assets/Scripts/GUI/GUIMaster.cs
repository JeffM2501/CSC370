using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIMaster : MonoBehaviour
{
    public Texture2D[] SkillList;

    public GUIStyle PlayerBarStyle = new GUIStyle();
    public GUIStyle ActionBarStyle = new GUIStyle();

    public float ActionBarScale = 1;

    public Texture2D PlayerInfoFrame;

    public InventoryScreen InventoryWindow = new InventoryScreen();

    void Alive()
    {
    }

	void Start ()
	{
        GameState.Instance.Init(this);
	}

	void Update ()
	{
	
	}

    public void Load()
    {
        InventoryWindow.Init();
    }

    void OnGUI()
    {
        ActionBar();
        PlayerInfo();

        InventoryWindow.Draw();
    }

    public void ToggleInventory()
    {
        InventoryWindow.Enabled = !InventoryWindow.Enabled;

        GameState.Instance.Log("Toggle");
    }

    void PlayerInfo()
    {
        if (PlayerInfoFrame == null)
            return;

        Rect rect = new Rect(GetSkillSize().width, GetSkillSize().height, PlayerInfoFrame.width, PlayerInfoFrame.height);
        GUI.BeginGroup(rect);

        GUI.Box(new Rect(0, 0, PlayerInfoFrame.width, PlayerInfoFrame.height), PlayerInfoFrame);
        GUI.Label(new Rect(28, 15, 100, 50), "Player 1", PlayerBarStyle);
        GUI.EndGroup();
    }

    protected int GetSkillCount()
    {
        return SkillList.Length;
    }

    protected Rect GetSkillSize()
    {
        if (SkillList.Length == 0)
            return Rect.MinMaxRect(0, 0, 0, 0);
        return new Rect(0, 0, SkillList[0].width * ActionBarScale, SkillList[0].height * ActionBarScale);
    }

    void ActionBar()
    {
        int SkillCount = GetSkillCount();
        if (SkillCount == 0)
            return;

        Rect skillSize = GetSkillSize();

        float oneWidth = skillSize.width;

        float fullWidth = oneWidth * SkillCount;

        GUI.BeginGroup(new Rect(skillSize.width,Camera.mainCamera.pixelHeight - (skillSize.height *2),fullWidth,skillSize.height));

        int skillButton = 0;
        
        foreach (Texture2D skill in SkillList)
        {
            if (GUI.Button(skillSize, skill, ActionBarStyle))
                GameState.Instance.SkillButtonClicked(skillButton);

            skillSize.x += oneWidth;
            skillButton++;
        }


        GUI.EndGroup();
    }
}
