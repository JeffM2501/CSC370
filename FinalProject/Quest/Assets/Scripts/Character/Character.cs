using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character
{
    public Movement CharacterMovemnt = null;

    public bool Alive = true;

    public Character Target = null;

    public GameObject WorldObject;

    public UInt64 ID = UInt64.MinValue;

    public string Name = string.Empty;

    public int XP = 0;

    public bool IsHominid = true;

    public AnimationSequence Anims = null;

    protected bool Animating = false;

    protected WeaponAnimation WeaponAnim = null;

    public AudioClip HitSound = null;
    public AudioClip DieSound = null;

    public AudioClip MoveSound = null;

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

    protected float LastHeal = float.MinValue;
    public float HealInterval = 3;

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

    public SkillInstance BasicAttackSkill = new SkillInstance(SkillFactory.BasicAttacks[Weapon.WeaponTypes.Hand]);

    public float LastSkillUse = float.MinValue;

    protected bool UseLayers = false;
    protected bool ForceHair = false;

    // this is a hack because unity 4.2 broke my spite layering code
    // see http://forum.unity3d.com/threads/192146-Unity-4-2-Multiple-Materials-draw-order for bug info
    public List<string> FemaleLayers = new List<string>();
    public List<string> MaleLayers = new List<string>();

    public string BaseLayer = string.Empty;
    public Color HairColor = Color.white;
    public string HairLayer = string.Empty;

    public Color EyeColor = Color.white;
    public string EyeLayer = string.Empty;

    protected Texture AnimTexture = null;

    protected List<SpriteManager.SpriteLayer> GraphicLayers = new List<SpriteManager.SpriteLayer>();

    public event GameState.EventCallback LayersChanged;

    public delegate bool TimedEventCallback (Character sender, object tag);

    public event EventHandler EquipementChanged = null;

    public event EventHandler Death;

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
        if (Damage >= HitPoints && Alive)
        {
            Anims.SetSequence("Dying");
            Alive = false;
            Die();
            if (Death != null)
                Death(this, EventArgs.Empty);
        }

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

        if (HitPoints == 0)
            RebuildTempStats();

        if (Anims != null)
            Anims.Update();

        if (Alive)
        {
            if(Time.time - LastHeal > HealInterval)
            {
                LastHeal = Time.time;

                Heal(Attributes[Attribute.AttributeTypes.Might].Level);
                AddMana(Attributes[Attribute.AttributeTypes.Smarts].Level);
            }
        }
    }

    public virtual void Init()
    {
        CharacterMovemnt = WorldObject.GetComponent("Movement") as Movement;

        if (AnimTexture == null)
            RebuildEquipment();

        if (IsHominid)
        {
            Anims = new HominidAnimation(AnimTexture);
        }
        else
        {
            Anims = new MonsterAnimation(AnimTexture);
            UseLayers = false;
        }

        Anims.AnimationComplete += AnimComplete;

        WorldObject.GetComponent<CharacterObject>().SetCharacter(this);
        SetupWeaponAnim();
    }

    public virtual void Bury()
    {
        if (WorldObject != null)
            MonoBehaviour.Destroy(WorldObject);
    }

    protected void AnimComplete(object sender, EventArgs args)
    {
        if (Anims.CurrentSequence == "Idle" || Anims.CurrentSequence == "Walk")
            return;

        Animating = false;
    }

    protected void SetupWeaponAnim()
    {
         if (WorldObject == null)
            return;

        for (int i = 0; i < WorldObject.transform.childCount; i++)
        {
            Transform child = WorldObject.transform.GetChild(i);
            if (child.name.Contains("WeaponPlane"))
            {
                for (int j = 0; j < child.transform.childCount; j++)
                {
                    Transform meshChild = child.GetChild(j);

                    WeaponAnim = meshChild.GetComponent<WeaponAnimation>();
                    if (WeaponAnim == null)
                        return;
                }
            }
        }

        SetWeaponSprites();
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

    public void EquipItem(Equipment item)
    {
        EquipItem(item, Equipment.EquipmentLocation.Weapon);
    }

    public void EquipItem(Equipment item, Equipment.EquipmentLocation location)
    {
        Equipment returned = null;
        Weapon weapon = item as Weapon;
        if (weapon == null)
            returned = EquipedItems.EquipArmor(item);
        else
        {
            returned = EquipedItems.EquipWeapon(weapon, location == Equipment.EquipmentLocation.Weapon);
            BasicAttackSkill = new SkillInstance(SkillFactory.BasicAttacks[EquipedItems.WeaponType()]);
            SetWeaponSprites();
        }

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

    public virtual void Move(Vector3 vec)
    {
        if (!Alive)
            return;

        float x = Mathf.Abs(vec.x);
        float y = Mathf.Abs(vec.z);

//         if (x < 0.01f && y < 0.01f && Anims.CurrentDirection != AnimationSequence.Directions.None)
//             return;

        if (x < 0.0001f && y < 0.0001f)
        {
            if (!Animating)
                Anims.SetSequence("Idle");
        }
        else
        {
            if (!Animating)
                Anims.SetSequence("Walk");
            if (x > y)
                Anims.SetDirection(vec.x < 0 ? AnimationSequence.Directions.East : AnimationSequence.Directions.West);
            else
                Anims.SetDirection(vec.z > 0 ? AnimationSequence.Directions.North : AnimationSequence.Directions.South);
        }

        CharacterMovemnt.Move(vec * (Speed * Time.deltaTime));
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

        Speed = ((Attributes[Attribute.AttributeTypes.Agility].Level + SpeedBonus) * 1f) + 1f;
        HitPoints = Attributes[Attribute.AttributeTypes.Might].Level * 10 + HealthBonus;
        if (HitPoints < 5)
            HitPoints = 5;

        MagicPower = Attributes[Attribute.AttributeTypes.Smarts].Level * 5 + MagicBonus;
        PerceptionRange = (Attributes[Attribute.AttributeTypes.Smarts].Level + Attributes[Attribute.AttributeTypes.Agility].Level) * 2f;
        if (PerceptionRange < 5)
            PerceptionRange = 5;

        Initiative = 5;

        int smartAgiDiv = Attributes[Attribute.AttributeTypes.Smarts].Level + Attributes[Attribute.AttributeTypes.Agility].Level/2;
        if (smartAgiDiv >= Initiative)
            Initiative = Initiative/(smartAgiDiv-Initiative);
        else
            Initiative = Initiative/smartAgiDiv;

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

    protected string GetFixedSpriteImage(List<string> list)
    {
        if (FemaleLayers.Count == 1)
            return list[0];
        else
        {
            if (EquipedItems.Torso != null)
            {
                if (EquipedItems.Torso.Name == "Leather Armor")
                {
                    if (EquipedItems.Head != null)
                        return list[3];
                    else
                        return list[2];
                }
                else if (EquipedItems.Torso.Name != "Cloth Shirt")
                {
                    if (EquipedItems.Head != null)
                        return list[5];
                    else
                       return list[4];
                }
            }

            if (EquipedItems.Head != null)
                return list[1];
            else
                return list[0];
        }
    }

    public virtual void RebuildEquipment()
    {
        SpriteManager spriteMan = GameState.Instance.SpriteMan;

        GraphicLayers.Clear();

        if (!UseLayers && Gender == Genders.Female && FemaleLayers.Count > 0)
            GraphicLayers.Add(spriteMan.GetLayer(GetFixedSpriteImage(FemaleLayers), Color.white));
        else if (!UseLayers && Gender == Genders.Male && MaleLayers.Count > 0)
            GraphicLayers.Add(spriteMan.GetLayer(GetFixedSpriteImage(MaleLayers), Color.white));
        else
            GraphicLayers.Add(spriteMan.GetLayer(BaseLayer, Color.white));

        AnimTexture = GraphicLayers[0].LayerImage;

        if (UseLayers)
        {
            // pants, everyone gets em
            GraphicLayers.Add(spriteMan.GetLayer(ItemFactory.Pants.GetTextureForGender(Gender),Color.white));

            // shirts
            if (EquipedItems.Torso != null)
                GraphicLayers.Add(spriteMan.GetLayer(EquipedItems.Torso.GetTextureForGender(Gender), EquipedItems.Torso.LayerColor));

            // eyes
            if (EyeLayer != string.Empty)
                GraphicLayers.Add(spriteMan.GetLayer(EyeLayer, EyeColor));

            // hair or hats
            if (HairLayer != string.Empty && (ForceHair || EquipedItems.Head == null))
                GraphicLayers.Add(spriteMan.GetLayer(HairLayer, HairColor));

        //   if (EquipedItems.Head != null)
       //        GraphicLayers.Add(spriteMan.GetLayer(EquipedItems.Head.GetTextureForGender(Gender), EquipedItems.Head.LayerColor));
        }
        if (LayersChanged != null)
            LayersChanged(this, EventArgs.Empty);

        if (EquipementChanged != null)
            EquipementChanged(this, EventArgs.Empty);
    }

    protected void SetWeaponSprites()
    {
        if (WeaponAnim == null)
            return;

        if (EquipedItems.WeaponType() == Weapon.WeaponTypes.Hand)
            WeaponAnim.Stop();
        else
        {
            Material mat = Resources.Load("Items/Weapons/Materials/Sword") as Material;
            if (EquipedItems.WeaponType() == Weapon.WeaponTypes.Bow)
                mat = Resources.Load("Items/Weapons/Materials/Biw") as Material;
          else if (EquipedItems.WeaponType() == Weapon.WeaponTypes.Staff)
                mat = Resources.Load("Items/Weapons/Materials/Staff") as Material;

            if (EquipedItems.WeaponType() == Weapon.WeaponTypes.Bow)
                WeaponAnim.SetSequence(new RangedWeaponAnimation(mat.mainTexture));
            else
                WeaponAnim.SetSequence(new MeeleWeponAnimation(mat.mainTexture));
        }
    }

    public virtual List<SpriteManager.SpriteLayer> GetSpriteLayers()
    {
        return GraphicLayers;
    }

    public virtual void Die()
    {
        if (WorldObject.audio != null && DieSound != null)
            WorldObject.audio.PlayOneShot(DieSound);
        Select(false);

        CharacterObject obj = WorldObject.GetComponent<CharacterObject>();
        if (obj != null)
            obj.Die();

        GameState.Instance.DropLoot(this);
    }

    public virtual int GetHealth()
    {
        return HitPoints - Damage;
    }

    public virtual float GetHealthParam()
    {
        return Mathf.Max(0,GetHealth()/(float)HitPoints);
    }

    public virtual int GetMana()
    {
        return MagicPower - ManaSpent;
    }

    public virtual float GetManaParam()
    {
        return Mathf.Max(0, GetMana() / (float)MagicPower);
    }

    public virtual void Heal(int amount)
    {
        Damage -= amount;
        if (Damage < 0)
            Damage = 0;
    }

    public virtual void AddMana(int amount)
    {
        ManaSpent -= amount;
        if (ManaSpent < 0)
            ManaSpent = 0;
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, CharacterObject.HitType.Physical);
    }

    public virtual void TakeDamage(int amount, CharacterObject.HitType hitType)
    {
        if (amount <= 0)
            amount = 1;

        if (amount > 0 && WorldObject.audio != null && HitSound != null)
            WorldObject.audio.PlayOneShot(HitSound);

        Damage += amount;

        CharacterObject obj = WorldObject.GetComponent<CharacterObject>();
        if (obj != null)
            obj.Hit(hitType);

        Debug.Log(Name + " Took Damage: " + amount.ToString());
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
        if (!SkillUseable(BasicAttackSkill))
            return false;
        UseSkill(BasicAttackSkill);
        Debug.Log("Basic Attack!");
        GameState.Instance.BattleMan.PhysicalAttack(this, Target, 0.5f, AttackBonus, BasicAttackSkill.BaseSkill.Range, EquipedItems.WieldingMinDamage(), EquipedItems.WieldingMaxDamage());
        AttackComplete();
        return true;
    }

    public virtual bool ActivateSkill(SkillInstance skill)
    {
        if (!SkillUseable(skill))
            return false;
        UseSkill(skill);
        skill.OnAcivate(this);
        Debug.Log("Use Skill " + skill.BaseSkill.Name);
        AttackComplete();
        return true;
    }

    public virtual bool CastSpell(SpellInstance spell)
    {
        if (!SkillUseable(spell))
            return false;
        UseSkill(spell);
        spell.OnAcivate(this);
        Debug.Log("Cast Spell " + spell.BaseSpell.Name);
        AttackComplete();
        return true;
    }

    protected void UseSkill(SkillInstance skill)
    {
        LastSkillUse = Time.time;
        skill.LastUse = LastSkillUse;

        AnimateTo(skill.BaseSkill.AnimType);
    }

    protected virtual void AttackComplete()
    {
        if (Target == null || !Target.Alive)
            SetTarget(null);
    }

    public bool SkillUseable( SkillInstance skill )
    {
        if (!skill.Useable(this))
            return false;

        float timeSinceLastSkill = Time.time - LastSkillUse;

        if (timeSinceLastSkill < Initiative)
            return false;

        return (Time.time - skill.LastUse) > skill.BaseSkill.Cooldown; 
    }

    public float GetSkillUseParamater(SkillInstance skill)
    {
        if (!skill.Useable(this))
            return 1;

        float param = 0;
        bool inCooldown = false;
        if (skill.BaseSkill.Cooldown > 0)
        {
            float timeSinceThisSkill = Time.time - skill.LastUse;

            if (timeSinceThisSkill < skill.BaseSkill.Cooldown) // skill is in cooldown
            {
                param = timeSinceThisSkill / skill.BaseSkill.Cooldown;
                inCooldown = true;
            }
        }

        float timeSinceLastSkill = Time.time - LastSkillUse;
        if (timeSinceLastSkill < Initiative)
        {
            float globalParam = timeSinceLastSkill / Initiative;
            if (inCooldown)
            {
                float skillEnd = skill.LastUse + skill.BaseSkill.Cooldown;
                float timerEnd = LastSkillUse + Initiative;
                if (skillEnd < timerEnd) // the timer will end later
                    param = globalParam;
            }
            else
                param = globalParam;
        }

        return param;
    }

    public void AnimateTo(Skill.ActiveAnimationTypes animType)
    {
        if (WeaponAnim == null || Animating || animType == Skill.ActiveAnimationTypes.None)
            return;

        WeaponAnim.Stop();

        Animating = true;
        switch (animType)
        {
            case Skill.ActiveAnimationTypes.Meele:
                Anims.SetSequence("HandAttack");
                break;

            case Skill.ActiveAnimationTypes.Casting:
                Anims.SetSequence("Cast");
                break;

            case Skill.ActiveAnimationTypes.Ranged:
                Anims.SetSequence("RangedAttack");
                break;
        }

        if (animType != Skill.ActiveAnimationTypes.Casting)
            WeaponAnim.Play();
    }

    public void Select(bool select)
    {
        if (WorldObject == null)
            return;

        for (int i = 0; i < WorldObject.transform.childCount; i++)
        {
            Transform child = WorldObject.transform.GetChild(i);
            if (child.name.Contains("Bounce"))
                child.gameObject.SetActive(select);
        }
    }

    public void ClearInventory()
    {
        InventoryItems.Clear();
        EquipedItems.Clear();
    }
}
