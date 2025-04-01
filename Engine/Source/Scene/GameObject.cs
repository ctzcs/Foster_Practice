namespace Engine.Source.Scene;

public class GameObject : Behaviour
{
    private readonly List<Component> components = [];

    public Scene? Scene { get; private set; }

    //是否激活
    public bool IsActive { get; set; }

    internal void AttachToScene(Scene scene)
    {
        Scene = scene;
    }

    internal void DetachFromScene()
    {
        Scene = null;
    }


    public T Add<T>(T component) where T : Component
    {
        components.Add(component);
        component.AttachToGameObject(this);
        component.Awake();
        return component;
    }

    public void Remove<T>(T behaviour) where T : Component
    {
        components.Remove(behaviour);
    }

    public void Clear()
    {
        for (var i = 0; i < components.Count; i++) components[i].Destroy();
        components.Clear();
    }

    public override void Update()
    {
        if (!IsActive) return;

        for (var i = 0; i < components.Count; i++)
        {
            if (components[i].IsEnabled == false) continue;
            components[i].Update();
        }
    }

    public override void Destroy()
    {
        IsActive = false;
        Clear();
    }
}