using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharcterScreen : GUIPanel
 {
    public Character TheCharacter = null;

    protected GUIElement XPLabel = null;

    protected List<GUIElement> SkillElements = new List<GUIElement>();

    protected Texture UpArrow = null;
    protected Texture DisableArrow = null;

    public CharcterScreen()
    {
        Enabled = false;

        Bounds = new Rect(10, 10, 512, 512);
        HAlignement = Alignments.Max;
        VAlignement = Alignments.Max;
    }

    float Margin = 24;

    protected bool ForceReload = false;

    protected override void Load()
    {
        base.Load();

        Skin = Resources.Load("GUI/UI Skin") as GUISkin;

        UpArrow = Resources.Load("GUI/FullUpArrow") as Texture;
        DisableArrow = Resources.Load("GUI/EmptyUpArrow") as Texture;

        TheCharacter = GameState.Instance.PlayerObject;
        this.Background = Resources.Load("GUI/PlainBackground") as Texture;

        NewImageButton(Alignments.Max, Margin, Alignments.Absolute, Margin, Resources.Load("GUI/CloseBox") as Texture, Close);
        NewLabel(Alignments.Absolute, Margin, Alignments.Absolute, Margin, 256, 32, TheCharacter.Name + ":Stats");
        XPLabel = NewLabel(Alignments.Absolute, Margin, Alignments.Absolute, 54, 256, 32, "XP:" + TheCharacter.XP.ToString());
        NewLabel(Alignments.Max, Margin * 5, Alignments.Absolute, Margin * 2.5f, 64, 32, "Skills").SetFont(Color.white, 24);

        ToolTipElement = NewImage(Alignments.Absolute, 0, Alignments.Max, 0, Resources.Load("GUI/WideToolTip") as Texture);
        ToolTipTextElement = ToolTipElement.NewLabel(Alignments.Absolute, 35, Alignments.Absolute, 15, 450, 32, "Blerg");

        ToolTipTextElement.TextStyle = new GUIStyle();
        ToolTipTextElement.TextStyle.font = Resources.Load("GUI/ALEAWBB_") as Font;
        ToolTipTextElement.TextStyle.normal.textColor = Color.white;
        ToolTipTextElement.TextStyle.fontSize = 12;

        ToolTipElement.Enabled = false;

        SetPlayerData();
    }

    public override void PreDraw()
    {
        base.PreDraw();

         if (ForceReload)
         {
             SetPlayerData();
             ForceReload = false;
         }
    }

    public void BuildSkills()
    {
        foreach (GUIElement element in SkillElements)
        {
            if (this.Elements.Contains(element))
                this.Elements.Remove(element);
        }
        SkillElements.Clear();

        SetSkills();
        NeedRebuild = true;
    }

    protected void SetSkills()
    {
        // do the stats
         Vector2 offset = new Vector2(Margin, 96);
 
         offset = AddSkill(TheCharacter.Attributes[Attribute.AttributeTypes.Might], offset);
         offset = AddSkill(TheCharacter.Attributes[Attribute.AttributeTypes.Smarts], offset);
         offset = AddSkill(TheCharacter.Attributes[Attribute.AttributeTypes.Agility], offset);

         foreach (SkillInstance skill in TheCharacter.Skills)
         {
             if (skill.Level > 0)
                offset = AddSkill(skill, offset);
         }
         foreach (SpellInstance spell in TheCharacter.Spells)
             offset = AddSkill(spell, offset);
 
         foreach (Skill skill in SkillFactory.Skills.Values)
         {
             if (skill.CharacterHasRequirements(TheCharacter) && skill.Purchase > 0 && TheCharacter.GetSkillByName(skill.Name) == null)
                  offset = AddSkill(skill, offset);
         }

         foreach (Spell spell in SpellFeactory.Spells.Values)
         {
             if (spell.CharacterHasRequirements(TheCharacter) && spell.Purchase > 0 && TheCharacter.GetSkillByName(spell.Name) == null)
                 offset = AddSkill(spell, offset);
         }
    }

    protected void UpgradeSkill(object sender, EventArgs args)
    {
        GUIElement element = sender as GUIElement;
        if (element == null || element.Tag == null)
            return;

        SkillInstance skill = element.Tag as SkillInstance;

        if (skill != null)
        {
            if (TheCharacter.XP >= skill.BaseSkill.Upgrade)
            {
                skill.Level++;
                TheCharacter.XP -= skill.BaseSkill.Upgrade;
                ForceReload = true;
            }
        }
        else
        {
            Skill newSkill = element.Tag as Skill;
            if (newSkill != null)
            {
                if (TheCharacter.XP >= newSkill.Purchase)
                {
                    TheCharacter.XP -= newSkill.Purchase;
                    TheCharacter.AddSkill(newSkill, 1);
                    ForceReload = true;
                    TheCharacter.RebuildEquipment();
                }
            }
        }
    }

    Vector2 AddSkill(SkillInstance skill, Vector2 offset)
    {
        return AddSkill(skill.BaseSkill, skill.Level, offset, skill);
    }

    Vector2 AddSkill(Skill skill, Vector2 offset)
    {
        return AddSkill(skill, 0, offset, skill);
    }

    Vector2 AddSkill(Skill skill, int level, Vector2 offset, object tag)
    {
        Texture icon = Resources.Load(skill.IconImage) as Texture;
        if (icon == null)
        {
            Debug.Log("Can't load skill icon " + skill.IconImage);
            return offset;
        }

        // add the name
        Rect size = new Rect(0, 0, 100, 55);

        //    SkillElements.Add(NewImage(GUIPanel.Alignments.Absolute, offset.x, GUIPanel.Alignments.Absolute, offset.y, Resources.Load(skill.BaseSkill.IconImage) as Texture));

        GUIElement lab = NewLabel(GUIPanel.Alignments.Absolute, offset.x, GUIPanel.Alignments.Absolute, offset.y, 50, 12, skill.Name);
        lab.SetFont(Color.white, 16);
        SkillElements.Add(lab);

        lab = NewImage(GUIPanel.Alignments.Absolute, offset.x, GUIPanel.Alignments.Absolute, offset.y + 18, icon);
        lab.ToolTip = skill.Description;
        SkillElements.Add(lab);

        int cost = skill.Upgrade;
        if (level == 0)
            cost = skill.Purchase;

        bool upgradeable = cost < TheCharacter.XP;
        if (level > 0)
            upgradeable = upgradeable && level < skill.MaxLevel;

        Color costColor = Color.black;
        if (upgradeable)
            costColor = Color.green;

        lab = NewLabel(GUIPanel.Alignments.Absolute, offset.x, GUIPanel.Alignments.Absolute, offset.y + 60, 50, 12, cost.ToString());
        lab.SetFont(costColor, 16);
        SkillElements.Add(lab);

        lab = NewLabel(GUIPanel.Alignments.Absolute, offset.x + 50, GUIPanel.Alignments.Absolute, offset.y + 16, 50, 12, level.ToString());
        lab.SetFont(costColor, 24);
        SkillElements.Add(lab);

        GUIElement upgrade = NewImageButton(GUIPanel.Alignments.Absolute, offset.x + 50, GUIPanel.Alignments.Absolute, offset.y + 38, upgradeable ? UpArrow : DisableArrow, UpgradeSkill);
        if (upgradeable)
        {
            upgrade.ToolTip = "Upgrade the skill by one level, costs " + cost.ToString();
            upgrade.Tag = tag;
        }
        else
        {
            upgrade.ToolTip = "Need " + cost.ToString() + " XP to Upgrade";
            upgrade.Tag = null;
        }

        SkillElements.Add(upgrade);

        Vector2 ret = new Vector2(offset.x + size.width, offset.y);
        if (ret.x > (Bounds.width - Margin - size.width))
            ret = new Vector2(Margin, offset.y + size.height + Margin);

        return ret;
    }

    public void SetPlayerData()
    {
        BuildSkills();
        XPLabel.Name = "XP:" + TheCharacter.XP.ToString();
    }
}
