using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.AOT.SourceGenerator;
using Arch.Core;
using Arch.Core.Extensions;

namespace Engine.Transform;

[Component]
public struct Transform
{
    #region Relationship
    public Entity parent;
    
    public List<Entity> children;
    
    #endregion
    
    #region Position
    /// <summary>
    /// Local用来表示相对父节点的偏移
    /// </summary>
    public Vector2 localPosition;

    /// WorldPos用来渲染
    public Vector2 position;
    
    public Matrix3x2 localTransform;
    public Matrix3x2 worldTransform;
    /*public Matrix3x2 worldToLocalTransform;
    public Matrix3x2 worldInverseTransform;*/
    public Matrix3x2 translationMatrix;
    #endregion
    
    #region Rotation
    public float localRad;
    //弧度制
    public float rad;
    public Matrix3x2 rotationMatrix;
    
    #endregion
    
    #region Scale
    public Vector2 localScale;
    public Vector2 scale;
    public Matrix3x2 scaleMatrix;
    #endregion
    
    

    #region DirtyTag

    [Flags]
    public enum EDirtyType
    {
        Clean = 0,
        PositionDirty = 1,
        ScaleDirty = 2,
        RotationDirty = 4
    }
    
    public EDirtyType hierarchyDirty;

    //public bool localDirty;
    /*public bool localPositionDirty;
    public bool localScaleDirty;
    public bool localRotationDirty;*/
    //public bool positionDirty;
    

    #endregion
    

    public Transform(Entity parent,Vector2 localPosition,float localRad,Vector2 localScale)
    {
        this.parent = parent;
        this.localPosition = localPosition;
        this.localRad = localRad;
        this.localScale = localScale;
        
        children = new List<Entity>();
        position = localPosition;
        rad = localRad;
        scale = localScale;
        worldTransform = Matrix3x2.Identity;
        /*worldToLocalTransform = Matrix3x2.Identity;
        worldInverseTransform = Matrix3x2.Identity;*/
        SetDirty(EDirtyType.PositionDirty | EDirtyType.RotationDirty | EDirtyType.ScaleDirty);
    }
    
    /// <summary>
    /// sets the dirty flag on the enum and passes it down to our children
    /// </summary>
    
    /// <param name="dirtyFlagType">Dirty flag type.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetDirty(EDirtyType dirtyFlagType)
    {
        if ((hierarchyDirty & dirtyFlagType) == 0)
        {
            hierarchyDirty |= dirtyFlagType;

            switch (dirtyFlagType)
            {
                case EDirtyType.PositionDirty:
                    break;
                case EDirtyType.RotationDirty:
                    break;
                case EDirtyType.ScaleDirty:
                    break;
            }
            
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].IsAlive())
                {
                    children[i].Get<Transform>().SetDirty(dirtyFlagType);
                }
            }
        }
    }
    
    public int ChildrenCount => children.Count;
    public bool HasParent => parent != Entity.Null;
}

