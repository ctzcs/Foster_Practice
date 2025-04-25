using System.Numerics;
using Foster.Framework;

namespace Engine.UI;

public class UiElement(bool maskable, bool selectable, bool visible, Rect rect, UiElement? parent)
{
    protected bool isDisabled = false;
    protected bool maskable = maskable;
    protected bool selectable = selectable;
    protected bool visible = visible;
    protected Rect rect = rect;
    protected UiElement? parent = parent;
    protected List<UiElement> children = new();
    
    public UiElement? Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public Rect Rect
    {
        get=>rect;
        set=>rect = value;
    }
    
    public bool Selectable
    {
        get => selectable;
        set => selectable = value;
    }

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }
    public void AddChild(UiElement child)
    {
        children.Add(child);
        child.Parent = parent;
    }

    public void RemoveChild(UiElement child)
    {
        children.Remove(child);
    }
    
    public void AfterUpdate(Batcher batcher)
    {
        if (!visible)return;
        Draw(batcher);
        foreach (var child in children)
        {
            child.AfterUpdate(batcher);
        }        
    }
    

    public virtual UiElement? Hit(Vector2 point)
    {
        
        if (!selectable)return null;
        
        if (rect.Contains(point))
        {
            /*Log.Info($"{rect} {point} hit");*/
            return this;
        }
        return null;
    }
    

    protected virtual void Draw(Batcher batcher){}
    
    
}

//Image,Text=>maskable
//Button,Dropdown,Slider,Scrollbar,Toggle,InputField=>selectable