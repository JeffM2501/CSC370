using UnityEngine;
using System.Collections;

public class animatedTexture : MonoBehaviour
 {
	void Start ()
	{
	
	}

	void Update () 
	{
	
	}

    public Vector2 UVSpeed = new Vector2(0, 1);
    public Vector2 UVOffset = Vector2.zero;

    void LateUpdate()
    {
        UVOffset += UVSpeed * Time.deltaTime;
        this.renderer.materials[0].SetTextureOffset("_MainTex", UVOffset);
    }
}
