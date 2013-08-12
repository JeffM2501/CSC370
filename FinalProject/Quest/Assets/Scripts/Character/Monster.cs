using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Monster : Character
{
    public bool Activated = false;

    public AI Smarts = null;

    public override void Init()
    {
        base.Init();

        HitSound = Resources.Load("Sounds/Monsters/zombie/zombie-hit1") as AudioClip;
        DieSound = Resources.Load("Sounds/Monsters/zombie/zombie-dying1") as AudioClip;

        if (Smarts == null)
            Smarts = new FightToDeath();
    }

    public override void Die()
    {
        base.Die();
        Smarts.Die();
    }
}
