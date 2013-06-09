using UnityEngine;
using System.Collections;

public class stackedSpheres : MonoBehaviour 
{
    

	void Start ()
	{
	    GameObject anObject = null;

        int rows = 9;
        int cols = 9;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                anObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                anObject.transform.Translate(col * 2f, row * 2f, 0);
            }
        }
	}

    void Update()
	{
	
	}
}
