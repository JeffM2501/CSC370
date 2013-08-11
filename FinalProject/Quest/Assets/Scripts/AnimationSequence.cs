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
    public Directions CurrentDirection = Directions.South;
    
    protected AnimationSet CurrentAnimSet = null;

    protected bool forceFrame = false;

    public int CurrentFrame = -1;
    public float LastUpdateTime = 0;

    public int MidFrame = 0;

    public GameObject SpriteQuad = null;
    public Mesh TheMesh;

    public EventHandler AnimationComplete;

    public void SetGameObject(GameObject obj)
    {
        TheMesh = obj.GetComponent<MeshFilter>().mesh;
    }

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
// 
//                 v.Add(new Vector2(startX, 1 - startY));
//                 v.Add(new Vector2(startX + xOffset, 1 - startY - yOffset));
//                 v.Add(new Vector2(startX + xOffset, 1 - startY));
//                 v.Add(new Vector2(startX, 1 - startY - yOffset));


                v.Add(new Vector2(startX, 1 - startY - yOffset));
                v.Add(new Vector2(startX + xOffset, 1 - startY));
                v.Add(new Vector2(startX, 1 - startY));
                v.Add(new Vector2(startX + xOffset, 1 - startY - yOffset));

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

                v.Add(new Vector2(startX + xOffset, 1 - startY - yOffset));
                v.Add(new Vector2(startX, 1 - startY));
                v.Add(new Vector2(startX + xOffset, 1 - startY));
                v.Add(new Vector2(startX, 1 - startY - yOffset));

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

    public void Clear()
    {
        CurrentAnimSet = null;
        CurrentSequence = string.Empty;
    }

    public void SetSequence(string name)
    {
        if (CurrentAnimSet != null && name == CurrentAnimSet.Name)
            return;

        LastUpdateTime = -9999;

        if (FrameSets.ContainsKey(name))
            CurrentAnimSet = FrameSets[name];
        else if (FrameSets.ContainsKey("Idle"))
            CurrentAnimSet = FrameSets["Idle"];
        else
        {
            CurrentAnimSet = null;
            return;
        }

        CurrentSequence = CurrentAnimSet.Name;

        CurrentFrame = -1;
    }

    public void SetDirection(Directions dir)
    {
        if (dir == CurrentDirection)
            return;

        CurrentDirection = dir;
        forceFrame = true;
    }

    public AnimationFrameSet FrameSetForDir()
    {
        if (CurrentAnimSet.Frames.ContainsKey(CurrentDirection))
            return CurrentAnimSet.Frames[CurrentDirection];

        if (CurrentAnimSet.Frames.ContainsKey(Directions.None))
            return CurrentAnimSet.Frames[Directions.None];

        if (CurrentAnimSet.Frames.ContainsKey(Directions.South))
            return CurrentAnimSet.Frames[Directions.South];

        if (CurrentAnimSet.Frames.Count > 0)
        {
            foreach (AnimationFrameSet a in CurrentAnimSet.Frames.Values)
                return a;
        }
        return null;
    }

    int SetFrame( bool advance )
    {
        if (CurrentAnimSet == null)
            SetSequence(CurrentSequence);

        if (CurrentAnimSet == null)
            return 0;
        else
        {
            if (advance)
                CurrentFrame++;

            AnimationFrameSet set = FrameSetForDir();

            if (CurrentFrame >= set.Length)
            {
                if (AnimationComplete != null)
                    AnimationComplete(this, EventArgs.Empty);

                if (CurrentAnimSet.Looping)
                    CurrentFrame = 0;
                else if (CurrentAnimSet.EndAnimation != string.Empty)
                {
                    SetSequence(CurrentAnimSet.EndAnimation);
                    return SetFrame(advance);
                }
                else
                    CurrentFrame = set.Start + set.Length;
            }

            return set.Start + CurrentFrame;
        }
    }

    public void Update()
    {
        // see if we have to change frames

        int UVIndex = -1;

        if (LastUpdateTime + FrameTime < Time.time || forceFrame)
        {
            UVIndex = SetFrame(LastUpdateTime + FrameTime < Time.time);
            LastUpdateTime = Time.time;
        }
        else
            return;

        forceFrame = false;

        // apply UVs to object
        if (TheMesh != null && UVIndex >= 0)
            TheMesh.uv = FrameUVs[UVIndex];
    }
}

public class HominidAnimation : AnimationSequence
{
    public HominidAnimation(Texture image)
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
    public MonsterAnimation(Texture image)
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

public class MeeleWeponAnimation : AnimationSequence
{
    public MeeleWeponAnimation(Texture image)
    {
        Image = image;
        Init();
    }

    protected override void Init()
    {
        base.Init();
        ComputeFrames(6, 1);

        AddSequence("Attack", Directions.North, 0, 6, false, false);
        AddSequence("Attack", Directions.West, 0, 6, false, false);
        AddSequence("Attack", Directions.South, 0, 6, true, false);
        AddSequence("Attack", Directions.East, 0, 6, true, false);
    }
}

public class RangedWeaponAnimation : AnimationSequence
{
    public RangedWeaponAnimation(Texture image)
    {
        Image = image;
        Init();
    }

    protected override void Init()
    {
        base.Init();
        ComputeFrames(13, 1);

        AddSequence("Attack", Directions.North, 0, 13, false, false);
        AddSequence("Attack", Directions.West, 0, 13, false, false);
        AddSequence("Attack", Directions.South, 0, 13, true, false);
        AddSequence("Attack", Directions.East, 0, 13, true, false);
    }
}
