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
    protected TargetSelection Selector;
    protected LootScreen Loot;
    protected InGameMenu GameMenu;

    protected CharcterScreen SkillScreen;

    public Vector2 ScreenSize = Vector2.zero;

    protected Player ThePlayer;

    public Texture SkillCover = null;

    protected float LastSkillClick;

    public bool InMenu = false;

    void Awake()
    {
    }

	void Start ()
	{
        GameState.Instance.Init(this);
        ScreenSize = new Vector2(Camera.main.pixelWidth,Camera.main.pixelHeight);
	}

	void Update ()
	{
        if (ScreenSize.x != Camera.main.pixelWidth || ScreenSize.y != Camera.main.pixelHeight)
        {
            GUIPanel.RebuildAll();
            ScreenSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        }

        if (StatusWindow != null && ThePlayer != null)
        {
            if (ThePlayer.HitPoints != 0)
                StatusWindow.SetHealth(ThePlayer.Damage / (float)ThePlayer.HitPoints);

            if (ThePlayer.ManaSpent != 0)
                StatusWindow.SetMana(ThePlayer.ManaSpent / (float)ThePlayer.MagicPower);
            else
                StatusWindow.SetMana(0);
        }

        if (ThePlayer != null && ThePlayer.Alive && Input.GetKeyDown(KeyCode.Escape))
        {
            if (InMenu)
            {
                InMenu = false;
                GameMenu.Enabled = false;
            }
            else
            {
                InMenu = true;
                GameMenu.Enabled = true;
            }
        }

        // check for clicks
        if (Input.GetMouseButtonDown(0) && !InMenu)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // turn ourselves off so we don't get us
            ThePlayer.WorldObject.collider.enabled = false;

            if (Physics.Raycast(ray, out hit, 100) )
            {
            //    Debug.Log(hit.transform.gameObject);
            //    Debug.Log(hit.transform.gameObject.tag);

                if (hit.transform.gameObject.tag == "Mob")
                    GameState.Instance.SelectMob(hit.transform.gameObject);
                else if (hit.transform.gameObject.tag == "LootDrop")
                {
                    GameState.Instance.SelectMob(null);
                    Loot.Show(hit.transform.gameObject.GetComponent<ItemContainer>());
                }
            }

            ThePlayer.WorldObject.collider.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SkillScreen.BuildSkills();
	}

    public void Load()
    {
        GUIPanel.Pannels.Clear();

        InventoryWindow = new InventoryScreen();
        InventoryWindow.Init();

        SkillScreen = new CharcterScreen();
        SkillScreen.Init();

        StatusWindow = new PlayerStatus();
        StatusWindow.Init();

        Selector = new TargetSelection();
        Selector.Init();
        Selector.Enabled = false;

        Loot = new LootScreen();
        Loot.Init();
        Loot.Enabled = false;

        GameMenu = new InGameMenu();
        GameMenu.Init();
        GameMenu.Enabled = false;
        InMenu = false;

        SetPlayer(GameState.Instance.PlayerObject);
    }

    void OnGUI()
    {
        GUI.skin = Skin;

        ActionBar();

        GUIPanel.DrawAll();
    }

    public void CloseGameMenu()
    {
        InMenu = false;
        GameMenu.Enabled = false;
    }

    public void ShowDeadMenu()
    {
        CloseGameMenu();
        InMenu = true;
        GameMenu.SetDead();
        GameMenu.Enabled = true;
    }

    public void SetPlayer(Player player)
    {
        ThePlayer = player;
        BuildSkillList(ThePlayer,EventArgs.Empty);

        ThePlayer.EquipementChanged += BuildSkillList;
    }

    public void BuildSkillList(object sender, EventArgs args)
    {
        SkillList.Clear();

        SkillList.Add(ThePlayer.BasicAttackSkill);

        foreach (SkillInstance skill in ThePlayer.Skills)
        {
            if (skill.BaseSkill.SkillType == Skill.SkillTypes.Active)
                SkillList.Add(skill);
        }

        foreach (SpellInstance spell in ThePlayer.Spells)
        {
            if (spell.BaseSkill.SkillType == Skill.SkillTypes.Spell)
                SkillList.Add(spell);
        }
    }

    public void ToggleInventory()
    {
        InventoryWindow.Enabled = !InventoryWindow.Enabled;

        if (InventoryWindow.Enabled)
            InventoryWindow.SetInventoryItems();
    }

    public void UpdateInventory()
    {
        if (InventoryWindow.Enabled)
            InventoryWindow.SetInventoryItems();
    }

    public void ToggleSkills()
    {
        SkillScreen.Enabled = !SkillScreen.Enabled;

        if (SkillScreen.Enabled)
            SkillScreen.SetPlayerData();
    }

    public void UpdateSkills()
    {
        if (SkillScreen.Enabled)
            SkillScreen.SetPlayerData();
    }

    public void SelectCharacter(Character character)
    {
        Selector.SelectCharacter(character);
    }

    public void SelectItem(Item item)
    {
        Selector.SelectItem(item);
    }

    public void ClearSelection()
    {
        Selector.Clear();
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

    public void ProcessSkillClick( int id )
    {
        Player player = GameState.Instance.PlayerObject;

        SkillInstance skill = GetSkill(id);

        if (skill == null || !skill.Useable(player))
            return;

        if (player.Target == null)
            GameState.Instance.SelectNearestMob();

        if (id == 0)
            player.BasicAttack();
        else
        {
            SpellInstance spell = skill as SpellInstance;
            if (spell != null)
                player.CastSpell(spell);
            else
                player.ActivateSkill(GetSkill(id));
        }
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
            SkillInstance skill = GetSkill(i);

            if (GUI.Button(skillSize, GetSkillImage(i), ActionBarStyle) && ThePlayer.SkillUseable(skill))
                ProcessSkillClick(i);

            if (!ThePlayer.SkillUseable(skill))
            {
                float param = ThePlayer.GetSkillUseParamater(skill);

                param *= skillSize.height;

                float top = skillSize.yMin + ( skillSize.height - param);
                Rect paramBox = new Rect(skillSize.xMin,top,skillSize.width,param);

                GUI.DrawTexture(paramBox, SkillCover);
            }

            skillSize.x += oneWidth;
        }


        GUI.EndGroup();
    }
}
