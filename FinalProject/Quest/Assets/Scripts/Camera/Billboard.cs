using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour 
{
    public bool StayUpright = true;
    Camera LastCamera;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        if (Camera.current != null)
            LastCamera = Camera.current;

        if (LastCamera == null)
            return;

       
        transform.LookAt(LastCamera.transform);
        transform.RotateAroundLocal(Vector3.fwd, Mathf.PI);
        if (StayUpright)
            transform.eulerAngles = new Vector3(0, -transform.eulerAngles.y, transform.eulerAngles.z);

       
	}
}
