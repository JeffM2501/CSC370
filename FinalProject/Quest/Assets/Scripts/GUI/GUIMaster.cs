using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIMaster : MonoBehaviour
{
    public GUISkin Skin;

    public Texture2D[] SkillList;

    public GUIStyle ActionBarStyle = new GUIStyle();

    public float ActionBarScale = 1;

    protected InventoryScreen InventoryWindow;
    protected PlayerStatus StatusWindow;

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
        Debug.Log("GUI Load");

        InventoryWindow = new InventoryScreen();
        InventoryWindow.Init();

        StatusWindow = new PlayerStatus();
        StatusWindow.Init();
    }

    void OnGUI()
    {
        GUI.skin = Skin;

        ActionBar();

        GUIPanel.DrawAll();
    }

    public void ToggleInventory()
    {
        InventoryWindow.Enabled = !InventoryWindow.Enabled;

        Debug.Log("Toggle Inventory to " + InventoryWindow.Enabled.ToString());
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

        GUI.BeginGroup(new Rect(skillSize.width,Camera.main.pixelHeight - (skillSize.height *2),fullWidth,skillSize.height));

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
