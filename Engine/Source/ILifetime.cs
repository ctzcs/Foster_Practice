namespace Engine.Source;

public interface ILifetime
{
    void Awake();
    void Start();
    void Destroy();
    void Update();
    void Render();
}