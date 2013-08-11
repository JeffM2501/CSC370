using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterObject : MonoBehaviour
{
    public Character TheCharacter;

    protected Material RootMaterial = null;

    public GameObject Billboard = null;

    public GameObject HitPlane = null;

    protected bool NeedRebuild = false;

    protected AnimationSequence Anims = null;

    protected Color OrigonalColor = Color.white;

    public Color HitFlashColor = Color.red;

    bool InHit = false;
    float LastHitStart = 0;
    public float HitFlashDurration = 1;

    public void SetCharacter(Character c)
    {
        TheCharacter = c;
        NeedRebuild = true;

        Anims = TheCharacter.Anims;
        Anims.SetGameObject(Billboard);
    }

    void Alive()
    {
        if (RootMaterial == null)
            RootMaterial = gameObject.renderer.materials[0];
    }

	void Start ()
	{
        if (HitPlane != null)
            HitPlane.SetActive(false);
	}

	void Update ()
	{
        if (NeedRebuild && Billboard != null && TheCharacter != null)
            BuildMaterialLayers();

        if (InHit)
        {
            if (LastHitStart + HitFlashDurration < Time.time)
            {
                if (HitPlane != null)
                    HitPlane.SetActive(false);
                InHit = false;
                Billboard.renderer.materials[0].color = OrigonalColor;
            }
        }
	}

    public void Hit()
    {
        if (InHit)
            return;
        if (HitPlane != null)
            HitPlane.SetActive(true);

        OrigonalColor = Billboard.renderer.materials[0].color;
        Billboard.renderer.materials[0].color = HitFlashColor;
        InHit = true;
        LastHitStart = Time.time;
    }

    public void BuildMaterialLayers()
    {
        if (Billboard == null)
            return;

        List<Material> mats = new List<Material>();

        foreach (SpriteManager.SpriteLayer layer in TheCharacter.GetSpriteLayers())
            mats.Add(layer.LayerMaterial);
        Billboard.renderer.materials = mats.ToArray();

        NeedRebuild = false;
    }
}
