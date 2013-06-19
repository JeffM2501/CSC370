using UnityEngine;
using System.Collections;

public class billboard : MonoBehaviour
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
        if (StayUpright)
           transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
	}
}
