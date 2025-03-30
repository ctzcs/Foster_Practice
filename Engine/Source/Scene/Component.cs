namespace Engine.Source.Scene;

public class Component:Behaviour
{
    //是否启用
    public bool IsEnabled { get; set; }
    public GameObject? GameObject{get;private set;}

    internal void AttachToGameObject(GameObject gameObject)
    {
        this.GameObject = gameObject;
    }

    internal void DetachFromGameObject(GameObject gameObject)
    {
        this.GameObject = null!;
    }

    
}