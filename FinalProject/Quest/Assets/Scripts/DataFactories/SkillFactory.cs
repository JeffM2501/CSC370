using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class SkillFactory 
{
    public static Attribute Might;
    public static Attribute Smarts;
    public static Attribute Agility;

    public static Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();

    public static Dictionary<string, Spell> Spells = new Dictionary<string, Spell>();

    public static void Setup()
    {
        if (Skills.Count != 0)
            return; // been here, done this

        // stats
        Might = new Attribute();
        Might.Name = "Might";
        Might.SkillType = Skill.SkillTypes.Attribute;
        Might.IconImage = "GUI/SkillIcons/Might";
        Might.AttributeType = Attribute.AttributeTypes.Might;
        Might.Description = "Physical Ability. Affects hit points and melee attacks.";

        Smarts = new Attribute();
        Smarts.Name = "Smarts";
        Smarts.SkillType = Skill.SkillTypes.Attribute;
        Smarts.AttributeType = Attribute.AttributeTypes.Smarts;
        Smarts.IconImage = "GUI/SkillIcons/Smarts";
        Smarts.Description = "Mental Ability. Affects magic points and magical attacks.";

        Agility = new Attribute();
        Agility.Name = "Agility";
        Agility.SkillType = Skill.SkillTypes.Attribute;
        Agility.AttributeType = Attribute.AttributeTypes.Agility;
        Agility.IconImage = "GUI/SkillIcons/Speed";
        Agility.Description = "Dexterity Ability. Affects defense and ranged attacks.";

        // weapon skills
        AddWeaponSkill("Swords", "GUI/SkillIcons/Sword", Weapon.WeaponTypes.Sword).Requirements.Add("Might 1");
        AddWeaponSkill("Bows", "GUI/SkillIcons/Bow", Weapon.WeaponTypes.Bow).Requirements.Add("Agility 1");
        AddWeaponSkill("Staves", "GUI/SkillIcons/Staves", Weapon.WeaponTypes.Staff).Requirements.Add("Smarts 1");
        AddSkill("Fist", "GUI/SkillIcons/Fist", new FistWeapon());

        // magics
        Skill arcane = new Skill();
        arcane.SkillType = Skill.SkillTypes.MagicArcane; 
        arcane.Requirements.Add("Smarts 2");
        arcane.AdditionalSkillsGranted.Add("Magic Missile");
        arcane.Purchase = 100;
        arcane.Upgrade = 500;
        AddSkill("Arcane", "GUI/SkillIcons/Arcane", arcane);

        Skill divine = new Skill();
        divine.SkillType = Skill.SkillTypes.MagicDivine;
        divine.Requirements.Add("Smarts 2");
        divine.AdditionalSkillsGranted.Add("Bless");
        divine.Purchase = 100;
        divine.Upgrade = 500;
        AddSkill("Divine", "GUI/SkillIcons/Divine", divine);

        // bonuses
        AddSkill("Dodge", "GUI/SkillIcons/Dodge", new Dodge());
        AddSkill("Tough As Nails", "GUI/SkillIcons/Tough", new ToughAsNails());
        AddSkill("Deadeye", "GUI/SkillIcons/Deadeye", new ToughAsNails());

        // attacks
        AddSkill("Cleave", "GUI/SkillIcons/Cleave", new Cleave());
        AddSkill("Headshot", "GUI/SkillIcons/Headshot", new Headshot());
        AddSkill("Claw", "GUI/SkillIcons/Beast", new Claw());

    
    }

    public static Skill AddWeaponSkill(string name, string icon, Weapon.WeaponTypes weapon)
    {
        WeaponSkill w = new WeaponSkill();
        w.WeaponType = weapon;

        return AddSkill(name, icon, w);
    }

    public static Skill AddSkill(string name, string icon, Skill skill)
    {
        skill.Name = name;
        skill.Level = 0;
        skill.IconImage = icon;
        Skills.Add(skill.Name, skill);
        return skill;
    }
}

