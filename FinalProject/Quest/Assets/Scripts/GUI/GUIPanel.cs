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
    protected Rect ActualBounds = new Rect();

    protected bool NeedRebuild = true;

    protected Rect EfectiveBounds = new Rect();

    public bool UseImageSize = true;

    public int ID = 0;

    public static int LastGUID = 0;

    public GUIStyle Style = new GUIStyle();

    [System.Serializable]
    public enum Alignments
    {
        Absolute,
        Max,
        Center,
    }

    public Alignments HAlignement = Alignments.Absolute;
    public Alignments VAlignement = Alignments.Absolute;

    public GUIPanel()
    {

    }

    public void Init()
    {
        Pannels.Add(this);
        ID = LastGUID;
        LastGUID++;

        Load();
    }

    protected virtual void Load()
    {
    }

    public void Resize()
    {
        NeedRebuild = true;
    }

    public void Dispose()
    {
        if (Pannels.Contains(this))
            Pannels.Remove(this);
    }

    protected virtual void Rebuild()
    {
        NeedRebuild = false;

        float x = Bounds.xMin;
        if (HAlignement == Alignments.Max)
            x = Camera.main.pixelWidth - Bounds.xMin;
        else if (HAlignement == Alignments.Center)
            x = (Camera.main.pixelWidth * 0.5f) - (Bounds.width * 0.5f) - Bounds.xMin;


        float y = Bounds.xMin;
        if (VAlignement == Alignments.Max)
            y = Camera.main.pixelHeight - Bounds.yMin;
        else if (VAlignement == Alignments.Center)
            y = (Camera.main.pixelHeight * 0.5f) - (Bounds.height * 0.5f) - Bounds.yMin;

        EfectiveBounds = new Rect(x, y, Bounds.width, Bounds.height);

        foreach (GUIElement element in Elements)
            element.Rebuild(EfectiveBounds);
    }

    public void Draw()
    {
        if (!Enabled)
            return;

        if (NeedRebuild)
            Rebuild();

        PreDraw();
        if (Background != null)
            GUI.Window(ID, Bounds, Update, Background, Style);
        else
            GUI.Window(ID, Bounds, Update, Name, Style);
    }

    public void Update(int id)
    {
        foreach (GUIElement element in Elements)
            element.Draw();
    }

    public virtual void PreDraw()
    {

    }

    public GUIElement NewImageButton(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, Texture2D image,EventHandler handler)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Button;

        element.Clicked += handler;

        Elements.Add(element);
        return element;
    }

    public GUIElement NewImage(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, Texture2D image )
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Image;

        Elements.Add(element);
        return element;
    }

    public GUIElement NewImage(Alignments hAlign, float x, Alignments vAlign, float y, Texture2D image)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, image.width, image.height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Image;

        Elements.Add(element);
        return element;
    }

    public GUIElement NewLabel(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, string text)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.Name = text;
        element.ElementType = GUIElement.ElementTypes.Label;

        Elements.Add(element);
        return element;
    }
}

[System.Serializable]
public class GUIElement
{
    public bool Enabled = true;
    public Rect Bounds = new Rect();

    protected bool NeedRebuild = false;

    protected Rect EfectiveBounds = new Rect();

    public GUIPanel.Alignments HAlignement = GUIPanel.Alignments.Absolute;
    public GUIPanel.Alignments VAlignement = GUIPanel.Alignments.Absolute;

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

    public virtual void Rebuild(Rect parrent)
    {
        float x = Bounds.xMin;
        if (HAlignement == GUIPanel.Alignments.Max)
            x = parrent.width - Bounds.xMin;
        else if (HAlignement == GUIPanel.Alignments.Center)
            x = (parrent.width * 0.5f) - (Bounds.width * 0.5f) - Bounds.xMin;

        float y = Bounds.xMin;
        if (VAlignement == GUIPanel.Alignments.Max)
            y = parrent.height - Bounds.yMin;
        else if (VAlignement == GUIPanel.Alignments.Center)
            y = (parrent.height * 0.5f) - (Bounds.height * 0.5f) - Bounds.yMin;


        EfectiveBounds = new Rect(parrent.xMin + x, parrent.yMin + y, Bounds.width, Bounds.height);

        foreach (GUIElement elemment in Children)
            Rebuild(EfectiveBounds);
    }

    public void Draw()
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

        foreach (GUIElement element in Children)
            Draw();
    }
}
