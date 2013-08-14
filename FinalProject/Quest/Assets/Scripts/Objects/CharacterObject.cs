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
    public GameObject HitMesh = null;

    protected bool NeedRebuild = false;

    protected AnimationSequence Anims = null;

    protected Color OrigonalColor = Color.white;

    public Color HitFlashColor = Color.red;

    public Material DamageGraphoc = null;
    public Material FireSpellGraphic = null;
    public Material IceSpellGraphic = null;
    public Material DivineSpellGraphic = null;
    public Material GenericSpellGrpahic = null;

    public Material BaseMaterial = null;

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

    void Awake()
    {
  //      if (RootMaterial == null)
      //      RootMaterial = gameObject.renderer.materials[0];
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
                {
                    HitPlane.SetActive(false);
                    HitMesh.renderer.materials[0] = BaseMaterial;
                }
                InHit = false;
                Billboard.renderer.materials[0].color = OrigonalColor;
            }
        }

        if (Dying && gameObject.activeSelf)
        {
            float delta = Time.time - DeathTime;

            if (delta > DieSinkDelay)
            {
                float time = delta - DieSinkDelay;
                time *= 0.1f;

                transform.Translate(0, -1f * DieSinkSpeed * time, 0);

                if (delta > DeathDurration)
                    gameObject.SetActive(false);
            }
        }
	}

    public enum HitType
    {
        Physical,
        Fire,
        Ice,
        Divine,
        GenericSpell,
    }

    public void Hit(HitType hitType)
    {
        if (InHit || Dying)
            return;

        if (HitPlane != null)
        {
            HitPlane.SetActive(true);

            BaseMaterial = HitMesh.renderer.materials[0];

            switch (hitType)
            {
                case HitType.Divine:
                    if (DivineSpellGraphic != null)
                        HitMesh.renderer.material = DivineSpellGraphic;
                    break;

                case HitType.Physical:
                    if (DamageGraphoc != null)
                        HitMesh.renderer.material = DamageGraphoc;
                    break;

                case HitType.Fire:
                    if (FireSpellGraphic != null)
                        HitMesh.renderer.material = FireSpellGraphic;
                    break;

                case HitType.Ice:
                    if (IceSpellGraphic != null)
                        HitMesh.renderer.material = IceSpellGraphic;
                    break;

                case HitType.GenericSpell:
                    if (GenericSpellGrpahic != null)
                        HitMesh.renderer.material = GenericSpellGrpahic;
                    break;
            }
        }

      //  OrigonalColor = Billboard.renderer.materials[0].color;
      //  Billboard.renderer.materials[0].color = HitFlashColor;
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
