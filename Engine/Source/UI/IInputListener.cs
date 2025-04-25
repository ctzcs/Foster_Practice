using System.Numerics;
using Foster.Framework;

namespace Engine.UI;

public interface IInputListener
{
    bool OnMouseScrolled(UiFrame state)=>false;
    void OnPointerMoved(UiFrame state){}
    void OnPointerEnter(UiFrame state) {}
    //void OnPointerStay(InputState state){}
    void OnPointerExit(UiFrame state){}
    
    bool OnPointerDown(UiFrame state);
    void OnPointerUp(UiFrame state){}
    //void OnPointerClick(InputState state){}

    bool OnRightPointerDown(UiFrame state);
    void OnRightPointerUp(UiFrame state){}
    //void OnRightPointerClick(InputState state){}
    
}