using System.Numerics;
using Foster.Framework;

namespace Engine.UI.Widgets;

public class Button(bool maskable, bool selectable, bool visible, Rect rect, UiElement? parent) 
    :UiElement(maskable, selectable, visible, rect, parent),IInputListener
{
    protected bool _mouseOver, _mouseDown;
    private Color _color = Color.Blue;
    protected override void Draw(Batcher batcher)
    {
        batcher.Quad(rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft,_color);
    }

    public void OnPointerEnter(UiFrame state)
    {
        _color = Color.Red;
        Log.Info("OnPointerEnter");
    }
    
    public void OnPointerExit(UiFrame state)
    {
        _color = Color.Gray;
        Log.Info("OnPointerExit");
    }

    bool IInputListener.OnPointerDown(UiFrame state)
    {
        if (isDisabled)
            return false;

        _mouseDown = true;
        return true;
    }

    bool IInputListener.OnRightPointerDown(UiFrame state)
    {
        if (isDisabled)
            return false;

        _mouseDown = true;
        return true;
    }
}