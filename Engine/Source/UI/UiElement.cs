using Foster.Framework;

namespace Engine.Source.UI;

public class UiElement
{
    /// <summary>
    /// 元素的矩形
    /// </summary>
    private Rect rect;
    private bool visible;
    private bool selectable;
    private bool maskable;
    
}
//Image,Text=>maskable
//Button,Dropdown,Slider,Scrollbar,Toggle,InputField=>selectable

