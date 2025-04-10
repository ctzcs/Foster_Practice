

namespace Engine.Physics.SpatialHash
{
    public class CollisionSystemInspector
    {
        private void OnDrawGizmos()
        {
            CollisionSystem.I.DrawChunk();
        }

        private void OnDestroy()
        {
            CollisionSystem.I.Clear();
        }
    }
}