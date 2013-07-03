using UnityEngine;
using System.Collections;

public class CountBalls : MonoBehaviour 
{
    public Material ColorMatch;

    public int Score = 0;
    public int TotalBalls = 0;

	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.renderer.material.color == ColorMatch.color)
            Score++;

        TotalBalls++;
    }
}
