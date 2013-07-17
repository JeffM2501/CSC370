using UnityEngine;
using System.Collections;

public class cubespinner : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 90);
	}
}
