using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character
{
    public GameObject WorldObject;

    public UInt64 ID = UInt64.MinValue;

    public string Name = string.Empty;
   
    public enum Genders
    {
        Male,
        Female,
    }

    public Genders Gender = Genders.Female;

    public class BasicStats
    {
        public int HitPoints;
        public int MagicPower;
        public float PerceptionRange;
        public float Speed;
        public int AttackValue;
        public int ArmorValue;
    }

    public BasicStats Stats = new BasicStats();
    public Inventory InventoryItems = new Inventory();
    public EquipmentList EquipedItems = new EquipmentList();

    // temp stats
    public int AttackBonus = 0;
    public int DefenseBonus = 0;

    public float DodgeBonus = 0;
    public float CritBonus = 0;

    public int HealthBonus = 0;

    public int SkillSlots = 3;

    public Dictionary<Attribute.AttributeTypes, AttributeInstance> Attributes = new Dictionary<Attribute.AttributeTypes, AttributeInstance>();
    public List<SkillInstance> Skills = new List<SkillInstance>();

    public Texture2D BaseLayer;
    public Texture3D HairLayer;

  //  public event GameState.EventCallback LayersChanged;

    public SkillInstance GetSkillByName(string name)
    {
        foreach (SkillInstance skill in Skills)
        {
            if (skill.BaseSkill.Name == name)
                return skill;
        }
        return null;
    }

    public void EquipItem(Equipment item, Equipment.EquipmentLocation location)
    {
        Equipment returned = null;
        Weapon weapon = item as Weapon;
        if (weapon == null)
            returned = EquipedItems.EquipArmor(item);
        returned = EquipedItems.EquipWeapon(weapon, location == Equipment.EquipmentLocation.Weapon);

        if (!InventoryItems.AddItem(returned))
            DropItem(returned);

        RebuildTempStats();
    }

    public void DropItem(Item item)
    {

    }

    public void RebuildTempStats()
    {
        AttackBonus = 0;
        DefenseBonus = 0;
        HealthBonus = 0;

        foreach (SkillInstance skill in Skills)
            skill.OnApply(this);
    }

    public List<SpriteManager.SpriteLayer> GetSpriteLayers()
    {
        return new List<SpriteManager.SpriteLayer>();
    }
}
