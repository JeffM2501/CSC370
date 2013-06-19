using UnityEngine;
using System.Collections;

public class orbit : MonoBehaviour
{
    // default path
    public float Radius = 30;
    public float Speed = 3;

    // optional turning angle
    public bool UseLook = false;
    public Transform LookTarget;

    // data
    private Vector3 StartPostion; 
    private float x = 0;
    private float z = 0;

	// start where we are
	void Start ()
    {
        StartPostion = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}
	
	// spin us right round
	void Update () 
    {
        x = Radius * Mathf.Sin(Time.fixedTime * Speed) + StartPostion.x;
        z = Radius * Mathf.Cos(Time.fixedTime * Speed) + StartPostion.z;

        this.transform.position = new Vector3(x, StartPostion.y, z);

        if (UseLook)
            this.transform.LookAt(LookTarget);
	}
}
