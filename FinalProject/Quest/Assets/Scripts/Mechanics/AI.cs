using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AI 
{
    public Monster TheCharacter = null;

    protected float SpawnTime = 0;

    public virtual void Activate(Monster character)
    {
        TheCharacter = character;
        SpawnTime = Time.time;
    }

    public void Update()
    {
        if (TheCharacter.Alive)
            Think();
    }

    public virtual void Think()
    {

    }

    public virtual void Die()
    {

    }

    protected virtual void MoveToTarget()
    {
        if (TheCharacter.Target == null)
            return;

        Vector3 toTarget = TheCharacter.Target.WorldObject.transform.position - TheCharacter.WorldObject.transform.position;

        float totalDist = Vector3.Magnitude(toTarget);
        toTarget.Normalize();

        float maxDist = TheCharacter.Speed * Time.deltaTime;

        TheCharacter.Move(toTarget);  
    }

    protected virtual void MoveAwayFromTarget()
    {
        if (TheCharacter.Target == null)
            return;

        Vector3 toTarget = TheCharacter.WorldObject.transform.position - TheCharacter.Target.WorldObject.transform.position;

        float totalDist = Vector3.Magnitude(toTarget);
        toTarget.Normalize();

        float maxDist = TheCharacter.Speed * Time.deltaTime;

        TheCharacter.Move(toTarget);
    }

    protected float DistToTarget()
    {
        if (TheCharacter.Target == null)
            return float.MaxValue;

        return Vector3.Distance(TheCharacter.Target.WorldObject.transform.position, TheCharacter.WorldObject.transform.position);
    }

    protected bool InAttackRange(SkillInstance skill)
    {
        return DistToTarget() <= skill.BaseSkill.Range;
    }
}

public class FightToDeath : AI
{
    public float Fercocity = 0.5f;

    public FightToDeath() : base()
    {

    }

    public FightToDeath(float ferocity)
        : base()
    {
        Fercocity = ferocity;
    }


    public override void Think()
    {
        if (!InAttackRange(TheCharacter.BasicAttackSkill))
            MoveToTarget();
        else
        {
            float aliveTime = Time.time - SpawnTime;
            if (aliveTime < TheCharacter.Initiative)
                return;
            if (TheCharacter.SkillUseable(TheCharacter.BasicAttackSkill) && UnityEngine.Random.value < Fercocity)
                TheCharacter.BasicAttack();
        }
    }
}

public class FightOrFlight : AI
{
    public float Fercocity = 0.5f;
    public float FlightLimit = 0.25f;

    public FightOrFlight() : base()
    {

    }

    public FightOrFlight(float ferocity, float healthLimit)
        : base()
    {
        Fercocity = ferocity;
        FlightLimit = healthLimit;
    }

    public override void Think()
    {
        bool inRange = InAttackRange(TheCharacter.BasicAttackSkill);
        bool flight = TheCharacter.GetHealthParam() <= FlightLimit;

        if (!inRange && !flight)
            MoveToTarget();
        else
        {
            if (flight)
                MoveAwayFromTarget();
            else
            {
                float aliveTime = Time.time - SpawnTime;
                if (aliveTime < TheCharacter.Initiative)
                    return;
                if (TheCharacter.SkillUseable(TheCharacter.BasicAttackSkill) && UnityEngine.Random.value < Fercocity)
                    TheCharacter.BasicAttack();
            }
        }
    }
}