public class WeaponSkill : Skill
{
    public WeaponSkill()
    {
        this.SkillType = Skill.SkillTypes.Weapon;
        Purchase = 100;
        Upgrade = 500;
        MaxLevel = 10;
        Description = "Gives bonuses to attacks with the specified weapon";
    }

    public Weapon.WeaponTypes WeaponType = Weapon.WeaponTypes.Hand;

    public override void OnApply(Character character)
    {
        base.OnApply(character);
        if (character.EquipedItems.IsWielding(WeaponType))
            character.AttackBonus += Level;
    }
}

public class FistWeapon : Skill
{
    public FistWeapon()
        : base()
    {
        Requirements.Add("Might 2");
        Requirements.Add("Agility 2");
        MaxLevel = 5;
    }
    
    public override void OnApply(Character character)
    {
        base.OnApply(character);
        if (character.EquipedItems.IsWielding(Weapon.WeaponTypes.Hand) || (character.EquipedItems.LeftHand == null && character.EquipedItems.RightHand == null))
            character.AttackBonus += Level;
    }
}

public class Dodge : Skill
{
    public Dodge()
        : base()
    {
        Purchase = 100;
        Upgrade = 300;
        Requirements.Add("Smarts 1");
        this.SkillType = Skill.SkillTypes.Passive;
        MaxLevel = 10;
        Description = "Lets you avoid some attacks";
    }

    public override void OnApply(Character character)
    {
        base.OnApply(character);
        character.DodgeBonus += 0.05f * Level;
    }
}

public class ToughAsNails : Skill
{
    public ToughAsNails()
        : base()
    {
        Purchase = 100;
        Upgrade = 300;
        Requirements.Add("Might 1");
        this.SkillType = Skill.SkillTypes.Passive;
        MaxLevel = 10;
        Description = "Adds more hit points per level";
    }

    public override void OnApply(Character character)
    {
        base.OnApply(character);
        character.HealthBonus += 5 * Level;
    }
}

public class Deadeye : Skill
{
    public Deadeye()
        : base()
    {
        Purchase = 100;
        Upgrade = 300;
        Requirements.Add("Agility 2");
        this.SkillType = Skill.SkillTypes.Passive;
        MaxLevel = 10;
        Description = "Increases chance for critical hit";
    }

    public override void OnApply(Character character)
    {
        base.OnApply(character);
        character.CritBonus += 0.25f * Level;
    }
}

public class Cleave : Skill
{
    public Cleave()
        : base()
    {
        Purchase = 200;
        Upgrade = 400;
        Requirements.Add("Might 2");
        this.SkillType = Skill.SkillTypes.Active;
        MaxLevel = 10;
        Description = "Large melee attack that hits all around";

        Cooldown = 2;
    }

    public override void OnAcivate(Character character)
    {
        base.OnAcivate(character);
        // tell combat system to hit EVERYONE
    }
}

public class Headshot : Skill
{
    public Headshot()
        : base()
    {
        Purchase = 200;
        Upgrade = 400;
        Requirements.Add("Agility 2");
        this.SkillType = Skill.SkillTypes.Active;
        MaxLevel = 10;
        Description = "Ranged attack with bonus damage";

        Cooldown = 2;
    }

    public override void OnAcivate(Character character)
    {
        base.OnAcivate(character);
        // tell combat system to attack target with bonuses
    }
}

public class Claw : Skill
{
    public Claw()
        : base()
    {
        Purchase = -1;
        Upgrade = -1;
        Requirements.Add("Might 2");
        this.SkillType = Skill.SkillTypes.Active;
        MaxLevel = 8;
        Description = "Monster Skill, nobody should EVER see this";

        Cooldown = 1;
    }

    public override void OnAcivate(Character character)
    {
        base.OnAcivate(character);
        // tell combat system to attack target with bonuses
    }
}

