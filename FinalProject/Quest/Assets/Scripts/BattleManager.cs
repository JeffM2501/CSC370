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

    public void Update()
    {

    }


}
