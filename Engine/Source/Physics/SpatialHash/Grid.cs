namespace Engine.Physics.SpatialHash
{
    public class Grid
    {
        public int x;
        public int y;
        public List<IEntityObj> entities = new(16);

        public Grid(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public void Add(IEntityObj obj)
        {
            entities.Add(obj);
        }

        public void Remove(IEntityObj obj)
        {
            //没想到上面这个更慢
            /*var index = entities.IndexOf(obj);
            int count = entities.Count;
            if (count == 0)
            {
                return;
            }
            if (index < entities.Count && index >= 0)
            {
                entities[index] = entities[^1];
            }
            entities.RemoveAt(count - 1);*/

            entities.Remove(obj);
        }

        public void GetAll(List<IEntityObj> entityObjs)
        {
            entityObjs.AddRange(entities);
        }
        
        public List<IEntityObj> GetAll() => entities;
    }
}