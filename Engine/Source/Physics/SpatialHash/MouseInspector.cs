
/*
using System.Collections.Generic;
using System.Numerics;
using Engine.Core.Structure;
using Foster.Framework;
using UnityEngine;

namespace Test.CollisionDetector
{
    public class MouseInspector
    {
        private List<IEntityObj> _entityObjs;
        public Vector2Int chooseIndex;
        public int range;
        private Camera _camera;
        private Vector3[] _randomPositions;
        private void Awake()
        {
            _entityObjs = new(2048);
        }

        private void FixedUpdate()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            var worldPos = _camera.ScreenToWorldPoint(mousePos);
            
            if (_entityObjs == null )
            {
                return;
            }
            if ( _entityObjs.Count > 0)
            {
                foreach (var obj in _entityObjs)
                {
                    if (obj is EntityObj e)
                    {
                        var color = Color.black;
                        color.a = 0.5f;
                        e.SetColor(color);
                        
                    }
                }
            }
            _entityObjs.Clear();
            
            for (int i = 0; i < 200; i++)
            {
                Vector3 position = Random.insideUnitCircle * 30;
                position += worldPos;
                CollectPos(position);
            }
            
            PaintPos();

            void CollectPos(Vector3 pos)
            {
                var chooseIndex = CollisionSystem.I.VectorToIndex(pos);
            
                CollisionSystem.I.GEntityObjs(chooseIndex, range,_entityObjs);
                
            }

            void PaintPos()
            {
                foreach (var obj in _entityObjs)
                {
                    if (obj is EntityObj e)
                    {
                        /*var color = Color.black;
                        color.a = 0.5f;
                        e.SetColor(color);#1#
                        e.ResetColor();
                    }
                }
            }
            
            
        }
    }
}*/