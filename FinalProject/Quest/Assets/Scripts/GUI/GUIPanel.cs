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

    public static void DrawAll()
    {
        foreach (GUIPanel panel in Pannels)
            panel.Draw();
    }

    public bool Enabled = true;

    public List<GUIElement> Elements = new List<GUIElement>();
    public Texture Background;
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
      //  Debug.Log("Rebuild");

        NeedRebuild = false;

      //  Debug.Log(Bounds.ToString());

        float x = Bounds.xMin;
        if (HAlignement == Alignments.Max)
            x = Camera.main.pixelWidth - Bounds.width - Bounds.xMin;
        else if (HAlignement == Alignments.Center)
            x = (Camera.main.pixelWidth * 0.5f) - (Bounds.width * 0.5f) - Bounds.xMin;

        float y = Bounds.xMin;
        if (VAlignement == Alignments.Max)
            y = Camera.main.pixelHeight - Bounds.height - Bounds.yMin;
        else if (VAlignement == Alignments.Center)
            y = (Camera.main.pixelHeight * 0.5f) - (Bounds.height * 0.5f) - Bounds.yMin;

        EfectiveBounds = new Rect(x, y, Bounds.width, Bounds.height);

        Rect relativeBounds = new Rect(0, 0, Bounds.width, Bounds.height);
        foreach (GUIElement element in Elements)
            element.Rebuild(relativeBounds);
    }

    public void Draw()
    {
        if (!Enabled)
            return;

        if (NeedRebuild)
            Rebuild();

        PreDraw();
        if (Background != null)
            GUI.Window(ID, EfectiveBounds, Update, Background, Style);
        else
            GUI.Window(ID, EfectiveBounds, Update, Name, Style);
    }

    public void Update(int id)
    {
        int depthSave = GUI.depth;
        GUI.depth = 100;
        foreach (GUIElement element in Elements)
            element.Draw();
        GUI.depth = depthSave;
    }

    public virtual void PreDraw()
    {

    }

    public GUIElement NewImageButton(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, Texture image, EventHandler handler)
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

    public GUIElement NewImage(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, Texture image)
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

    public GUIElement NewImage(Alignments hAlign, float x, Alignments vAlign, float y, Texture image)
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
    public int ID = 0;

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

    public Texture BackgroundImage = null;
    public string Name = string.Empty;

    public virtual void Rebuild(Rect parrent)
    {
        float x = parrent.xMin + Bounds.xMin;
        if (HAlignement == GUIPanel.Alignments.Max)
            x = parrent.xMax - Bounds.width - Bounds.xMin;
        else if (HAlignement == GUIPanel.Alignments.Center)
            x = parrent.xMin + ((parrent.width * 0.5f) - (Bounds.width * 0.5f) + Bounds.xMin);

        float y = parrent.yMin + Bounds.yMin;
        if (VAlignement == GUIPanel.Alignments.Max)
            y = parrent.yMax - Bounds.height - Bounds.yMin;
        else if (VAlignement == GUIPanel.Alignments.Center)
            y = parrent.yMin + (parrent.height * 0.5f) - (Bounds.height * 0.5f) + Bounds.yMin;

        EfectiveBounds = new Rect(x, y, Bounds.width, Bounds.height);

        Debug.Log("Building " + this.ToString() + " : " + Name);
        Debug.Log("Parrent " + parrent.ToString()); 
        Debug.Log("Bounds " + EfectiveBounds.ToString());

        foreach (GUIElement elemment in Children)
            elemment.Rebuild(EfectiveBounds);
    }

    public void Draw()
    {
        if (!Enabled)
            return;

        GUI.depth--;
        switch (ElementType)
        {
            case ElementTypes.Label:
                GUI.Label(EfectiveBounds, Name);
                break;

            case ElementTypes.Image:
                GUI.DrawTexture(EfectiveBounds, BackgroundImage);
                break;

            case ElementTypes.Button:
                if (GUI.Button(EfectiveBounds,Name))
                {
                    if (Clicked != null)
                        Clicked(this, EventArgs.Empty);
                }
                break;
        }

        foreach (GUIElement element in Children)
            element.Draw();
        GUI.depth++;
    }

    public GUIElement NewImageButton(GUIPanel.Alignments hAlign, float x, GUIPanel.Alignments vAlign, float y, float width, float height, Texture image, EventHandler handler)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Button;

        element.Clicked += handler;

        Children.Add(element);
        return element;
    }

    public GUIElement NewImage(GUIPanel.Alignments hAlign, float x, GUIPanel.Alignments vAlign, float y, float width, float height, Texture image)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Image;

        Children.Add(element);
        return element;
    }

    public GUIElement NewImage(GUIPanel.Alignments hAlign, float x, GUIPanel.Alignments vAlign, float y, Texture image)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, image.width, image.height);
        element.BackgroundImage = image;
        element.ElementType = GUIElement.ElementTypes.Image;

        Children.Add(element);
        return element;
    }

    public GUIElement NewLabel(GUIPanel.Alignments hAlign, float x, GUIPanel.Alignments vAlign, float y, float width, float height, string text)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.Name = text;
        element.ElementType = GUIElement.ElementTypes.Label;

        Children.Add(element);
        return element;
    }
}
