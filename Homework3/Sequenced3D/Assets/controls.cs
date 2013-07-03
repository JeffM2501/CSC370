using UnityEngine;
using System.Collections;

public class controls : MonoBehaviour 
{
	void Start ()
	{
        animation.Play("idle");
        animation["walk"].wrapMode = WrapMode.Loop;
	}
	
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            animation.Play("walk");
        else if (Input.GetKeyDown(KeyCode.Space))
            animation.Play("jump");
        animation.CrossFadeQueued("idle", 0.5f);
	}
}
