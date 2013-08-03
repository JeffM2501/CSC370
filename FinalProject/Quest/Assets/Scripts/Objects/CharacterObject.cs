using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterObject : MonoBehaviour
{
    public Character TheCharacter;

    protected Material RootMaterial = null;

    public void SetCharactr(Character c)
    {
        TheCharacter = c;
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
	
	}

    public void BuildMaterialLayers()
    {
        List<Material> mats = new List<Material>();

        foreach (SpriteManager.SpriteLayer layer in TheCharacter.GetSpriteLayers())
            mats.Add(layer.LayerMaterial);

        gameObject.renderer.materials = mats.ToArray();
    }
}
