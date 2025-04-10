namespace Engine.Scene;

public class Scene : ILifetime
{
    public List<Component> AddComponents = new();
    public List<GameObject> AddGameObjects = new();
    public List<Component> RemoveComponents = new();
    public List<GameObject> RemoveGameObjects = new();


    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Destroy()
    {
    }

    public virtual void Update()
    {
        //TODO 激活，移除go
        //TODO 激活，移除component
    }

    public virtual void Render()
    {
    }

    public GameObject Instantiate(GameObject gameObject)
    {
        gameObject.AttachToScene(this);
        AddGameObjects.Add(gameObject);
        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        gameObject.DetachFromScene();
        RemoveGameObjects.Add(gameObject);
    }

    public void DestroyImmediate(GameObject gameObject)
    {
        gameObject?.Destroy();
    }
}