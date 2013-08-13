using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraJiggle : MonoBehaviour
 {
    public float Period = 1;
    public float Rotation = 25;

    protected float StartAngle = 0;

	void Start () 
	{
        StartAngle = transform.rotation.eulerAngles.y;
	}

	void Update ()
	{
        float newAngle = StartAngle + Mathf.Sin(Time.time * Period) * Rotation;

        float oldAngle = transform.rotation.eulerAngles.y;
        transform.Rotate(0, newAngle - oldAngle, 0);

    //    this.gameObject.transform.rotation.SetEulerAngles();

   //     Debug.Log(newAngle);
	}
}
