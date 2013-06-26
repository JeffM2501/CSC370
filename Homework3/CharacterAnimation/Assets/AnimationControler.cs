using UnityEngine;
using System.Collections;

public class AnimationControler : MonoBehaviour 
{
    public float WalkSpeed = 1;
    public float RunSpeed = 2;
    public float TurnSpeed = 20;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        string animation = "Idle";

        string movePrefix = "Walk";
        bool run = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.CapsLock);
            
        if (run)
            movePrefix = "Run";

        float speed = run ? RunSpeed : WalkSpeed;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(this.transform.forward * (speed * Time.deltaTime));
            animation = movePrefix + "Forward";
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(this.transform.forward * (speed * -1f * Time.deltaTime));
            animation = movePrefix + "Backward";
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up,-Time.deltaTime * TurnSpeed);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up,Time.deltaTime * TurnSpeed);

        this.animation.CrossFade(animation);
	}

    void Awake()
    {
        this.animation.wrapMode = WrapMode.Loop;
        this.animation.Play("Idle");
    }
}
