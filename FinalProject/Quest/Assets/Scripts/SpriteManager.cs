using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpriteManager
{
    public class SpriteLayer
    {
        public string Name = string.Empty;
        public Texture LayerImage = null;
        public Color LayerColor = Color.white;
        public Material LayerMaterial = null;

        public SpriteLayer(Texture tex, Color c)
        {
            LayerImage = tex;
            LayerColor = c;
            LayerMaterial = new Material(Resources.Load("Resources/DefaultSpriteMaterial") as Material);
            LayerMaterial.color = c;
            LayerMaterial.SetTexture("_mainTexture", tex);
        }

        public bool Equals(Texture tex, Color c)
        {
           return LayerImage == tex && c == LayerColor;
        }
    }

    public List<SpriteLayer> LayerCache = new List<SpriteLayer>();

    public SpriteLayer GetLayer(Texture image, Color color )
    {
        foreach(SpriteLayer layer in LayerCache)
        {
            if (layer.Equals(image, color))
                return layer;
        }
        SpriteLayer l = new SpriteLayer(image, color);
        LayerCache.Add(l);
        return l;
    }
}
