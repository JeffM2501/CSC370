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

        public string MatName = string.Empty;

        public SpriteLayer(string mat, Color c)
        {
            MatName = mat;
            LayerColor = c;

            Material baseMat = Resources.Load(mat) as Material;
            if (baseMat == null)
            {
                Debug.Log("Unable to load material " + mat);
                return;
            }

            LayerMaterial = new Material(baseMat);
            
            LayerImage = LayerMaterial.mainTexture;
            LayerMaterial.name += c.ToString();
            LayerMaterial.color = c;
        }

        public bool Equals(string mat, Color c)
        {
            return MatName == mat && c == LayerColor;
        }
    }

    public List<SpriteLayer> LayerCache = new List<SpriteLayer>();

    public SpriteLayer GetLayer(string mat, Color color )
    {
        foreach(SpriteLayer layer in LayerCache)
        {
            if (layer.Equals(mat, color))
                return layer;
        }
        SpriteLayer l = new SpriteLayer(mat, color);
        LayerCache.Add(l);
        return l;
    }
}
