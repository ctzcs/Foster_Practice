using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Engine;
using Foster.Framework;

namespace Content.Frog;

public class FrogSample:ILifetime
{
    private App ctx;
    private const int MaxFrogs = 1_000_000;
    private const int AddRemoveAmount = 5_000;
    private const int DrawBatchSize = 32768;

    private readonly Batcher _batcher;
    private readonly SpriteFont font;
    private readonly FrameCounter frameCounter = new();
    private readonly Frog[] frogs = new Frog[MaxFrogs];
    private readonly Material material;
    private readonly Mesh<PosTexColVertex> mesh;
    private readonly Texture texture;
    private readonly PosTexColVertex[] vertexArray = new PosTexColVertex[DrawBatchSize * 4];
    private int frogCount;

    private Rng rng = new(1337);

    public FrogSample(App ctx)
    {
        this.ctx = ctx;
        _batcher = new Batcher(ctx.GraphicsDevice);
        texture = new Texture(ctx.GraphicsDevice, new Image(Path.Join("Assets", "frog_knight.png")));
        font = new SpriteFont(ctx.GraphicsDevice, Path.Join("Assets", "monogram.ttf"), 32);

        //构建网格
        mesh = new Mesh<PosTexColVertex>(ctx.GraphicsDevice);
        //构建材质
        material = new Material(new TexturedShader(ctx.GraphicsDevice));

        // We only need to initialize indices once, since we're only drawing quads
        var indexArray = new int[DrawBatchSize * 6];
        var vertexCount = 0;
        
        for (var i = 0; i < indexArray.Length; i += 6)
        {
            //顶点合并，但是索引不合并，索引+6，顶点数量+4
            indexArray[i + 0] = vertexCount + 0;
            indexArray[i + 1] = vertexCount + 1;
            indexArray[i + 2] = vertexCount + 2;
            
            indexArray[i + 3] = vertexCount + 0;
            indexArray[i + 4] = vertexCount + 2;
            indexArray[i + 5] = vertexCount + 3;
            vertexCount += 4;
        }

        mesh.SetIndices(indexArray);

        // Texture coordinates will not change, so we can initialize those
        for (var i = 0; i < DrawBatchSize * 4; i += 4)
        {
            vertexArray[i].Tex = new Vector2(0, 0);
            vertexArray[i + 1].Tex = new Vector2(1, 0);
            vertexArray[i + 2].Tex = new Vector2(1, 1);
            vertexArray[i + 3].Tex = new Vector2(0, 1);
        }
    }
    
    public void Start()
    {
        
    }

    public void Destroy()
    {
        
    }

    public void Update()
    {
        if (ctx.Input.Mouse.LeftDown)
        {
            for (var i = 0; i < AddRemoveAmount; i++)
                if (frogCount < MaxFrogs)
                {
                    frogs[frogCount].Position = ctx.Input.Mouse.Position;
                    frogs[frogCount].Speed.X = rng.Float(-250, 250) / 60.0f;
                    frogs[frogCount].Speed.Y = rng.Float(-250, 250) / 60.0f;
                    frogs[frogCount].Color = new Color(
                        rng.U8(50, 240),
                        rng.U8(80, 240),
                        rng.U8(100, 240),
                        255
                    );
                    frogCount++;
                }
        }
        // Remove frogs
        else if (ctx.Input.Mouse.RightDown)
        {
            frogCount = Math.Max(0, frogCount - AddRemoveAmount);
        }

        // Update frogs
        var halfSize = (Vector2)texture.Size / 2f;
        var screenSize = new Vector2(ctx.Window.WidthInPixels, ctx.Window.HeightInPixels);

        var range = 100;
        float acc = 10;
        float maxSpeed = 2;
        var mousePos = ctx.Input.Mouse.Position;
        for (var i = 0; i < frogCount; i++)
        {
            if (IsInCircle(mousePos, frogs[i].Position, range))
            {
                var accDir = frogs[i].Position - mousePos;
                frogs[i].Speed += accDir.Normalized() * acc;
            }

            if (Vector2.DistanceSquared(frogs[i].Speed, Vector2.Zero) > maxSpeed * maxSpeed)
                frogs[i].Speed = frogs[i].Speed.Normalized() * maxSpeed;

            frogs[i].Position += frogs[i].Speed;

            if (frogs[i].Position.X + halfSize.X > screenSize.X ||
                frogs[i].Position.X + halfSize.X < 0)
                frogs[i].Speed.X *= -1;

            if (frogs[i].Position.Y + halfSize.Y > screenSize.Y ||
                frogs[i].Position.Y + halfSize.Y - 40 < 0)
                frogs[i].Speed.Y *= -1;
        }
    }
    

