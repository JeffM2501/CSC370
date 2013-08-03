using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	void Start ()
	{
        GameState.Instance.Init(this); // fire off the manager
	}

	void Update ()
	{
        Debug.Log("Update");
        print("update keys");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

     //   if (h != 0 || v != 0)
        //    print("H = " + h.ToString() + " V = " + v.ToString());

        GameState.Instance.MovePlayer(new Vector3(h, GameState.MovementZ, v));

        GameState.Instance.Update();

        CheckKeys();
	}

    public void CheckKeys()
    {
        Debug.Log("check keys");
        if (Input.GetKeyDown(KeyCode.I))
            GameState.Instance.GUI.ToggleInventory();
    }

    public void ConsolePrint(string message)
    {
        Debug.Log(message);
    }
}
