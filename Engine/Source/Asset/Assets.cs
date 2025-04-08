using System.Numerics;
using Foster.Framework;

namespace Engine.Source.Asset;

public static class Assets
{
	public static SpriteFont? Font { get; private set; }
	public static Texture? Atlas { get; private set; }
	public static readonly Dictionary<string, Sprite> Sprites = new();
	public static readonly Dictionary<string, Subtexture> Subtextures = new();
	
	private const string assetsFolderName = "Assets";
	private static string? path = null;

	public static string AssetsPath
	{
		get
		{
			// during development we search up from the build directory to find the Assets folder
			// (instead of copying all the Assets to the build directory).
			if (path == null)
			{
				var up = "";
				while (!Directory.Exists(Path.Join(up, assetsFolderName)) && up.Length < 10)
					up = Path.Join(up, "..");
				path = Path.Join(up, assetsFolderName);
			}

			return path ?? throw new Exception("Unable to find Assets path");
		}
	}

	/// <summary>
	/// 打包资源
	/// </summary>
	/// <param name="gfx"></param>
	public static void Load(GraphicsDevice gfx)
	{
		var spritesPath = Path.Join(AssetsPath, "Sprites");
		var spriteFiles = new Dictionary<string, Aseprite>();

		// 加载主要字体文件
		Font = new SpriteFont(gfx, Path.Join(AssetsPath, "Fonts", "monogram.ttf"), 32);
		Font.LineGap = 4;

		// 获取所有的ase结尾的sprites文件
		foreach (var file in Directory.EnumerateFiles(spritesPath, "*.ase", SearchOption.AllDirectories))
		{
			var name = Path.ChangeExtension(Path.GetRelativePath(spritesPath, file), null);
			Log.Info(name);
			try
			{
				var ase = new Aseprite(file);
			
				if (ase.Frames.Length > 0)
					spriteFiles.Add(name, ase);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			
		}
		
		// 打包所有的sprites
		Packer.Output output;
		{
			var packer = new Packer();

			foreach (var (name, ase) in spriteFiles)
			{
				var frames = ase.RenderAllFrames();
				for (int i = 0; i < frames.Length; i ++)
					packer.Add($"{name}/{i}", frames[i]);
			}

			/*foreach (var (name, ase) in tilesetFiles)
			{
				var image = ase.RenderFrame(0);
				var columns = image.Width / Game.TileSize;
				var rows = image.Height / Game.TileSize;

				for (int x = 0; x < columns; x ++)
					for (int y = 0; y < rows; y ++)
						packer.Add($"tilesets/{name}{x}x{y}", image, new RectInt(x, y, 1, 1) * Game.TileSize);
			}*/

			output = packer.Pack();
		}

		// create texture file
		Atlas = new Texture(gfx, output.Pages[0], name: "Atlas");

		// create subtextures
		foreach (var it in output.Entries)
			Subtextures.Add(it.Name, new Subtexture(Atlas, it.Source, it.Frame));

		// create sprite assets
		foreach (var (name, ase) in spriteFiles)
		{
			// find origin
			Vector2 origin = Vector2.Zero;
			if (ase.Slices.Count > 0 && ase.Slices[0].Keys.Length > 0 && ase.Slices[0].Keys[0].Pivot.HasValue)
				origin = ase.Slices[0].Keys[0].Pivot!.Value;

			var sprite = new Sprite(name, origin);

			// add frames
			for (int i = 0; i < ase.Frames.Length; i ++)
				sprite.Frames.Add(new(GetSubtexture($"{name}/{i}"), ase.Frames[i].Duration / 1000.0f));

			// add animations
			foreach (var tag in ase.Tags)
			{
				if (!string.IsNullOrEmpty(tag.Name))
					sprite.AddAnimation(tag.Name, tag.From, tag.To - tag.From + 1);
			}

			Sprites.Add(name, sprite);
		}
	}

	/// <summary>
	/// 卸载资源
	/// </summary>
	public static void Unload()
	{
		Atlas?.Dispose();
		Atlas = null;
		Font = null;

		Sprites.Clear();
		Subtextures.Clear();
		
	}

	/// <summary>
	/// 获取Sprite
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Sprite? GetSprite(string name)
	{
		if (Sprites.TryGetValue(name, out var value))
			return value;
		return null;
	}
	

	/// <summary>
	/// 获取子纹理
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Subtexture GetSubtexture(string name)
	{
		if (Subtextures.TryGetValue(name, out var value))
			return value;
		return new();
	}

}