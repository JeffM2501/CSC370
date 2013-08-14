using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GUIPanel : IDisposable
{
    public static List<GUIPanel> Pannels = new List<GUIPanel>();

    public GUISkin Skin = null;

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

    public GUIStyle ToolTipStyle = new GUIStyle();
    public Texture ToolTipBackgorund = null;
    public Rect ToolTipRect = new Rect();
    public Vector2 ToolTipOffset = Vector2.zero;

    protected string ToolTipString = string.Empty;

    public bool KeepToolTipInBounds = false;

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
        ToolTipStyle.normal.textColor = Color.white;
    }

    protected void Close(object sender, EventArgs args)
    {
        Enabled = false;
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

        if (Skin == null)
        {
            if (GameState.Instance.GUI != null && GameState.Instance.GUI.Skin != null)
                GUI.skin = GameState.Instance.GUI.Skin;
        }
        else
            GUI.skin = Skin;

        PreDraw();
        ToolTipString = string.Empty;
        if (Background != null)
            GUI.Window(ID, EfectiveBounds, Update, Background, Style);
        else
            GUI.Window(ID, EfectiveBounds, Update, Name, Style);

//          if (ToolTipString != string.Empty)
//              DrawToolTipWindow();
    }

    public void DrawToolTipWindow()
    {
        Debug.Log(ToolTipString);
        Rect tipBounds = new Rect(Input.mousePosition.x + ToolTipOffset.x, Input.mousePosition.y + ToolTipOffset.y, ToolTipRect.width, ToolTipRect.height);

        if (ToolTipBackgorund != null)
        {
            tipBounds.width = ToolTipBackgorund.width;
            tipBounds.height = ToolTipBackgorund.height;
        }

        GUI.Window(-ID, tipBounds, DrawToolTip, ToolTipBackgorund, ToolTipStyle == null ? Style : ToolTipStyle);
    }

    public void DrawToolTip(int id)
    {
        Rect labelRect = ToolTipRect;
        
        if (ToolTipBackgorund == null)
            labelRect = new Rect(0,0, ToolTipRect.width, ToolTipRect.height);
        GUI.Label(labelRect, GUI.tooltip, ToolTipStyle);
    }

    public void Update(int id)
    {
        int depthSave = GUI.depth;
        GUI.depth = 100;
        foreach (GUIElement element in Elements)
            element.Draw();
        GUI.depth = depthSave;

        if (GUI.tooltip != string.Empty)
            ToolTipString = GUI.tooltip.Clone() as string;

        if (true)
        {
            if (GUI.tooltip != string.Empty)
            {
                float ttOffsetY = 0;
                if (ToolTipBackgorund != null)
                    ttOffsetY = ToolTipBackgorund.height;
                ttOffsetY += ToolTipOffset.y;

             //   float width = ToolTipRect.width;

                float x = Input.mousePosition.x - Bounds.x;
                float y = Camera.main.pixelHeight - Input.mousePosition.y - Bounds.y;

                Vector2 ttBounds = new Vector2(ToolTipRect.width, ToolTipRect.height);
                if (ToolTipBackgorund != null)
                    ttBounds = new Vector2(ToolTipBackgorund.width, ToolTipBackgorund.height);

                if (KeepToolTipInBounds)
                {
                    if (x < 0)
                        x = 1;
                    if (y < 0)
                        y = 1;

                    if (x + ttBounds.x > Bounds.width)
                        x -= (x + ttBounds.x) - Bounds.width;

                    if (y + ttBounds.y > Bounds.height)
                        y -= (y + ttBounds.y) - Bounds.height;
                }

                if (ToolTipBackgorund != null)
                {
                    Rect pos = new Rect(x + ToolTipOffset.x, y - ToolTipBackgorund.height + ToolTipOffset.y, ToolTipBackgorund.width, ToolTipBackgorund.height);
                    GUI.Box(pos, ToolTipBackgorund, ToolTipStyle);
                }

                Rect labelRect = new Rect(x + ToolTipOffset.x + ToolTipRect.x, y + ToolTipOffset.y + ToolTipRect.y, ToolTipRect.width, ToolTipRect.height);
                GUI.Label(labelRect, GUI.tooltip, ToolTipStyle);
            }
        }
    }

    protected void ToolTipWindow()
    {
        float ttOffsetY = 0;
        if (ToolTipBackgorund != null)
            ttOffsetY = ToolTipBackgorund.height;
        ttOffsetY += ToolTipOffset.y;

      //  float width = ToolTipRect.width;

        float x = Input.mousePosition.x - Bounds.x;
        float y = Camera.main.pixelHeight - Input.mousePosition.y - Bounds.y;
        if (ToolTipBackgorund != null)
        {
            Rect pos = new Rect(x + ToolTipOffset.x, y - ToolTipBackgorund.height + ToolTipOffset.y, ToolTipBackgorund.width, ToolTipBackgorund.height);
            GUI.Box(pos, ToolTipBackgorund, ToolTipStyle);
        }

        Rect labelRect = new Rect(x + ToolTipOffset.x + ToolTipRect.x, y + ToolTipOffset.y + ToolTipRect.y, ToolTipRect.width, ToolTipRect.height);
        GUI.Label(labelRect, GUI.tooltip, ToolTipStyle);
    }


    public virtual void PreDraw()
    {

    }

    public GUIElement NewButton(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, string text, EventHandler handler)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.Name = text;
        element.ElementType = GUIElement.ElementTypes.Button;

        element.Clicked += handler;

        Elements.Add(element);
        return element;
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

    public GUIElement NewImageButton(Alignments hAlign, float x, Alignments vAlign, float y, Texture image, EventHandler handler)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, image.width, image.height);
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

    public GUIElement NewFrame(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.ElementType = GUIElement.ElementTypes.Frame;
        Elements.Add(element);
        return element;
    }

    public GUIElement NewScrollView(Alignments hAlign, float x, Alignments vAlign, float y, float width, float height, Rect innerSize, bool horizontal)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, width, height);
        element.ElementType = horizontal ? GUIElement.ElementTypes.HorizontalScrollFrame : GUIElement.ElementTypes.VerticalScrollFrame;
        element.InnerSize = innerSize;
        Elements.Add(element);
        return element;
    }
}

