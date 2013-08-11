using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BouncyPlane : MonoBehaviour 
{
    public float BouncyScale = 1.0f;
    public float Period = 2.0f;

    protected float StartY = 0;
	void Start ()
	{
        StartY = this.transform.position.y;
	}
	
	void Update ()
	{
        float newY = StartY + Mathf.Sin(Time.fixedTime * Period) * BouncyScale;

        this.transform.Translate(0, newY - this.transform.position.y, 0);
	}
}
