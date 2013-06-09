using UnityEngine;
using System.Collections;

public class resize : MonoBehaviour 
{
    public Vector3 objScale = new Vector3(2f, 0.2f, 0.5f);

	void Start ()
	{
        gameObject.transform.localScale = objScale;
	}
	
	void Update ()
	{
	
	}
}
