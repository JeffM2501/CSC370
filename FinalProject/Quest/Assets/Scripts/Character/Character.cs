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

    public int HitPoints = 0;
    public int MagicPower = 0;
    public float PerceptionRange = 0;
    public float Speed = 0;

    public float Initiative = 0;

    public int AttackValue = 0;
    public int ArmorValue = 0;

    public Inventory InventoryItems = new Inventory();
    public EquipmentList EquipedItems = new EquipmentList();

    // temp stats
    public int AttackBonus = 0;
    public int DefenseBonus = 0;

    public float DodgeBonus = 0;
    public float CritBonus = 0;

    public int HealthBonus = 0;
    public int MagicBonus = 0;

    public float SpeedBonus = 0;

    public int SkillSlots = 3;

    public Dictionary<Attribute.AttributeTypes, AttributeInstance> Attributes = new Dictionary<Attribute.AttributeTypes, AttributeInstance>();
    public List<SkillInstance> Skills = new List<SkillInstance>();

    protected bool UseLayers = true;
    protected bool ForceHair = false;

    public Texture  BaseLayer;
    public Color    HairColor = Color.white;
    public Texture  HairLayer;

    public Color EyeColor = Color.white;
    public Texture EyeLayer = null;

    protected List<SpriteManager.SpriteLayer> GraphicLayers = new List<SpriteManager.SpriteLayer>();

    public event GameState.EventCallback LayersChanged;

    public virtual void Init()
    {

    }

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

        RebuildEquipment();
    }

    public void DropItem(Item item)
    {
        GameState.Instance.DropItem(item, WorldObject.transform.position);

        // find a bundle around us
        GameObject.FindGameObjectsWithTag("Bundle");
    }

    public void RebuildTempStats()
    {
        AttackBonus = 0;
        MagicBonus = 0;
        DefenseBonus = 0;
        HealthBonus = 0;
        SpeedBonus = 0;

        foreach (SkillInstance skill in Skills)
            skill.OnApply(this);

        Speed = (Attributes[Attribute.AttributeTypes.Agility].Level + SpeedBonus) * 2f;
        HitPoints = Attributes[Attribute.AttributeTypes.Might].Level * 10 + HealthBonus;
        if (HitPoints < 5)
            HitPoints = 5;

        MagicPower = Attributes[Attribute.AttributeTypes.Smarts].Level * 5 + MagicBonus;
        PerceptionRange = (Attributes[Attribute.AttributeTypes.Smarts].Level + Attributes[Attribute.AttributeTypes.Agility].Level) * 2f;
        if (PerceptionRange < 5)
            PerceptionRange = 5;

        Initiative = (Attributes[Attribute.AttributeTypes.Smarts].Level + Attributes[Attribute.AttributeTypes.Agility].Level) / 2f;

        int weaponStatValue = Attributes[Attribute.AttributeTypes.Might].Level;
        if (EquipedItems.IsWielding(Weapon.WeaponTypes.Staff))
            weaponStatValue = Attributes[Attribute.AttributeTypes.Smarts].Level;
         if (EquipedItems.IsWielding(Weapon.WeaponTypes.Bow))
            weaponStatValue = Attributes[Attribute.AttributeTypes.Agility].Level;

         AttackBonus = weaponStatValue + AttackBonus;

         ArmorValue = EquipedItems.ArmorValue() + DefenseBonus;
    }

    public virtual void RebuildEquipment()
    {
        SpriteManager spriteMan = GameState.Instance.SpriteMan;

        // base
        GraphicLayers.Add(spriteMan.GetLayer(BaseLayer, Color.white));

        if (!UseLayers)
            return;

        // pants, everyone gets em
        GraphicLayers.Add(spriteMan.GetLayer(ItemFactory.Pants.GetTextureForGender(Gender),Color.white));

        // shirts
        if (EquipedItems.Torso != null)
            GraphicLayers.Add(spriteMan.GetLayer(EquipedItems.Torso.GetTextureForGender(Gender), EquipedItems.Torso.LayerColor));

        // eyes
        if (EyeLayer != null)
            GraphicLayers.Add(spriteMan.GetLayer(EyeLayer, EyeColor));

        // hair or hats
        if (HairLayer != null && (ForceHair || EquipedItems.Head == null))
            GraphicLayers.Add(spriteMan.GetLayer(BaseLayer, HairColor));

        if (EquipedItems.Head != null)
            GraphicLayers.Add(spriteMan.GetLayer(EquipedItems.Head.GetTextureForGender(Gender), EquipedItems.Head.LayerColor));

        if (LayersChanged != null)
            LayersChanged(this, EventArgs.Empty);
    }

    public virtual List<SpriteManager.SpriteLayer> GetSpriteLayers()
    {
        return GraphicLayers;
    }
}
