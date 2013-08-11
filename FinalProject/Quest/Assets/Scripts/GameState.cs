using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameState
{
    public delegate void EventCallback(object sender, EventArgs args);

    public static GameState Instance = new GameState();

    public static GlobalPrefabs Prefabs = null;
    public InputManager InputMan;
    public SpriteManager SpriteMan = new SpriteManager();
    public BattleManager BattleMan = new BattleManager();

    public Player PlayerObject;

    public List<Character> ActiveCharacters = new List<Character>();

    Level LevelMap = new Level();

    public GUIMaster GUI;

    public static float MovementZ = 0;

    public bool Inited = false;

    public void Log(string message)
    {
        if (InputMan == null)
            return;
        InputMan.ConsolePrint(message);
    }

    public void Init( InputManager inMan )
    {
        InputMan = inMan;
        CheckInits();
    }

    public void Init( GUIMaster gui )
    {
        GUI = gui;
        CheckInits();
    }

    public void CheckInits()
    {
        if (InputMan != null && GUI != null)
        {
            SkillFactory.Setup();
            ItemFactory.Setup();
            SpellFeactory.Setup();

            SetPlayer(GameObject.Find("Player") as GameObject);
     
            GUI.Load();
        }
    }

    public void SetPlayer(GameObject player)
    {
        if (player == null)
            return;

        PlayerObject = new Player();
        PlayerObject.Name = "Player1";
        PlayerObject.WorldObject = player;
        PlayerObject.PlayerMovemnt = player.GetComponent("Movement") as Movement;

        PlayerObject.Init();


        MonsterFactory.NewOrc(player.transform.position + new Vector3(0, 0, -2));
        MonsterFactory.NewSkellymans(player.transform.position + new Vector3(0, 0, -4));

        MonsterFactory.NewBandit(player.transform.position + new Vector3(0, 0, -6));


        BattleMan.Init(PlayerObject, ActiveCharacters);
    }

    public void Update()
    {
        if (PlayerObject != null)
            PlayerObject.Update();

        List<Character> newlyDead = new List<Character>();

        foreach (Character c in ActiveCharacters)
        {
            c.Update();

            if (!c.Alive && !c.WorldObject.gameObject.activeSelf)
                newlyDead.Add(c);
        }

        // bring out yer dead
        foreach (Character c in newlyDead)
        {
            c.Bury();
            ActiveCharacters.Remove(c);
        }
       
        if (Input.GetKeyDown(KeyCode.I))
            GUI.ToggleInventory();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // select nearest enemy in perception

            float dist = float.MaxValue;
            GameObject nearest = null;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Mob"))
            {
                CharacterObject c = obj.GetComponent<CharacterObject>();
                if (!obj.gameObject.activeSelf || c == null || !c.TheCharacter.Alive)
                    continue;

                float d = Vector3.Distance(PlayerObject.WorldObject.transform.position, obj.transform.position);

                if (d < PlayerObject.PerceptionRange)
                {
                    if (nearest == null || (d < dist))
                    {
                        nearest = obj;
                        dist = d;
                    }
                }
            }

            SelectMob(nearest);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            GUI.ProcessSkillClick(0);
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            GUI.ProcessSkillClick(1);
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            GUI.ProcessSkillClick(2);
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            GUI.ProcessSkillClick(3);
        else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            GUI.ProcessSkillClick(4);
        else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            GUI.ProcessSkillClick(5);
        else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            GUI.ProcessSkillClick(6);
        else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
            GUI.ProcessSkillClick(7);
        else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
            GUI.ProcessSkillClick(8);
        else if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            GUI.ProcessSkillClick(9);

    }

    protected ItemContainer GetBag(Vector3 location)
    {
        GameObject bag = FindGameObjectInRadius(location, "LootDrop", 2.0f);

        if (bag == null)
        {
            bag = MonoBehaviour.Instantiate(Prefabs.DroppedBag) as GameObject;
            bag.transform.position = new Vector3(location.x, 0.125f, location.z);
        }

        return bag.GetComponent<ItemContainer>();
    }

    public void DropLoot(Character character)
    {
        ItemContainer container = GetBag(character.WorldObject.transform.position);
        
        container.Items.AddItem(character.EquipedItems.Head);
        container.Items.AddItem(character.EquipedItems.Torso);
        container.Items.AddItem(character.EquipedItems.LeftHand);
        container.Items.AddItem(character.EquipedItems.RightHand);

        foreach (Item item in character.InventoryItems.GetItems())
            container.Items.AddItem(item);

        character.ClearInventory();
    }

    public void MovePlayer(Vector3 vec)
    {
        if (PlayerObject == null)
            return;

        PlayerObject.Move(vec);
    }

    public void SelectMob(GameObject obj)
    {
        if (PlayerObject.Target != null)
            PlayerObject.Target.Select(false);

        PlayerObject.Target = null;

        if (obj == null)
            return;

        CharacterObject targetObject  = obj.GetComponent<CharacterObject>();
        if (targetObject == null)
            return;

        PlayerObject.Target = targetObject.TheCharacter;
        PlayerObject.Target.Select(true);

        GUI.SelectCharacter(PlayerObject.Target);
    }

    public void RoomStartup(RoomInstnace room)
    {
        LevelMap.AddRoom(room);
    }

    public void SkillButtonClicked(SkillInstance skill)
    {
        skill.OnAcivate(PlayerObject);
    }

    public static GameObject FindGameObjectInRadius(Vector3 source, string tag, float rad)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            float dist = Vector3.Distance(source, obj.transform.position);
            if (dist <= rad)
                return obj;
        }

        return null;
    }

    public void DropItem(Item item, Vector3 location)
    {
        GetBag(location).Items.AddItem(item);
    }

}