    public void Render()
    {
        frameCounter.Update();
        ctx.Window.Clear(Color.White);
        _batcher.Text(font, $"{frogCount} Frogs : {frameCounter.FPS} FPS", new Vector2(8, -2), Color.Black);
        _batcher.Render(ctx.Window);
        _batcher.Clear();
        // Batching/batch size is important: too low = excessive draw calls, too high = slower gpu copies
        for (var i = 0; i < frogCount; i += DrawBatchSize)
        {
            var count = Math.Min(frogCount - i, DrawBatchSize);
            if (ctx.Input.Keyboard.Down(Keys.Space))
                RenderBatchCustom(i, count);
            else
                RenderBatchFoster(i, count);
        }
    }



    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsInCircle(Vector2 pos, Vector2 target, float radius)
    {
        return Vector2.DistanceSquared(pos, target) <= radius * radius;
    }
    


    /// <summary>
    ///     Plain Foster.
    ///     So simple, so fast.
    /// </summary>
    private void RenderBatchFoster(int from, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var frog = frogs[i + from];
            _batcher.Image(texture, frog.Position, frog.Color);
        }

        _batcher.Render(ctx.Window);
        _batcher.Clear();
    }

    /// <summary>
    ///     A tailor made solution for shoving frogs into a gpu.
    ///     Goes down a rabbit hole (ha) for a few extra frames:
    ///     - Smaller vertex format
    ///     - Simplified shader logic
    ///     - Initialize indices only on startup
    ///     - Initialize vertex texture coords on startup
    ///     - One time shader uniform set per frame
    ///     - A lot of inlining (same result could be achieved with AggressiveInlining)
    /// </summary>
    private void RenderBatchCustom(int from, int count)
    {
        //
        for (var i = 0; i < count; i++)
        {
            var frog = frogs[i + from];
            var v = i * 4;
            vertexArray[v].Col = frog.Color;
            vertexArray[v + 1].Col = frog.Color;
            vertexArray[v + 2].Col = frog.Color;
            vertexArray[v + 3].Col = frog.Color;
            vertexArray[v].Pos = frog.Position;
            vertexArray[v + 1].Pos = frog.Position + new Vector2(texture.Width, 0);
            vertexArray[v + 2].Pos = frog.Position + new Vector2(texture.Width, texture.Height);
            vertexArray[v + 3].Pos = frog.Position + new Vector2(0, texture.Height);
        }

        mesh.SetVertices(vertexArray.AsSpan(0, count * 4));

        if (from == 0)
        {
            var matrix = Matrix4x4.CreateOrthographicOffCenter(0, ctx.Window.WidthInPixels, ctx.Window.HeightInPixels,
                0, 0,
                float.MaxValue);
            material.Vertex.SetUniformBuffer(matrix);
            material.Fragment.Samplers[0] = new Material.BoundSampler(texture, new TextureSampler());
        }

        DrawCommand command = new(ctx.Window, mesh, material)
        {
            BlendMode = BlendMode.Premultiply,
            MeshIndexStart = 0,
            MeshIndexCount = count * 6
        };

        command.Submit(ctx.GraphicsDevice);
    }

    public struct Frog
    {
        public Vector2 Position;
        public Vector2 Speed;
        public Color Color;
    }
}