using System.Numerics;
using Foster.Framework;

namespace Engine.UI;

public class UiFrame
{
    public InputState inputState;
    public Vector2 targetPosition;
    
    public MouseState Mouse=>inputState.Mouse;


    public void CopyFrom(UiFrame frame)
    {
        this.inputState = frame.inputState;
        this.targetPosition = frame.targetPosition;
    }
}