[System.Serializable]
public class GUIElement
{
    protected Vector2 ScrollPoition = Vector2.zero;

    public int FontSize = -1;
    public Color FontColor = Color.clear;

    public GUIStyle TextStyle = null;

    public int ID = 0;

    public bool Enabled = true;
    public Rect Bounds = new Rect();

    protected bool NeedRebuild = false;

    protected Rect EfectiveBounds = new Rect();

    public Rect InnerSize = new Rect();

    public GUIPanel.Alignments HAlignement = GUIPanel.Alignments.Absolute;
    public GUIPanel.Alignments VAlignement = GUIPanel.Alignments.Absolute;

    public object Tag = null;


    public string ToolTip = string.Empty;

    [System.Serializable]
    public enum ElementTypes
    {
        Label,
        Frame,
        Button,
        Image,
        VerticalScrollFrame,
        HorizontalScrollFrame,
    }

    public ElementTypes ElementType = ElementTypes.Frame;

    public List<GUIElement> Children = new List<GUIElement>();

    public event EventHandler Clicked;

    public Texture BackgroundImage = null;
    public string Name = string.Empty;

    public void Clear()
    {
        Children.Clear();
    }

    public void SetFont(Color color, int fontSize)
    {
        FontColor = color;
        FontSize = fontSize;
    }

    public void BuildFontStuff()
    {
        TextStyle = new GUIStyle();
        TextStyle.fontSize = FontSize;
        TextStyle.normal.textColor = FontColor;
    }

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

    //    Debug.Log("Building " + this.ToString() + " : " + Name);
    //    Debug.Log("Parrent " + parrent.ToString()); 
     //   Debug.Log("Bounds " + EfectiveBounds.ToString());

        if (ElementType == ElementTypes.HorizontalScrollFrame || ElementType == ElementTypes.VerticalScrollFrame)
        {
            foreach (GUIElement elemment in Children)
                elemment.Rebuild(Bounds);
        }
        else
        {
            foreach (GUIElement elemment in Children)
                elemment.Rebuild(EfectiveBounds);
        }
        
    }

    public void Draw()
    {
        if (!Enabled)
            return;

        GUI.depth--;
        GUIContent context = null;
        switch (ElementType)
        {
            case ElementTypes.Label:
                {
                    if (FontColor != Color.clear && FontSize > 0 && TextStyle == null)
                        BuildFontStuff();

                    context = new GUIContent(Name, null, ToolTip);

                    if (TextStyle != null)
                        GUI.Label(EfectiveBounds, context, TextStyle);
                    else
                        GUI.Label(EfectiveBounds, context);
                }
                
                break;

            case ElementTypes.Image:
                GUI.DrawTexture(EfectiveBounds, BackgroundImage);
                break;

            case ElementTypes.Button:

                bool click = false;

                context = new GUIContent(Name, BackgroundImage, ToolTip);
//                 if (BackgroundImage != null)
//                     click = GUI.Button(EfectiveBounds, BackgroundImage);
//                 else
                click = GUI.Button(EfectiveBounds, context);

                if (click)
                {
                    if (Clicked != null)
                        Clicked(this, EventArgs.Empty);
                }
                break;

            case ElementTypes.Frame:
                GUI.BeginGroup(EfectiveBounds);
                break;

            case ElementTypes.VerticalScrollFrame:
                ScrollPoition = GUI.BeginScrollView(EfectiveBounds, Vector2.zero, InnerSize,false,true);
                break;

            case ElementTypes.HorizontalScrollFrame:
                ScrollPoition = GUI.BeginScrollView(EfectiveBounds, Vector2.zero, InnerSize, true, false);
                break;
        }

        foreach (GUIElement element in Children)
            element.Draw();

        if (ElementType == ElementTypes.Frame)
            GUI.EndGroup();
        else if (ElementType == ElementTypes.HorizontalScrollFrame || ElementType == ElementTypes.VerticalScrollFrame)
            GUI.EndScrollView();

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

    public GUIElement NewImageButton(GUIPanel.Alignments hAlign, float x, GUIPanel.Alignments vAlign, float y, Texture image, EventHandler handler)
    {
        GUIElement element = new GUIElement();

        element.HAlignement = hAlign;
        element.VAlignement = vAlign;
        element.Bounds = new Rect(x, y, image.width, image.height);
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
