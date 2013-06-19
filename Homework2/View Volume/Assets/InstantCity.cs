using UnityEngine;
using System.Collections;

public class InstantCity : MonoBehaviour
 {
    public GameObject Building;
	void Start ()
	{
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Vector3 pos = new Vector3(i * Random.Range(100, 200), 0, j * Random.Range(100, 200));

                GameObject b = Instantiate(Building, pos, Quaternion.identity) as GameObject;
            }
        }
	}

    void Update() 
	{
	
	}
}
