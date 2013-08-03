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
    private EquipmentList EquipedItems = new EquipmentList();

    public int SkillSlots = 3;

    public Dictionary<Attribute.AttributeTypes, Attribute> Attributes = new Dictionary<Attribute.AttributeTypes, Attribute>();
    public List<Skill> Skills = new List<Skill>();

    public Texture2D BaseLayer;
    public Texture3D HairLayer;

  //  public event GameState.EventCallback LayersChanged;

    public void EquipItem(Equipment item, Equipment.EquipmentLocation location)
    {

    }

    public List<SpriteManager.SpriteLayer> GetSpriteLayers()
    {
        return new List<SpriteManager.SpriteLayer>();
    }
}
