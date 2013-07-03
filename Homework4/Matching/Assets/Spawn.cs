using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour 
{
    public GameObject Ball;

    public Material[] MaterialList;// = new Material[];

	void Start ()
	{
        int level = PlayerPrefs.GetInt("Level");

        float y = 0;

        if (level == 2)
            y = 2;

        GameObject.Find("Cube").transform.Translate(0,y,0);
 
	}
	
	void Update ()
	{
        int level = 1;
        if (PlayerPrefs.GetInt("Level") > 0)
            level = PlayerPrefs.GetInt("Level");

        if (Random.Range(0,200/level) < 1)
        {
            GameObject sphere = Instantiate(Ball, new Vector3(Random.Range(-6.0f, 6.0f), 8.0f, 0), Quaternion.identity) as GameObject;

            sphere.renderer.material = MaterialList[Random.Range(0,MaterialList.Length)];
            sphere.renderer.material.shader = Shader.Find("Diffuse");
        }
  
	}
}
