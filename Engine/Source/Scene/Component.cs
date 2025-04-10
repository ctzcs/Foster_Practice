namespace Engine.Scene;

public class Component : Behaviour
{
    //是否启用
    public bool IsEnabled { get; set; }
    public GameObject? GameObject { get; private set; }

    internal void AttachToGameObject(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    internal void DetachFromGameObject(GameObject gameObject)
    {
        GameObject = null!;
    }
}