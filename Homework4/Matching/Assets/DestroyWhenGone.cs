using UnityEngine;
using System.Collections;

public class DestroyWhenGone : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
