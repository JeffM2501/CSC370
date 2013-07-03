using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour 
{
    protected float Speed = 0.03f;
    protected float RotationSpeed = 0.5f;

	void Start ()
	{
        animation.wrapMode = WrapMode.Loop;
        animation["Idle"].layer = -1;
        animation["WalkForward"].layer = -1;
        animation["WalkBackward"].layer = -1;
        animation["ShootStraight"].wrapMode = WrapMode.Clamp;

        animation.AddClip(animation["ShootStraight"].clip, "ShootUpperBody");
        animation["ShootUpperBody"].AddMixingTransform(transform.Find("Reference/RightGun"));
        animation["ShootUpperBody"].AddMixingTransform(transform.Find("Reference/Hips/Spine"));
        animation["ShootUpperBody"].wrapMode = WrapMode.Clamp;

        animation.Stop();
	}
	
	void Update ()
	{
	    float translation = Input.GetAxis ("Vertical") * Speed;
	    float rotation = Input.GetAxis ("Horizontal") * RotationSpeed;

        bool walking = translation != 0;

	    if(translation > 0)
		     animation.CrossFade("WalkForward");
	    else if (translation < 0)
            animation.CrossFade("WalkBackward");
	    else
            animation.CrossFade("Idle");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Firing");
            animation.CrossFade(walking ? "ShootUpperBody" : "ShootStraight");
        }
	
	    transform.Translate (0, 0, translation);
	    transform.Rotate (0, rotation, 0);
	}
}
