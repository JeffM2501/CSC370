using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleManager  
{
    public Player ThePlayer = null;
    public List<Character> Mobs = null;

    protected AudioClip MissSound = null;
    protected AudioClip HitSound = null;

    public void Init(Player player, List<Character> mobs)
    {
        ThePlayer = player;
        Mobs = mobs;
    }

    protected void Miss(Character attacker)
    {
        if (MissSound == null)
            MissSound = Resources.Load("Sounds/sharpknife-miss1") as AudioClip;

        if (attacker.WorldObject.audio != null)
            attacker.WorldObject.audio.PlayOneShot(MissSound);
    }

    protected void Hitt(Character defender)
    {
        if (HitSound == null)
            HitSound = Resources.Load("Sounds/longsword-hit1") as AudioClip;

        if (defender.WorldObject.audio != null)
            defender.WorldObject.audio.PlayOneShot(HitSound);
    }

    public void PhysicalAttack(Character attacker, Character defender, float hitChance, int skill, float attackRange, int minDamage, int maxDamage)
    {
        if (defender == null || Vector3.Distance(attacker.WorldObject.transform.position, defender.WorldObject.transform.position) > attackRange)
            return;

        if (UnityEngine.Random.value <= defender.DodgeBonus)
        {
            Debug.Log("Attak Dodged");
            Miss(attacker);
            return;
        }

        int roll = UnityEngine.Random.Range(1, 20);
        int hitRoll = roll + skill;

        int targetNum = (int)(20 * hitChance);

        Debug.Log("Hit Roll = " + hitRoll.ToString() + " vs " + targetNum.ToString());

        if (hitRoll < targetNum)
        {
            Debug.Log("Miss");
            Miss(attacker);
            return;
        }
        else
            Hitt(defender);

        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        if (roll >= 20 - attacker.CritBonus)
            damage = maxDamage * 2;
        else
        {
            damage += hitRoll - targetNum;

            float armorParam = 1;
            if (defender.ArmorValue > 0)
                armorParam = Mathf.Min(damage / defender.ArmorValue, 1.0f);

            Debug.Log("Hit Damage = Raw " + damage.ToString() + " Param " + armorParam.ToString());
            damage =  (int)(damage * armorParam);
        }

        defender.TakeDamage(damage);

        if (defender.Damage >= defender.HitPoints)
            attacker.XP += defender.XP;
    }


    public void SpellAttack(Character attacker, Character defender, float attackRange, int damage)
    {
        SpellAttack(attacker, defender, CharacterObject.HitType.GenericSpell, attackRange, damage);
    }
    
    public void SpellAttack(Character attacker, Character defender, CharacterObject.HitType hitType, float attackRange, int damage)
    {
        if (defender == null || Vector3.Distance(attacker.WorldObject.transform.position, defender.WorldObject.transform.position) > attackRange)
            return;

        if (UnityEngine.Random.value <= defender.DodgeBonus)
            return;

        float armorParam = 1;
        if (defender.ArmorValue > 0)
            armorParam = Mathf.Min(damage / defender.ArmorValue, 1.0f);

        damage = (int)(damage * armorParam);

        defender.TakeDamage(damage, hitType);

        if (defender.Damage >= defender.HitPoints)
            attacker.XP += defender.XP;
    }

    public void ActivateMonster(Monster mob)
    {
        mob.Activated = true;
        mob.Target = ThePlayer;
        mob.AnimateTo(Skill.ActiveAnimationTypes.Casting);
        // start the AI
        Debug.Log("Monster " + mob.Name + " activated");

        mob.Smarts.Activate(mob as Monster);
    }

    public void Update()
    {
        if (ThePlayer == null || !ThePlayer.Alive)
            return;

        foreach (Monster monster in Mobs)
        {
            if (!monster.Alive)
                continue;

            if (monster.Activated)
                monster.Smarts.Update();
            else
            {
                float dist = Vector3.Distance(monster.WorldObject.transform.position, ThePlayer.WorldObject.transform.position);

                if (dist <= monster.PerceptionRange)
                {
                    Vector3 offset = new Vector3(0,0.5f,0);
                    Vector3 dir = ThePlayer.WorldObject.transform.position - monster.WorldObject.transform.position;
                    dir.Normalize();

                    Ray ray = new Ray(monster.WorldObject.transform.position + offset, dir);

                    Debug.DrawRay(ray.origin, ray.direction * monster.PerceptionRange, Color.green, 0.001f);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, monster.PerceptionRange))
                    {
                        if (hit.transform.gameObject == ThePlayer.WorldObject)
                            ActivateMonster(monster);
                    }
                }
            }
        }
    }
}
