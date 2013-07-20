using UnityEngine;
using System.Collections;
using System;

public class GameState
{
    public delegate void EventCallback(object sender, EventArgs args);

    public static GameState Instance = new GameState();

    public InputManager InputMan;
    public SpriteManager SpriteMan = new SpriteManager();

    GameObject PlayerObject;
    Movement PlayerMovemnt;

    public static float MovementZ = 0;

    public void Init( InputManager inMan )
    {
        InputMan = inMan;
        SetPlayer(GameObject.Find("Player") as GameObject);
    }

    public void SetPlayer(GameObject player)
    {
        if (player == null)
            return;

      //  PlayerObject = player;
        PlayerMovemnt = player.GetComponent("Movement") as Movement;
    }

    public void Update()
    {
        // do our work
    }

    public void MovePlayer(Vector3 vec)
    {
        if (PlayerMovemnt == null)
            return;

        PlayerMovemnt.Move(vec * (10 * Time.deltaTime));
    }
}
