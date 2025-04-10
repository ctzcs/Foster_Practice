using System.Collections.Concurrent;
using System.Numerics;
using Engine.Core.Structure;
using Engine.Utility;

namespace Engine.Physics.SpatialHash
{
    public class CollisionSystem
    {

        private static readonly Lazy<CollisionSystem> _i = new Lazy<CollisionSystem>(new CollisionSystem());
        public static CollisionSystem I => _i.Value;

        private int _chunkSize = 16;
        
        private Vector2 startPos = new Vector2(-10,-10);
        private float gridSize = 1f;
        
        
        
        public ConcurrentDictionary<Vector2Int, Chunk> Chunks = new ConcurrentDictionary<Vector2Int, Chunk>();
        private Dictionary<Vector2Int, List<IEntityObj>> _removeTemp = new();
        private Dictionary<Vector2Int, List<IEntityObj>> _addTemp = new();
        public void Init(){}

        public void AddChunk(){}
        public void RemoveChunk(){}


        public void GEntityObjs(Vector2Int gridIndex,int range,List<IEntityObj> entityObjs)
        {
            Vector2Int start = new Vector2Int(gridIndex.X - range, gridIndex.Y - range);
            int size = range * 2 + 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector2Int index = start + new Vector2Int(i, j);
                    var chunkIndex = GetChunkIndex(index, _chunkSize);
                    var indexInChunk = GetIndexInChunk(index, _chunkSize);
                    if (Chunks.TryGetValue(chunkIndex, out var chunk))
                    {
                        var grid = chunk.GetGrid(indexInChunk);
                        grid.GetAll(entityObjs);
                    }

                }
            }
        }

        public void Add(IEntityObj obj)
        {
            var pos = obj.Position;
            var gridIndex= VectorToIndex(pos);
            if (Chunks.TryGetValue(GetChunkIndex(gridIndex,_chunkSize),out var chunk))
            {
                var grid = chunk.GetGrid(GetIndexInChunk(gridIndex,_chunkSize));
                grid.Add(obj);
            }
        }

        public void Remove(IEntityObj obj)
        {
            var pos = obj.PrePosition;
            var gridIndex= VectorToIndex(pos);
            if (Chunks.TryGetValue(GetChunkIndex(gridIndex,_chunkSize),out var chunk))
            {
                var grid = chunk.GetGrid(GetIndexInChunk(gridIndex,_chunkSize));
                grid.Remove(obj);
            }
        }

        /// <summary>
        /// 从一个格子移动到另一个格子
        /// </summary>
        /// <param name="obj"></param>
        public void MoveTo(IEntityObj obj)
        {
            var prePos = obj.PrePosition;
            var preIndex= VectorToIndex(prePos);
            var preChunkIndex = GetChunkIndex(preIndex,_chunkSize);
            var pos = obj.Position;
            var nowIndex= VectorToIndex(pos);
            var gridChunkIndex = GetChunkIndex(nowIndex,_chunkSize);
            if (preIndex == nowIndex)
            {
                return;
            }
            
            if (!Chunks.TryGetValue(preChunkIndex,out var preChunk))
            {
                preChunk = new Chunk(preChunkIndex,_chunkSize);
                Chunks.TryAdd(preChunkIndex,preChunk);
            }
            preChunk.GetGrid(GetIndexInChunk(preIndex,_chunkSize)).Remove(obj);
            
            if (!Chunks.TryGetValue(gridChunkIndex,out var chunk))
            {
                chunk = new Chunk(gridChunkIndex,_chunkSize);
                Chunks.TryAdd(gridChunkIndex,chunk);
            }
            chunk.GetGrid(GetIndexInChunk(nowIndex,_chunkSize)).Add(obj);

        }
        
        public Grid GetGrid(Vector2Int gridIndex)
        {
            return Chunks.TryGetValue(GetChunkIndex(gridIndex,_chunkSize),out var chunk) ? 
                chunk.GetGrid(GetIndexInChunk(gridIndex,_chunkSize)) : null;
        }


        public Vector2Int VectorToIndex(Vector2 position)
        {
            Vector2 oppositePos = position - startPos;
            int x = Mathf.FloorToInt(oppositePos.X / gridSize) ;
            int y = Mathf.FloorToInt(oppositePos.Y / gridSize);
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// 获取Chunk索引
        /// </summary>
        /// <param name="gridIndex"></param>
        /// <returns></returns>
        public static Vector2Int GetChunkIndex(Vector2Int gridIndex,int chunkSize)
        {
            int x = gridIndex.X;
            int y = gridIndex.Y;
            if (x < 0)
            {
                x = x / chunkSize - 1;
            }
            else
            {
                x /= chunkSize;
            }
            
            if (y < 0)
            {
                y = y / chunkSize - 1;
            }
            else
            {
                y /= chunkSize;
            }
            return new Vector2Int(x, y);
        }


        /// <summary>
        /// 获取索引在Chunk中的位置
        /// </summary>
        /// <param name="gridIndex"></param>
        /// <returns></returns>
        public static Vector2Int GetIndexInChunk(Vector2Int gridIndex,int chunkSize)
        {
            int x = gridIndex.X % chunkSize;
            int y = gridIndex.Y % chunkSize;
            if (x < 0)
            {
                x = chunkSize + x;
            }

            if (y < 0)
            {
                y = chunkSize + y;
            }
            return new Vector2Int(x, y);
        }


        public void DrawChunk()
        {
            foreach (var chunk in Chunks.Values)
            {
                chunk.DrawChunk(startPos,gridSize);
            }
        }


        public void Clear()
        {
            foreach (var chunk in Chunks.Values)
            {
                chunk.Dispose();
            }
            Chunks.Clear();
        }
    }
    
    
    
    //左下角是第一个chunk
    //横着摆
    
}