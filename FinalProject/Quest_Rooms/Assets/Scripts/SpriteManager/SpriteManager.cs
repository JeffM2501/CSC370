using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour
{
    public int HorizontalFarames = 1;
    public int VerticalFrames = 1;

    protected Vector2 SingleFrameOffsets = Vector2.zero;

    Mesh TheMesh;

	void Start ()
	{
        TheMesh = (GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;

        float u = 1.0f / HorizontalFarames;
        float v = 1.0f / VerticalFrames;

        SingleFrameOffsets = new Vector2(u, v);

        SetFrame(9);
	}

	void Update ()
	{
	
	}

    public void SetFrame(int frame)
    {
        int frameY = frame / HorizontalFarames;
        int frameX = frame - (HorizontalFarames * frameY);

      //  frameY = VerticalFrames - frameY;

        float startU = frameX * SingleFrameOffsets.x;
       // float startV = (frameY * SingleFrameOffsets.y);
        float startV = 1.0f - (frameY * SingleFrameOffsets.y);

        float endU = startU + SingleFrameOffsets.x;
        float endV = startV - SingleFrameOffsets.y;

        print("Frame " + frameX.ToString() + " " + frameY.ToString());
        print("Start " + startU.ToString() + " " + startV.ToString());
        print("End " + endU.ToString() + " " + endV.ToString());

        Vector2[] uvs = new Vector2[4];

        uvs[0] = new Vector2(startU, endV);
        uvs[1] = new Vector2(endU, startV);
        uvs[2] = new Vector2(endU, endV);
        uvs[3] = new Vector2(startU, startV);

        TheMesh.uv = uvs;
    }
}
