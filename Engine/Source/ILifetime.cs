namespace Engine;

public interface ILifetime
{
    void Start();
    void Destroy();
    void Update();
    void Render();
}