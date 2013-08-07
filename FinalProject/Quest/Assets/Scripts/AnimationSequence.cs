using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimationSequence
{
    public float FrameTime = 1.0f / 15.0f;
    public Texture Image;
    protected Vector2 ImageSize = Vector2.zero;

    public List<Vector2[]> FrameUVs = new List<Vector2[]>();
 
    public class AnimationFrameSet
    {
        public int Start = 0;
        public int Length = 0;
    }

    public enum Directions
    {
        None,
        North,
        South,
        East,
        West,
    }

    public class AnimationSet
    {
        public bool Looping = true;
        public string EndAnimation = "Idle";

        public string Name = string.Empty;
        public Dictionary<Directions,AnimationFrameSet> Frames = new Dictionary<Directions,AnimationFrameSet>();

        public AnimationSet(string name)
        {
            Name = name;
        }
    }

    public Dictionary<string, AnimationSet> FrameSets = new Dictionary<string, AnimationSet>();

    public string CurrentSequence = string.Empty;
    public Directions CurrentDirection = Directions.None;
    
    protected AnimationSet CurrentAnimSet = null;

    public int CurrentFrame = -1;
    public float LastUpdateTime = 0;

    public int MidFrame = 0;

    public GameObject SpriteQuad = null;

    protected virtual void Init()
    {
        ImageSize = new Vector2(Image.width, Image.height);

    }

    protected virtual void ComputeFrames(int xFrames, int yFrame)
    {
        float xOffset = 1.0f / xFrames;
        float yOffset = 1.0f / yFrame;

        for (int y = 0; y < yFrame; y++)
        {
            for (int x = 0; x < xFrames; x++)
            {
                List<Vector2> v = new List<Vector2>();

                float startX = x * xOffset;
                float startY = y * yOffset;
                v.Add(new Vector2(startX, startY));
                v.Add(new Vector2(startX + xOffset, startY));
                v.Add(new Vector2(startX + xOffset, startY + yOffset));
                v.Add(new Vector2(startX, startY + yOffset));

                FrameUVs.Add(v.ToArray());
            }
        }

        MidFrame = FrameUVs.Count;

        for (int y = 0; y < yFrame; y++)
        {
            for (int x = 0; x < xFrames; x++)
            {
                List<Vector2> v = new List<Vector2>();

                float startX = x * xOffset;
                float startY = y * yOffset;
                v.Add(new Vector2(startX + xOffset, startY));
                v.Add(new Vector2(startX, startY));
                v.Add(new Vector2(startX, startY + yOffset));
                v.Add(new Vector2(startX + xOffset, startY + yOffset));

                FrameUVs.Add(v.ToArray());
            }
        }
    }

    public void AddSequence(string name, Directions dir, int start, int len, bool flip, bool loop)
    {
        if (!FrameSets.ContainsKey(name))
            FrameSets.Add(name, new AnimationSet(name));

        AnimationFrameSet set = new AnimationFrameSet();
        set.Length = len;
        set.Start = flip ? start + MidFrame : start;

        if (FrameSets[name].Frames.ContainsKey(dir))
            FrameSets[name].Frames[dir] = set;
        else
            FrameSets[name].Frames.Add(dir, set);

        FrameSets[name].Looping = loop;
    }

    public void Update()
    {

    }
}

public class HominidAnimation : AnimationSequence
{
    public HominidAnimation(Texture2D image)
    {
        Image = image;
        Init();
    }

    protected override void Init()
    {
        base.Init();
        ComputeFrames(19, 4);

        AddSequence("Walk", Directions.North, 0, 9, false, true);
        AddSequence("Walk", Directions.West, 9, 9, false, true);
        AddSequence("Walk", Directions.South, 18, 9, false, true);
        AddSequence("Walk", Directions.East, 27, 9, false, true);

        AddSequence("Idle", Directions.North, 0, 0, false, true);
        AddSequence("Idle", Directions.West, 9, 0, false, true);
        AddSequence("Idle", Directions.South, 18, 0, false, true);
        AddSequence("Idle", Directions.East, 27, 0, false, true);

        AddSequence("HandAttack", Directions.North, 36, 9, false, false);
        AddSequence("HandAttack", Directions.West, 36, 9, false, false);
        AddSequence("HandAttack", Directions.South, 36, 9, true, false);
        AddSequence("HandAttack", Directions.East, 36, 9, true, false);

        AddSequence("RangedAttack", Directions.North, 45, 18, false, false);
        AddSequence("RangedAttack", Directions.West, 45, 18, false, false);
        AddSequence("RangedAttack", Directions.South, 45, 18, true, false);
        AddSequence("RangedAttack", Directions.East, 45, 18, true, false);

        AddSequence("Cast", Directions.North, 63, 9, false, false);
        AddSequence("Cast", Directions.West, 63, 9, false, false);
        AddSequence("Cast", Directions.South, 63, 9, true, false);
        AddSequence("Cast", Directions.East, 63, 9, true, false);

        AddSequence("Dying", Directions.None, 63, 9, true, false);
        FrameSets["Dying"].EndAnimation = "Dead";

        AddSequence("Dead", Directions.None, 75, 0, false, false);
    }
}

public class MonsterAnimation : AnimationSequence
{
    public MonsterAnimation(Texture2D image)
    {
        Image = image;
        Init();
    }

    protected override void Init()
    {
        base.Init();
        ComputeFrames(3, 4);

        AddSequence("Walk", Directions.North, 0, 3, false, true);
        AddSequence("Walk", Directions.West, 3, 3, false, true);
        AddSequence("Walk", Directions.South, 6, 3, false, true);
        AddSequence("Walk", Directions.East, 9, 3, false, true);

        AddSequence("Idle", Directions.North, 0, 3, false, true);
        AddSequence("Idle", Directions.West, 3, 3, false, true);
        AddSequence("Idle", Directions.South, 6, 3, false, true);
        AddSequence("Idle", Directions.East, 9, 3, false, true);


        AddSequence("HandAttack", Directions.North, 0, 3, false, true);
        AddSequence("HandAttack", Directions.West, 3, 3, false, true);
        AddSequence("HandAttack", Directions.South, 6, 3, false, true);
        AddSequence("HandAttack", Directions.East, 9, 3, false, true);


        AddSequence("RangedAttack", Directions.North, 0, 3, false, true);
        AddSequence("RangedAttack", Directions.West, 3, 3, false, true);
        AddSequence("RangedAttack", Directions.South, 6, 3, false, true);
        AddSequence("RangedAttack", Directions.East, 9, 3, false, true);


        AddSequence("Cast", Directions.North, 0, 3, false, true);
        AddSequence("Cast", Directions.West, 3, 3, false, true);
        AddSequence("Cast", Directions.South, 6, 3, false, true);
        AddSequence("Cast", Directions.East, 9, 3, false, true);

        AddSequence("Dying", Directions.None, 6, 3, true, false);
        FrameSets["Dying"].EndAnimation = "Dead";

        AddSequence("Dead", Directions.None, 3, 0, false, false);
    }
}
