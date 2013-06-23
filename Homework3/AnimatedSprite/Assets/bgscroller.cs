using UnityEngine;
using System.Collections;

public class bgscroller : MonoBehaviour 
{
    public Vector2 UVSpeed = new Vector2(0, 0);
    public Vector2 UVOffset = new Vector2(0, 0);

	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

    void LateUpdate()
    {
        UVOffset += UVSpeed * Time.deltaTime;
        renderer.materials[0].SetTextureOffset("_MainTex", UVOffset);
    }
}
