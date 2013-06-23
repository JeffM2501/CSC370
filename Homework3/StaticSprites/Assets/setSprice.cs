using UnityEngine;
using System.Collections;

public class setSprice : MonoBehaviour
{
    public Vector2 StartPixel;
    public Vector2 EndPixel;


	void Start ()
	{
        Mesh mesh = (GetComponent("MeshFilter") as MeshFilter).mesh;

        Vector2[] UVs = new Vector2[4];

        Texture2D texture = renderer.material.mainTexture as Texture2D;

        renderer.material.shader = Shader.Find("Sprite");

        UVs[0] = new Vector2(StartPixel.x / texture.width, (texture.height - EndPixel.y) / texture.height);
        UVs[1] = new Vector2(StartPixel.x / texture.width, (texture.height - StartPixel.y) / texture.height);
        UVs[2] = new Vector2(EndPixel.x / texture.width, (texture.height - StartPixel.y) / texture.height);
        UVs[3] = new Vector2(EndPixel.x / texture.width, (texture.height - EndPixel.y) / texture.height);

        mesh.uv = UVs;
	}

	void Update ()
	{
	
	}
}
