using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameState
{
    public delegate void EventCallback(object sender, EventArgs args);

    public static GameState Instance = new GameState();

    public InputManager InputMan;
    public SpriteManager SpriteMan = new SpriteManager();

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
    }

    public void Update()
    {
        if (PlayerObject != null)
            PlayerObject.Update();

        foreach (Character c in ActiveCharacters)
            c.Update();

        if (Input.GetKeyDown(KeyCode.I))
            GUI.ToggleInventory();
    }

    public void MovePlayer(Vector3 vec)
    {
        if (PlayerObject == null)
            return;

        PlayerObject.Move(vec);
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
        GameObject nearestBag = FindGameObjectInRadius(location, "DropedBag", 2.0f);

        if (nearestBag == null)
            nearestBag = MonoBehaviour.Instantiate(GUI.DropedBagGraphic) as GameObject;
        
        // get the bag container and add the item
    }

}
