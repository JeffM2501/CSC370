using UnityEngine;
using System.Collections;

public class revolve : MonoBehaviour 
{
    public float Radius = 5f;
	void Start ()
	{
	
	}
	
	void Update ()
	{
        gameObject.transform.position = new Vector3(Radius * Mathf.Sin(Time.fixedTime), Radius * Mathf.Cos(Time.fixedTime),gameObject.transform.position.z);
	}
}
