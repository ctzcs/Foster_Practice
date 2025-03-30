namespace Engine.Source.Transform;

using System;
using System.Collections.Generic;
using System.Numerics;

public class Transform
{
    public bool dirty = true;
    private Transform? parent;
    private List<Transform> children = new List<Transform>();

    /// WorldPos用来渲染
    private Vector3 _worldPosition = Vector3.Zero;

    /// <summary>
    /// Local用来表示相对父节点的偏移
    /// </summary>
    private Vector3 _localPosition;

    private Vector3 scale = Vector3.One;

    public float rotation;

    public static Vector3 Origin => Vector3.Zero;

    public Vector3 LocalPosition
    {
        get => _localPosition;
        set
        {
            _localPosition = value;
            dirty = true;
        }
    }

    public Vector3 Position
    {
        get
        {
            if (dirty)
            {
                //计算父坐标
                CalculateWorldPosition();
                dirty = false;
                return _worldPosition;
            }
            return _worldPosition;
            
        }
        set
        {
            _worldPosition = value;
            CalculateLocalPosition();
            dirty = true;
        }
    }

    /*public Vector3 PositionV2
    {
        get => _worldPosition + _localPosition;
        set
        {
            _worldPosition = value;
            dirty = true;
        }
    }*/

    public int Facing => MathF.Sign(scale.X);

    public Transform()
    {
    }

    public Transform(Vector3 initialPosition)
    {
        Position = initialPosition;
    }

    internal void UpdateWorldPosition(Vector3 newWorldPosition)
    {
        _worldPosition = newWorldPosition;
        dirty = true;
    }

    internal void CalculateWorldPosition()
    {
        if (parent != null)
        {
            _worldPosition = parent.Position + _localPosition;
        }
        else
        {
            _worldPosition = _localPosition;
        }
    }

    internal void CalculateLocalPosition()
    {
        if (parent != null)
        {
            _localPosition = _worldPosition - parent.Position;
        }
        else
        {
            _localPosition = _worldPosition;
        }
    }
}
