using System.Numerics;
using Engine.Core.Structure;

namespace Engine.Physics.SpatialHash
{
    public class Chunk:IDisposable
    {
        public Vector2Int chunkIndex;
        public int chunkSize;
        public Grid[,] grids;
        
        public Chunk(int x,int y,int size)
        {
            chunkSize = size;
            chunkIndex = new Vector2Int(x, y);
            grids = new Grid[size,size];
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
            grids = new Grid[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grids[i, j] = new Grid(i,j);
                }
            }
        }
        
        public Grid GetGrid(Vector2Int index)
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
}