using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GUIPanel : IDisposable
{
    public static List<GUIPanel> Pannels = new List<GUIPanel>();

    public static void RebuildAll()
    {
        foreach(GUIPanel panel in Pannels)
            panel.Resize();
    }

    public bool Enabled = true;

    public List<GUIElement> Elements = new List<GUIElement>();
    public Texture2D Background;
    public string Name = string.Empty;

    public Rect Bounds = new Rect();

    protected bool NeedRebuild = false;

    protected Rect EfectiveBounds = new Rect();

    public bool UseImageSize = true;

    public int ID = 0;

    public static int LastGUID = 0;

    public GUIStyle Style = new GUIStyle();

    [System.Serializable]
    public enum Alignments
    {
        Absolute,
        Min,
        Max,
        Center,
    }

    public Alignments Alignement = Alignments.Absolute;

    public GUIPanel()
    {

    }

    public void Init()
    {
        Pannels.Add(this);
        ID = LastGUID;
        LastGUID++;
    }

    public void Resize()
    {
        NeedRebuild = true;
        foreach(GUIElement element in Elements)
            element.Resize();
    }

    public void Dispose()
    {
        if (Pannels.Contains(this))
            Pannels.Remove(this);
    }

    public void Draw()
    {
        if (!Enabled)
            return;

        PreDraw();
        if (Background != null)
            GUI.Window(ID, Bounds, Update, Background, Style);
        else
            GUI.Window(ID, Bounds, Update, Name, Style);
    }

    public void Update(int id)
    {
        foreach (GUIElement element in Elements)
            element.Draw(Bounds);
    }

    public virtual void PreDraw()
    {

    }
}

[System.Serializable]
public class GUIElement
{
    public bool Enabled = true;
    public Rect Bounds = new Rect();

    protected bool NeedRebuild = false;

    protected Rect EfectiveBounds = new Rect();

    public GUIPanel.Alignments Alignement = GUIPanel.Alignments.Absolute;

    public bool UseImageSize = false;

    [System.Serializable]
    public enum ElementTypes
    {
        Label,
        Frame,
        Button,
        ImageButton,
        Image,
    }

    public ElementTypes ElementType = ElementTypes.Frame;

    public List<GUIElement> Children = new List<GUIElement>();

    public event EventHandler Clicked;

    public Texture2D BackgroundImage = null;
    public string Name = string.Empty;

    public void Resize()
    {
        NeedRebuild = true;
        foreach(GUIElement element in Children)
            element.Resize();
    }

    public void Draw(Rect parrent)
    {
        if (!Enabled)
            return;

        switch (ElementType)
        {
            case ElementTypes.Label:
                GUI.Label(Bounds, Name);
                break;

            case ElementTypes.Image:
                GUI.Box(Bounds, BackgroundImage);
                break;

            case ElementTypes.Button:
                if (GUI.Button(Bounds,Name))
                {
                    if (Clicked != null)
                        Clicked(this, EventArgs.Empty);
                }
                break;
        }

        Rect myRect = new Rect(parrent.xMin + Bounds.xMin, parrent.yMin + Bounds.yMin, Bounds.width, Bounds.height);

        foreach (GUIElement element in Children)
            Draw(myRect);

    }
}
