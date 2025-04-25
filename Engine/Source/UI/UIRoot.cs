using System.Numerics;
using Engine.Camera;
using Engine.UI.Base;
using Foster.Framework;

namespace Engine.UI;

public class UiRoot
{
	private Window window;
    private Input input;
    private Vector2 logicScreen;
    private UiElement root;
    
    private UiElement? lastOver;
    
    private UiFrame lastFrame;
    private UiFrame currentFrame;
    
    
    List<UiElement> _inputFocusListeners = new();

    public UiElement Root => root;
    public UiRoot(Input input,Window window,Vector2 logicScreen)
    {
	    this.input = input;
	    this.window = window;
	    this.logicScreen = logicScreen;
	    root = new Group();
	    lastFrame = new UiFrame();
	    currentFrame = new UiFrame();
    }
    
    public void Update()
    {
        UpdateInputMouse();
    }

    public void AfterUpdate(Batcher batcher)
    {
	    root.AfterUpdate(batcher);
    }


    void UpdateInputMouse()
    {
	    /*var lastState = input.LastState;
	    var currentState = input.State;*/
	    
	    lastFrame.CopyFrom(currentFrame);
	    currentFrame.inputState = input.State;
	    var pos = CameraExt.ViewportToLogicScreen(CameraExt.ScreenToViewport(currentFrame.Mouse.Position, window),logicScreen);
        currentFrame.targetPosition = pos;
	    UpdateInputPoint(lastFrame,currentFrame,ref lastOver);
    }
    
    void UpdateInputPoint(UiFrame lastState,UiFrame curState,
        ref UiElement lastOver)
    {
	    var over = root.Hit(curState.targetPosition);
	    var inputPress = curState.Mouse.LeftPressed;
	    var inputRelease = curState.Mouse.RightReleased;
	    var inputMoved = curState.targetPosition != lastState.targetPosition;
	    var secondaryInputPress = curState.Mouse.RightPressed;
	    var secondaryInputRelease = curState.Mouse.RightReleased;
	    //鼠标进入
	    //鼠标离开
	    //鼠标点击
	    //鼠标释放
	    //鼠标移动
	    //鼠标滚轮
	    //鼠标悬停
	    
	    if (over != null) HandleMouseWheel(over,curState);
	    if (inputPress) UpdatePrimaryInputDown(curState, over);
	    if (secondaryInputPress) UpdateSecondaryInputDown(curState, over);
	    if (inputMoved) UpdateInputMoved(curState, over,ref lastOver);
	    if (inputRelease) UpdatePrimaryInputReleased(curState);
	    if (secondaryInputRelease) UpdateSecondaryInputReleased(curState);
	    lastOver = over;
    }
    
    /// <summary>
	/// Mouse or touch is down this frame.
	/// </summary>
	/// <param name="inputPos">location of cursor</param>
	/// <param name="over">element under cursor</param>
	void UpdatePrimaryInputDown(UiFrame state,UiElement over)
	{
		// lose keyboard focus if we click outside of the keyboardFocusElement
		/*if (_keyboardFocusElement != null && over != _keyboardFocusElement)
			SetKeyboardFocus(null);*/

		// if we are over an element and the left button was pressed we notify our listener
		if (over is IInputListener)
		{
			//var elementLocal = over.StageToLocalCoordinates(inputPos);
			var listener = over as IInputListener;

			// add the listener to be notified for all onMouseDown and onMouseUp events
			listener.OnPointerDown(state);
			_inputFocusListeners.Add(over);
			
		}
	}


	/// <summary>
	/// Mouse or touch is down this frame.
	/// </summary>
	/// <param name="inputPos">location of cursor</param>
	/// <param name="over">element under cursor</param>
	void UpdateSecondaryInputDown(UiFrame state,UiElement over)
	{
		// lose keyboard focus if we click outside of the keyboardFocusElement
		/*if (_keyboardFocusElement != null && over != _keyboardFocusElement)
			SetKeyboardFocus(null);*/

		// if we are over an element and the left button was pressed we notify our listener
		if (over is IInputListener)
		{
			//var elementLocal = over.StageToLocalCoordinates(inputPos);
			var listener = over as IInputListener;

			// add the listener to be notified for all onMouseDown and onMouseUp events
			listener.OnRightPointerDown(state);
			_inputFocusListeners.Add(over);
			
		}
	}


	/// <summary>
	/// Mouse or touch is being moved.
	/// </summary>
	/// <param name="inputPos">location of cursor</param>
	/// <param name="over">element under cursor</param>
	/// <param name="lastOver">element that was previously under the cursor</param>
	void UpdateInputMoved(UiFrame state,UiElement over,ref UiElement lastOver)
	{
		for (var i = _inputFocusListeners.Count - 1; i >= 0; i--)
			((IInputListener)_inputFocusListeners[i]).OnPointerMoved(state);

		if (over != lastOver)
		{
			(over as IInputListener)?.OnPointerEnter(state);
			(lastOver as IInputListener)?.OnPointerExit(state);
		}
	}


	/// <summary>
	/// Mouse or touch is being released this frame.
	/// </summary>
	/// <param name="state"></param>
	void UpdatePrimaryInputReleased(UiFrame state)
	{
		for (var i = _inputFocusListeners.Count - 1; i >= 0; i--)
			((IInputListener)_inputFocusListeners[i]).OnPointerUp(state);
		_inputFocusListeners.Clear();
	}

	/// <summary>
	/// Right mouse click or touch is being released this frame.
	/// </summary>
	/// <param name="inputPos">location under cursor</param>
	void UpdateSecondaryInputReleased(UiFrame state)
	{
		for (var i = _inputFocusListeners.Count - 1; i >= 0; i--)
			((IInputListener)_inputFocusListeners[i]).OnRightPointerUp(state);
		_inputFocusListeners.Clear();
	}


	/// <summary>
	/// bubbles the onMouseScrolled event from mouseOverElement to all parents until one of them handles it
	/// </summary>
	/// <returns>The mouse wheel.</returns>
	/// <param name="mouseOverElement">Mouse over element.</param>
	void HandleMouseWheel(UiElement mouseOverElement,UiFrame curstate)
	{
		// bail out if we have no mouse wheel motion
		if (curstate.Mouse.Wheel.Y == 0)
			return;

		// check the deepest Element first then check all of its parents that are IInputListeners
		var listener = mouseOverElement as IInputListener;
		if (listener != null && listener.OnMouseScrolled(curstate))
			return;

		while (mouseOverElement.Parent != null)
		{
			mouseOverElement = mouseOverElement.Parent;
			listener = mouseOverElement as IInputListener;
			if (listener != null && listener.OnMouseScrolled(curstate))
				return;
		}
	}
}
