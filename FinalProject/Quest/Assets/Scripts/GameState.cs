using UnityEngine;
using System.Collections;
using System;

public class GameState
{
    public delegate void EventCallback(object sender, EventArgs args);

    public static GameState Instance = new GameState();

    public InputManager InputMan;
    public SpriteManager SpriteMan = new SpriteManager();

    public Player PlayerObject;

    int CurrentRoom = -1;

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
            SetPlayer(GameObject.Find("Player") as GameObject);
            SkillFactory.Setup();
            ItemFactory.Setup();

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
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            GUI.ToggleInventory();
    }

    public void MovePlayer(Vector3 vec)
    {
        if (PlayerObject == null)
            return;

        PlayerObject.PlayerMovemnt.Move(vec * (10 * Time.deltaTime));
    }

    public void PlayerMoveRoom(RoomInstnace room, GameObject player)
    {
        CurrentRoom = room.RoomID;
    }

    public void RoomStartup(RoomInstnace room)
    {
        LevelMap.AddRoom(room);
    }

    public void SkillButtonClicked(SkillInstance skill)
    {
        skill.OnAcivate(PlayerObject);
    }
}
