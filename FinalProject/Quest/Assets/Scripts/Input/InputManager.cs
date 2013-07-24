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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

     //   if (h != 0 || v != 0)
        //    print("H = " + h.ToString() + " V = " + v.ToString());

        GameState.Instance.MovePlayer(new Vector3(h, GameState.MovementZ, v));

        GameState.Instance.Update();
	}
}