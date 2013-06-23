using UnityEngine;
using System.Collections;

public class AnimatedSprite : MonoBehaviour
{
    public Texture2D[] Textures;
    public Texture2D Atlas;
    public float FrameRate = 15.0f;

    public int CurrentFrame = 0;
    public float TotalWidth;
    public float NextFrame = 0;
    public float TimeBetweenFrames = 0;

    public int offset = 0;

    public Color LastColor = Color.white;

	void Start ()
	{
        if (Textures.Length > 0)
        {
            print(Textures.Length.ToString());
            TotalWidth = Textures[0].width * Textures.Length;

            Atlas = new Texture2D((int)TotalWidth, Textures[0].height,TextureFormat.ARGB32,false);

            gameObject.renderer.material.mainTexture = Atlas;
            gameObject.renderer.material.color = Color.white;
            gameObject.renderer.material.shader = Shader.Find("Sprite");

            offset = 0;

            foreach (Texture2D texture in Textures)
            {
                Atlas.SetPixels(offset, 0, texture.width, Atlas.height, texture.GetPixels(0, 0, texture.width, Atlas.height));
                offset += texture.width;
            }
            Atlas.Apply();

            renderer.material.mainTextureOffset = new Vector2(0, 0);
            renderer.material.mainTextureScale = new Vector2(1.0f / Textures.Length, 1);

            TimeBetweenFrames = 1.0f / FrameRate;
        }
	}

	void Update ()
	{
	
	}

    void LateUpdate()
    {
        if (Time.time > NextFrame)
        {
            renderer.material.mainTextureOffset = new Vector2(CurrentFrame * Textures[0].width / TotalWidth, 0);
            NextFrame = Time.time + TimeBetweenFrames;
            CurrentFrame++;
            if (CurrentFrame >= Textures.Length)
                CurrentFrame = 0;
        }
    }
}
