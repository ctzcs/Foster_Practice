using System.Numerics;

namespace Engine.Physics.SpatialHash
{
    public class EntityObj:IEntityObj
    {
        public static Vector2 MinPos = new Vector2(-999, -999);
        public Vector2 PrePosition { get; private set; }
        public Vector2 Position { get; private set; }

        /*private SpriteRenderer _sr;
        private Color _defaultColor = Color.white;
        private Color _transparent;

        private static Color[] _colors = new[]
        {
            new Color(0.6226415f,0.236917f,0.236917f,1),
            new Color(0.2456417f,0.62f,0.23f,1),
            new Color(0.76f,0.76f,0.12f,1),
            new Color(0.2f,0.76f,0.7f,1),
            new Color(0.3f,0.3f,0.8f,1),
        };*/

        /*public void Awake()
        {
            PrePosition = MinPos;
            _sr = GetComponent<SpriteRenderer>();
            _defaultColor = _colors[Random.Range(0, 4)];
            _transparent = Color.black;
            _transparent.a = 0.5f;
            SetColor(_transparent);
        }*/

        public void SetPosition(Vector2 newPos)
        {
            PrePosition = Position;
            Position = newPos;
             //  ??? transform.position = newPos;
            OnPositionChange();
        }

        public void SetPositionWithOutTransform(Vector2 newPos)
        {
            PrePosition = Position;
            Position = newPos;
            OnPositionChange();
        }

        /*public void SetColor(Color color)
        {
            //urp里面用反而被打断了
            /*_sr.GetPropertyBlock(_propBlock);
            _propBlock.SetColor(Color1, color);
            _sr.SetPropertyBlock(_propBlock);#1#
            _sr.color = color;
        }

        public void ResetColor()
        {
            _sr.color = _defaultColor;
            
        }*/

        /*private void FixedUpdate()
        {
            int  a = Random.Range(0, 2);
            if (a == 1)
            {
                var pos = Random.insideUnitCircle * RandomSpawner.rad;
                SetPosition(pos);
            }
            
        }*/

        void OnPositionChange()
        {
            CollisionSystem.I.MoveTo(this);
        }
    }
    
}