using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character
{
    public Character Target = null;

    public GameObject WorldObject;

    public UInt64 ID = UInt64.MinValue;

    public string Name = string.Empty;

    public int XP = 0;
   
    public enum Genders
    {
        Male,
        Female,
    }

    public Genders Gender = Genders.Female;

    public int ManaSpent = 0;
    public int Damage = 0;

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

    public enum BuffTypes
    {
        Unknown,
        Attack,
        Defense,
        Crit,
        Dodge,
        Health,
        Magic,
        Speed,
    }

    public class Buff
    {
        public BuffTypes BuffType = BuffTypes.Unknown;
        public float Value = 0;

        public float StarTime = 0;
        public float Durration = 0;
    }

    public List<Buff> Buffs = new List<Buff>();

    public void AddBuff(BuffTypes buff, float value, float durration)
    {
        Buff b = new Buff();
        b.BuffType = buff;
        b.Value = value;
        b.Durration = durration;
        b.StarTime = Time.time;
        Buffs.Add(b);
        RebuildTempStats();
    }

    public int SkillSlots = 3;

    public Dictionary<Attribute.AttributeTypes, AttributeInstance> Attributes = new Dictionary<Attribute.AttributeTypes, AttributeInstance>();
    public List<SkillInstance> Skills = new List<SkillInstance>();
    public List<SpellInstance> Spells = new List<SpellInstance>();

    protected bool UseLayers = true;
    protected bool ForceHair = false;

    public Texture  BaseLayer;
    public Color    HairColor = Color.white;
    public Texture  HairLayer;

    public Color EyeColor = Color.white;
    public Texture EyeLayer = null;

    protected List<SpriteManager.SpriteLayer> GraphicLayers = new List<SpriteManager.SpriteLayer>();

    public event GameState.EventCallback LayersChanged;

    public delegate bool TimedEventCallback (Character sender, object tag);

    public class TimedEvent
    {
        public float Interval = 0;
        public object Tag = null;
        public int Repeats = 0;
        public TimedEventCallback Callback;
        public float LastUpdate = 0;
        public int Count = 0;
    }

    public List<TimedEvent> TimedEvents = new List<TimedEvent>();

    public virtual void AddTimedEvent(float interval, int repeats, TimedEventCallback callback, object tag)
    {
        TimedEvent evt = new TimedEvent();
        evt.Interval = interval;
        evt.Repeats = repeats;
        evt.Callback = callback;
        evt.LastUpdate = Time.time;

        TimedEvents.Add(evt);
    }

    public virtual void Update()
    {
        List<TimedEvent> toKill = new List<TimedEvent>();
        foreach (TimedEvent evt in TimedEvents)
        {
            if (evt.LastUpdate + evt.Interval < Time.time)
            {
                if (evt.Callback(this, evt.Tag) || evt.Count >= evt.Repeats)
                    toKill.Add(evt);
                else
                {
                    evt.Count++;
                    evt.LastUpdate = Time.time;
                }
            }
        }

        foreach (TimedEvent evt in toKill)
            TimedEvents.Remove(evt);

        List<Buff> buffsToKill = new List<Buff>();

        foreach (Buff b in Buffs)
        {
            if (b.StarTime + b.Durration < Time.time)
                buffsToKill.Add(b);
        }

        foreach (Buff b in buffsToKill)
            Buffs.Remove(b);

        RebuildTempStats();
    }

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

    public void BackpackItem(Item item)
    {
        if (!InventoryItems.AddItem(item))
            DropItem(item);
    }

    public void EquipItem(Equipment item, Equipment.EquipmentLocation location)
    {
        Equipment returned = null;
        Weapon weapon = item as Weapon;
        if (weapon == null)
            returned = EquipedItems.EquipArmor(item);
        returned = EquipedItems.EquipWeapon(weapon, location == Equipment.EquipmentLocation.Weapon);

        if (returned != null && !InventoryItems.AddItem(returned))
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
        DodgeBonus = 0;
        CritBonus = 0;

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

         foreach (Buff buff in Buffs)
         {
             switch (buff.BuffType)
             {
                 case BuffTypes.Attack:
                     AttackBonus += (int)buff.Value;
                     break;

                 case BuffTypes.Defense:
                     ArmorValue += (int)buff.Value;
                     break;

                 case BuffTypes.Health:
                     HitPoints += (int)buff.Value;
                     break;

                 case BuffTypes.Speed:
                     Speed += (int)buff.Value;
                     break;

                 case BuffTypes.Magic:
                     MagicPower += (int)buff.Value;
                     break;

                 case BuffTypes.Dodge:
                     DodgeBonus += buff.Value;
                     break;

                 case BuffTypes.Crit:
                     CritBonus += buff.Value;
                     break;
             }
         }
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

    public virtual void Die()
    {
    }

    public virtual int GetHealth()
    {
        return HitPoints - Damage;
    }

    public virtual int GetMana()
    {
        return MagicPower - ManaSpent;
    }

    public virtual void Heal(int amount)
    {
        Damage -= amount;
        if (Damage < 0)
            Damage = 0;
    }

    public virtual void TakeDamage(int amount)
    {
        Damage += amount;
        if (Damage >= HitPoints)
            Die();
    }

    public virtual void SpendMana(int amount)
    {
        ManaSpent += amount;
    }

    public virtual bool SetTarget(Character target)
    {
        Target = target;

        return true;
    }

    public virtual bool BasicAttack()
    {
        return true;
    }

    public virtual bool ActivateSkill(SkillInstance skill)
    {
        return true;
    }

    public virtual bool CastSpell(SpellInstance skill)
    {
        return true;
    }
}
