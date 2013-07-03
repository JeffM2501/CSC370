using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpriteDetails
{
    public int PixelHeight;
    public int PixelWidth;

}

[System.Serializable]
public class AnimFrame
{
    public int StartFrame;
    public int NumberOfFrames;

     [HideInInspector]
    public string Name = string.Empty;

     public AnimFrame(string name)
    {
        Name = name;
    }
}

public class SpriteManager : MonoBehaviour
{
    float NextFrame = 0;
    float TimeBetweenFrames = 0;
    float FrameRate = 5.0f;
    Mesh TheMesh;
    Texture2D Texture;

    public SpriteDetails SingleSpriteDimensions = new SpriteDetails();

    public AnimFrame Idle = new AnimFrame("IDLE");
    public AnimFrame WalkLeft = new AnimFrame("WALK_LEFT");
    public AnimFrame WalkRight = new AnimFrame("WALK_RIGHT");
    public AnimFrame WalkForward = new AnimFrame("WALK_FORWARD");
    public AnimFrame WalkBack = new AnimFrame("WALK_BACK");

    protected AnimFrame CurrentAnimation;
    protected int CurrentFrame = 0;

    void SetAnimation( string name)
    {
        if (CurrentAnimation != null && CurrentAnimation.Name == name)
            return;

        CurrentFrame = 0;
        switch (name)
        {
            case "WALK_LEFT":
                CurrentAnimation = WalkLeft;
                break;

            case "WALK_RIGHT":
                CurrentAnimation = WalkRight;
                break;

            case "WALK_FORWARD":
                CurrentAnimation = WalkForward;
                break;
            
            case "WALK_BACK":
                CurrentAnimation = WalkBack;
                break;

            case "IDLE":
                CurrentAnimation = Idle;
                break;
        }
    }

    void UpdateSprite()
    {
        int currentRealFrame = CurrentAnimation.StartFrame + CurrentFrame;

        Vector2[] uvs = new Vector2[4];

        Vector2 start = new Vector2((SingleSpriteDimensions.PixelWidth * currentRealFrame)/Texture.width, (Texture.height - (SingleSpriteDimensions.PixelHeight * currentRealFrame))/Texture.height);
        Vector2 end = new Vector2((SingleSpriteDimensions.PixelWidth * (currentRealFrame + 1))/Texture.width, (Texture.height - (SingleSpriteDimensions.PixelHeight * (currentRealFrame + 1))/Texture.height));

        uvs[0] = new Vector2(start.x,end.y);
        uvs[1] = new Vector2(start.x,start.y);
        uvs[2] = new Vector2(end.x,start.y);
        uvs[3] = new Vector2(end.x,end.y);

        TheMesh.uv = uvs;
        CurrentFrame++;
        if (CurrentFrame > CurrentAnimation.NumberOfFrames)
            CurrentFrame = 0;
    }

	void Start ()
	{
        TheMesh = (GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
        Texture = renderer.material.mainTexture as Texture2D;
        renderer.material.shader = Shader.Find("Sprite");
        TimeBetweenFrames = 1.0f / FrameRate;

        SetAnimation("IDLE");
	}

    void MoveAnim(float x, float z, string name)
    {
        transform.Translate(x, z, 0);
        SetAnimation(name);
    }

	void Update () 
	{
        if (Input.GetKey(KeyCode.UpArrow))
            MoveAnim(0, 1, "WALK_BACK");
        else if (Input.GetKey(KeyCode.DownArrow))
            MoveAnim(0, -1, "WALK_FORWARD");
        else if (Input.GetKey(KeyCode.LeftArrow))
            MoveAnim(-1, 0, "WALK_LEFT");
        else if (Input.GetKey(KeyCode.RightArrow))
            MoveAnim(1, 0, "WALK_RIGHT");
        else
            SetAnimation("IDLE");
	}

    void LateUpdate()
    {
        if (Time.time > NextFrame)
        {
            NextFrame = Time.time + TimeBetweenFrames;
            UpdateSprite();
        }
    }
}



