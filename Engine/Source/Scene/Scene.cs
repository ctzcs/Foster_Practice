namespace Engine.Source.Scene;

public class Scene:ILifetime
{
    public List<GameObject> AddGameObjects = new List<GameObject>();
    public List<GameObject> RemoveGameObjects = new List<GameObject>();
    public List<Component> AddComponents = new List<Component>();
    public List<Component> RemoveComponents = new List<Component>();
    
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
}