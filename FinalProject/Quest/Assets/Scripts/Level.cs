using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public List<RoomInstnace> Rooms = new List<RoomInstnace>();

	void Start ()
	{
	
	}

    public void AddRoom(RoomInstnace room)
    {
        if (!Rooms.Contains(room))
            Rooms.Add(room);
    }

	void Update ()
	{
	
	}
}
