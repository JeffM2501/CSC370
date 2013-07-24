using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FloorActivator : MonoBehaviour
{
    RoomInstnace Room = null;
	void Start ()
	{
        Room = transform.parent.gameObject.GetComponent<RoomInstnace>();
	}

	void Update ()
	{
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Player")
            return;

        GameState.Instance.PlayerMoveRoom(Room, col.gameObject);
    }
}
