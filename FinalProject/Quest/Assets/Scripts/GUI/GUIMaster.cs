using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIMaster : MonoBehaviour
{
    public GUISkin Skin;

    protected List<SkillInstance> SkillList = new List<SkillInstance>();

    public GUIStyle ActionBarStyle = new GUIStyle();

    public float ActionBarScale = 1;

    protected InventoryScreen InventoryWindow;
    protected PlayerStatus StatusWindow;

    public GameObject DropedBagGraphic;

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

    public void SetPlayer(Player player)
    {
        SkillList.Clear();

        SkillList.Add(new SkillInstance(SkillFactory.BasicAttacks[player.EquipedItems.WeaponType()]));
        
        foreach (SkillInstance skill in player.Skills)
        {
            if (skill.BaseSkill.SkillType == Skill.SkillTypes.Active)
                SkillList.Add(skill);
        }

        foreach (SkillInstance skill in player.Skills)
        {
            if (skill.BaseSkill.SkillType == Skill.SkillTypes.Spell)
                SkillList.Add(skill);
        }
    }

    public void ToggleInventory()
    {
        InventoryWindow.Enabled = !InventoryWindow.Enabled;

        Debug.Log("Toggle Inventory to " + InventoryWindow.Enabled.ToString());
    }

    protected int GetSkillCount()
    {
        return SkillList.Count;
    }

    SkillInstance GetSkill(int index)
    {
        return SkillList[index];
    }

    Texture GetSkillImage(int index)
    {
        return Resources.Load(SkillList[index].BaseSkill.IconImage) as Texture;
    }

    protected Rect GetSkillSize()
    {
        if (SkillList.Count == 0)
            return Rect.MinMaxRect(0, 0, 0, 0);

        Texture tex = GetSkillImage(0);
        return new Rect(0, 0, tex.width * ActionBarScale, tex.height * ActionBarScale);
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

        for (int i = 0; i < SkillList.Count; i++ )
        {
            if (GUI.Button(skillSize, GetSkillImage(i), ActionBarStyle))
                GameState.Instance.SkillButtonClicked(GetSkill(i));

            skillSize.x += oneWidth;
        }


        GUI.EndGroup();
    }
}
