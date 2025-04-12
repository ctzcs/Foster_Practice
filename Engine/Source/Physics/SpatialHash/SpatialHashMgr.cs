
using System.Numerics;
using Arch.AOT.SourceGenerator;
using Arch.Core;
using Engine.Core.Structure;
using Engine.Utility;

namespace Engine.Physics.SpatialHash;

/// <summary>
/// 左下角是第一个chunk,横着摆
/// TODO OnPositionChange 的时候调用MoveTo
/// </summary>

public class SpatialHashMgr
{
    private int _chunkSize = 16;
    private float gridSize = 1f;
    private Vector2 startPos = new Vector2(-10, -10);
    
    public Dictionary<Vector2Int, Chunk> Chunks = [];
    private Dictionary<Vector2Int, List<Entity>> _removeTemp = [];
    private Dictionary<Vector2Int, List<Entity>> _addTemp = [];

    #region Obsolete
    
    public void GEntityObjs(Vector2Int gridIndex, int range, List<Entity> entityObjs)
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
                    entityObjs.AddRange(grid.GetAll());
                }

            }
        }
    }

    public void Add(Entity obj, in Transform.Transform transform)
    {
        var pos = transform.position;
        var gridIndex = VectorToIndex(pos);
        if (Chunks.TryGetValue(GetChunkIndex(gridIndex, _chunkSize), out var chunk))
        {
            var grid = chunk.GetGrid(GetIndexInChunk(gridIndex, _chunkSize));
            grid.Add(obj);
        }
    }

    public void Remove(Entity obj, in SpatialHash spatialHash)
    {
        if (Chunks.TryGetValue(spatialHash.chunkIndex, out var chunk))
        {
            var grid = chunk.GetGrid(spatialHash.gridIndex);
            grid.Remove(obj);
        }
    }

    /// <summary>
    /// 从一个格子移动到另一个格子
    /// </summary>
    /// <param name="obj"></param>
    public void MoveTo(Entity obj, ref SpatialHash spatialHash, in Transform.Transform transform)
    {
        var preIndex = spatialHash.index;
        var preChunkIndex = GetChunkIndex(preIndex, _chunkSize);
        var pos = transform.position;
        var nowIndex = VectorToIndex(pos);
        var gridChunkIndex = GetChunkIndex(nowIndex, _chunkSize);
        if (preIndex == nowIndex)
        {
            return;
        }

        if (!Chunks.TryGetValue(preChunkIndex, out var preChunk))
        {
            preChunk = new Chunk(preChunkIndex, _chunkSize);
            Chunks.TryAdd(preChunkIndex, preChunk);
        }

        preChunk.GetGrid(spatialHash.gridIndex).Remove(obj);

        if (!Chunks.TryGetValue(gridChunkIndex, out var chunk))
        {
            chunk = new Chunk(gridChunkIndex, _chunkSize);
            Chunks.TryAdd(gridChunkIndex, chunk);
        }

        var gridIndex = GetIndexInChunk(nowIndex, _chunkSize);
        chunk.GetGrid(gridIndex).Add(obj);
        spatialHash.index = nowIndex;
        spatialHash.chunkIndex = gridChunkIndex;
        spatialHash.gridIndex = gridIndex;
    }

    public Grid? GetGrid(Vector2Int index)
    {
        return Chunks.TryGetValue(GetChunkIndex(index, _chunkSize), out var chunk)
            ? chunk.GetGrid(GetIndexInChunk(index, _chunkSize))
            : null;
    }

    /// <summary>
    /// 获取整体索引
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2Int VectorToIndex(Vector2 position)
    {
        Vector2 oppositePos = position - startPos;
        int x = Mathf.FloorToInt(oppositePos.X / gridSize);
        int y = Mathf.FloorToInt(oppositePos.Y / gridSize);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// 获取Chunk索引
    /// </summary>
    /// <param name="gridIndex"></param>
    /// <returns></returns>
    public static Vector2Int GetChunkIndex(Vector2Int gridIndex, int chunkSize)
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
    public static Vector2Int GetIndexInChunk(Vector2Int gridIndex, int chunkSize)
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
            chunk.DrawChunk(startPos, gridSize);
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
    #endregion

}

public class Chunk:IDisposable
{
    public Vector2Int chunkIndex;
    public int chunkSize;
    public Grid?[,] grids;
    
    public Chunk(int x,int y,int size)
    {
        chunkSize = size;
        chunkIndex = new Vector2Int(x, y);
        grids = new Grid?[size,size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grids[i, j] = new Grid(i,j);
            }
        }
    }

    public Chunk(Vector2Int chunkIndex,int size)
    {
        chunkSize = size;
        this.chunkIndex = chunkIndex;
        grids = new Grid?[size,size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grids[i, j] = new Grid(i,j);
            }
        }
    }
    
    public Grid? GetGrid(Vector2Int index)
    {
        return grids[index.X, index.Y];
    }


    public void DrawChunk(Vector2 startPos,float gridSize)
    {
#if UNITY_EDITOR
        Vector2 chunkStartPos = startPos + new Vector2(chunkIndex.x * chunkSize * gridSize, chunkIndex.y *chunkSize* gridSize);
        Vector2 chunkEndPos = chunkStartPos + new Vector2(chunkSize * gridSize,chunkSize* gridSize);
        Vector2 br = new Vector3(chunkEndPos.x, chunkStartPos.y);
        Vector2 lt = new Vector3(chunkStartPos.x,chunkEndPos.y);
        Gizmos.DrawLine(chunkStartPos,br);
        Gizmos.DrawLine(br,chunkEndPos);
        Gizmos.DrawLine(chunkEndPos,lt);
        Gizmos.DrawLine(lt,chunkStartPos);
        Handles.Label((chunkStartPos + chunkEndPos)/2,chunkIndex.ToString());
#endif
        
    }

    public void Dispose()
    {
        grids = null;
    }
}


public class Grid
{
    public Vector2Int gridIndex;
    public List<Entity> entities = new(16);

    public Grid(int x, int y)
    {
        gridIndex = new Vector2Int(x, y);
    }
        
    public void Add(Entity obj)
    {
        entities.Add(obj);
    }

    public void Remove(Entity obj)
    {
        entities.Remove(obj);
    }
        
    public List<Entity> GetAll() => entities;
}
