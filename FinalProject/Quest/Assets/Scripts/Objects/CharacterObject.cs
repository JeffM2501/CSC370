using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterObject : MonoBehaviour
{
    public Character TheCharacter;

    protected Material RootMaterial = null;

    public GameObject Billboard = null;

    protected bool NeedRebuild = false;

    protected AnimationSequence Anims = null;

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
	    
	}

	void Update ()
	{
        if (NeedRebuild && Billboard != null && TheCharacter != null)
            BuildMaterialLayers();
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
