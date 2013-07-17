using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider RearLeft;
    public WheelCollider RearRight;

	void Start ()
	{
	
	}

	void Update ()
	{
        float speed = 10;

        float forward = Input.GetAxis("Vertical");

        FrontLeft.motorTorque = speed * forward;
        FrontRight.motorTorque = speed * forward;

        RearLeft.motorTorque = speed * forward;
        RearRight.motorTorque = speed * forward;

        print(forward.ToString());
	}
}
