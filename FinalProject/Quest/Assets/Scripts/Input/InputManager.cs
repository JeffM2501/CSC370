using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    public static InputManager Instance;

    GameObject PlayerObject;

    Movement PlayerMovemnt;

	void Start ()
	{
        Instance = this;
	}

    public void SetPlayer(GameObject player)
    {
        PlayerObject = player;
        PlayerMovemnt = player.GetComponent("Movement") as Movement;
    }

	void Update ()
	{
        if (PlayerObject == null)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
	}
}
