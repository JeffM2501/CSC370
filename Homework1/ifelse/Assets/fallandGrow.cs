using UnityEngine;
using System.Collections;

public class fallandGrow : MonoBehaviour 
{
    public float Speed = 4f;
    public float GrowthRate = 0.5f;
    public int GroundLevel = -4;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        if (gameObject.transform.position.y > GroundLevel)
        {
            gameObject.transform.Translate(Vector3.down * (Speed * Time.deltaTime));
        }
        else
        {
            Vector3 newScale = new Vector3( gameObject.transform.localScale.x + (GrowthRate * Time.deltaTime),
                                            gameObject.transform.localScale.y + (GrowthRate * Time.deltaTime),
                                            gameObject.transform.localScale.z + (GrowthRate * Time.deltaTime));
            gameObject.transform.localScale = newScale;
        }
	}
}
