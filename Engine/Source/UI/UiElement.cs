using Foster.Framework;

namespace Engine.Source.UI;

public class UiElement
{
    private bool maskable;

    /// <summary>
    ///     元素的矩形
    /// </summary>
    private Rect rect;

    private bool selectable;
    private bool visible;
}
//Image,Text=>maskable
//Button,Dropdown,Slider,Scrollbar,Toggle,InputField=>selectable