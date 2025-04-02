namespace Engine.Source;

public interface ILifetime
{
    void Start();
    void Destroy();
    void Update();
    void Render();
}