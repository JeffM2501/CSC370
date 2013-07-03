using UnityEngine;
using System.Collections;

public class StopBalls : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

    void OnTriggerEnter (Collider obj )
    {
        obj.gameObject.tag ="Untagged";
    }
}
