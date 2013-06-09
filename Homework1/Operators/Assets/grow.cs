using UnityEngine;
using System.Collections;

public class grow : MonoBehaviour 
{
    public float GrowthRate = 1.0f;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        Vector3 newScale = new Vector3(gameObject.transform.localScale.x + (GrowthRate * Time.deltaTime),
            gameObject.transform.localScale.y + (GrowthRate * Time.deltaTime),
            gameObject.transform.localScale.z + (GrowthRate * Time.deltaTime));
        gameObject.transform.localScale = newScale;
	}
}
