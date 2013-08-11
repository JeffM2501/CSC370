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

    protected bool Dying = false;
    public float DeathDurration = 4f;
    protected float DeathTime = 0;
    public float DieSinkSpeed = 2.0f;
    public float DieSinkDelay = 2;

    public void SetCharacter(Character c)
    {
        TheCharacter = c;
        NeedRebuild = true;

        Anims = TheCharacter.Anims;
        Anims.SetGameObject(Billboard);

        c.LayersChanged += new GameState.EventCallback(Character_LayersChanged);
    }

    void Character_LayersChanged(object sender, EventArgs args)
    {
        NeedRebuild = true;
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

        if (Dying && gameObject.activeSelf)
        {
            float delta = Time.time - DeathTime;

            if (delta > DieSinkDelay)
            {
                transform.Translate(0,-1f * DieSinkSpeed * Time.deltaTime, 0);

                if (delta > DeathDurration)
                    gameObject.SetActive(false);
            }
        }
	}

    public void Hit()
    {
        if (InHit || Dying)
            return;
        if (HitPlane != null)
            HitPlane.SetActive(true);

        OrigonalColor = Billboard.renderer.materials[0].color;
        Billboard.renderer.materials[0].color = HitFlashColor;
        InHit = true;
        LastHitStart = Time.time;
    }

    public void Die()
    {
        if (Dying || !gameObject.activeSelf)
            return;

        Dying = true;
        DeathTime = Time.time;
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
