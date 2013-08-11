using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleManager  
{
    public Player ThePlayer = null;
    public List<Character> Mobs = null;

    public void Init(Player player, List<Character> mobs)
    {
        ThePlayer = player;
        Mobs = mobs;
    }

    public void PhysicalAttack(Character attacker, Character defender, float hitChance, float attackRange, int minDamage, int maxDamage)
    {
        if (Vector3.Distance(attacker.WorldObject.transform.position, defender.WorldObject.transform.position) > attackRange)
            return;

        if (UnityEngine.Random.value <= defender.DodgeBonus)
            return;

        int roll = UnityEngine.Random.Range(1, 20);
        int hitRoll = roll + attacker.AttackValue;

        int targetNum = (int)(20 * hitChance);

        if (hitRoll < targetNum)
            return;

        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        if (roll >= 20 - attacker.CritBonus)
            damage = maxDamage * 2;
        else
        {
            damage += hitRoll - targetNum;

            float armorParam = Mathf.Min(damage / defender.ArmorValue, 1.0f);

            damage =  (int)(damage * armorParam);
        }

        defender.TakeDamage(damage);
    }

    public void SpellAttack(Character attacker, Character defender, float attackRange, int damage)
    {
        if (Vector3.Distance(attacker.WorldObject.transform.position, defender.WorldObject.transform.position) > attackRange)
            return;

        if (UnityEngine.Random.value <= defender.DodgeBonus)
            return;

        float armorParam = Mathf.Min(damage / defender.ArmorValue, 1.0f);

        damage = (int)(damage * armorParam);

        defender.TakeDamage(damage);
    }

    public void Update()
    {
        foreach (Monster monster in Mobs)
        {
            if (!monster.Alive)
                continue;

            if (monster.Activated)
            {
                // run the AI
            }
            else
            {
                float dist = Vector3.Distance(monster.WorldObject.transform.position, ThePlayer.WorldObject.transform.position);

                if (dist <= monster.PerceptionRange)
                {
                    Ray ray = new Ray(monster.WorldObject.transform.position, monster.WorldObject.transform.position - ThePlayer.WorldObject.transform.position);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, monster.PerceptionRange))
                    {
                        if (hit.transform.gameObject == ThePlayer.WorldObject)
                        {
                            monster.Activated = true;
                            monster.Target = ThePlayer;
                        }
                    }
                }
            }
        }
    }
}
