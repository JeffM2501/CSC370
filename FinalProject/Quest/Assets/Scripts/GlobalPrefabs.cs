using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GlobalPrefabs : MonoBehaviour
 {
    public GameObject DroppedBag = null;

    void Alive()
    {
        GameState.Prefabs = this;
    }

	void Start ()
	{
        GameState.Prefabs = this;
	}

	void Update ()
	{
	
	}
}
