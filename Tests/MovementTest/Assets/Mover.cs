using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
    public float TurnForce = 25;
    public float Thrust = 5;
    public float ReverseThrustPercent = 0.5f;

    float SlidingFriction = 0;

    bool Sliding = false;

    public WheelCollider[] DriveWheels;

	void Start ()
	{
        SlidingFriction = rigidbody.drag;
	}
	
	void Update ()
	{
        float turn = Input.GetAxis("Horizontal");
        rigidbody.AddTorque(Vector3.up * turn * TurnForce,ForceMode.Acceleration);

        float forward = Input.GetAxis("Vertical");
        float force = Thrust * forward;

        if (forward < 1)
            force *= ReverseThrustPercent;

      //  rigidbody.AddForce(transform.forward * force, ForceMode.Acceleration);
        foreach(WheelCollider wheel in DriveWheels)
            wheel.motorTorque = force;
 	}

    void OnCollisionEnter(Collision col)
    {
        Sliding = true;
     //   rigidbody.drag = SlidingFriction;
    }

    void OnCollisionExit(Collision col)
    {
        Sliding = false;
       // rigidbody.drag = SlidingFriction * 0.5f;
    }
}
