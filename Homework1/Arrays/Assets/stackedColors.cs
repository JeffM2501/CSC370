using UnityEngine;
using System.Collections;

public class stackedColors : MonoBehaviour 
{
    GameObject[] Objects = new GameObject[9];

	void Start ()
	{
        for (int i = 0; i < Objects.Length; i++)
        {
            Objects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Objects[i].transform.Translate(0, i + 1f, 0);
        }
	}
    
    void Update()
	{
        foreach (GameObject o in Objects)
            o.renderer.material.color = new Color(Random.RandomRange(0f, 1f), Random.RandomRange(0f, 1f), Random.RandomRange(0f, 1f));
	}
}
