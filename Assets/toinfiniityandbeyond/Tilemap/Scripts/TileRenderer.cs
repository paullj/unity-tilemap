using UnityEngine;
using System.Linq;

namespace toinfiniityandbeyond.Tilemapping
{
	[ExecuteInEditMode, AddComponentMenu ("Tilemapping/TilemapRenderer"), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileRenderer-Component")]
	public class TileRenderer : MonoBehaviour
	{
		public Color color;
		public Material material;
		public enum MeshType { Sprites, SingleQuad, MultipleQuads, ChunkedQuads }
		public MeshType meshType;

		[Space]
		public int sortingLayer;
		public int orderInLayer;

		[SerializeField]
		private TileMap tileMap;
		[SerializeField]
		private SpriteRenderer [] spriteMap;

		private void Reset ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			color = Color.white;
			material = new Material (Shader.Find ("Sprites/Default"));

			Resize (tileMap.Width, tileMap.Height);
			Rebuild ();
		}
		private void OnEnable ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			tileMap.OnTileChanged += UpdateTile;
			tileMap.OnTilemapRebuild += Resize;

			Rebuild ();
		}
		private void OnDisable ()
		{
			tileMap.OnTileChanged -= UpdateTile;
		}
		public void Resize(int width, int height)
		{
			if (spriteMap != null && spriteMap.Length > 0) {
				foreach (SpriteRenderer sprite in spriteMap) {
					if (!sprite)
						continue;
					DestroyImmediate (sprite.gameObject);
				}
			}
			spriteMap = new SpriteRenderer [tileMap.Width * tileMap.Height];
			/*SpriteRenderer [,] old = new SpriteRenderer [w, h];
			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					int index = x + y * w;
					old [x, y] = spriteMap [index];
				}
			}
			spriteMap = new SpriteRenderer [width * height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int index = x + y * width;
					if (x < w && y < h)
						spriteMap [index] = old [x, y];
				}
			}
			w = width;
			h = height;
			*/
		}
		public void Rebuild()
		{
			for (int x = 0; x < tileMap.Width; x++)
			{
				for (int y = 0; y < tileMap.Height; y++)
				{
					UpdateTile (x, y);
				}
			}
		}
		public void UpdateTile(int x, int y)
		{
			int index = x + y * tileMap.Width;
			SpriteRenderer current = spriteMap [index];
			if(current == null)
			{
				current = new GameObject (string.Format ("[{0}, {1}]", x, y), typeof(PolygonCollider2D)).AddComponent<SpriteRenderer>();
				current.transform.position = new Vector2 (x, y);
				current.transform.localScale = Vector2.one;
				current.transform.SetParent(transform);
				current.gameObject.hideFlags = HideFlags.HideInHierarchy;

				current.sharedMaterial = material;
				current.color = color;
				spriteMap [index] = current;
			}
			ScriptableTile tile = tileMap.GetTileAt (x, y);

			if(current.GetComponent<PolygonCollider2D> ())
				DestroyImmediate(current.GetComponent<PolygonCollider2D> ());

			current.sprite = tile ? tile.GetSprite (tileMap, new Point(x, y)) : null;

			if (current.sprite)
				current.gameObject.AddComponent<PolygonCollider2D> ();
		}

		private void Build (int width, int height, ScriptableTile [] map)
		{
			/*meshFilter = GetComponent<MeshFilter> () ? GetComponent<MeshFilter> () : gameObject.AddComponent<MeshFilter> ();
			meshRenderer = GetComponent<MeshRenderer> () ? GetComponent<MeshRenderer> () : gameObject.AddComponent<MeshRenderer> ();

			//meshFilter.hideFlags = HideFlags.HideInInspector;

			Mesh mesh = meshFilter.mesh = new Mesh ();

			Vector3 [] vertices = new Vector3 [0];
			Vector2 [] uv = new Vector2 [0];
			Vector4 [] tangents = new Vector4 [0];
			Vector4 tangent = new Vector4 (1f, 0f, 0f, -1f);
			int [] triangles = new int [0];

			if (meshType == MeshType.SingleQuad)
			{
				vertices = new Vector3 [4] {
					new Vector3(0, 0),
					new Vector3(width, 0),
					new Vector3(width, height),
					new Vector3(0, height)
				};
				uv = new Vector2 [4]
				{
					new Vector2 (0, 0),
					new Vector2 (1, 0),
					new Vector2 (1, 1),
					new Vector2 (0, 1)
				};
				tangents = new Vector4 [4]
				{
					tangent,
					tangent,
					tangent,
					tangent
				};

				triangles = new int [6]
				{
					3, 2, 1,
					3, 1, 0,
				};
			}
			else if (meshType == MeshType.MultipleQuads)
			{
				vertices = new Vector3 [(width + 1) * (height + 1)];
				uv = new Vector2 [vertices.Length];
				tangents = new Vector4 [vertices.Length];

				for (int i = 0, y = 0; y <= height; y++)
				{
					for (int x = 0; x <= width; x++, i++)
					{
						vertices [i] = new Vector3 (x, y);
						uv [i] = new Vector2 ((float)x / width, (float)y / height);
						tangents [i] = tangent;
					}
				}

				triangles = new int [width * height * 6];
				for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++)
				{
					for (int x = 0; x < width; x++, ti += 6, vi++)
					{
						triangles [ti] = vi;
						triangles [ti + 3] = triangles [ti + 2] = vi + 1;
						triangles [ti + 4] = triangles [ti + 1] = vi + width + 1;
						triangles [ti + 5] = vi + width + 2;
					}
				}
			}
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.tangents = tangents;
			mesh.triangles = triangles;

			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();

		//	meshRenderer.hideFlags = HideFlags.HideInInspector;

			meshRenderer.sortingLayerID = sortingLayer;
			meshRenderer.sortingOrder = orderInLayer;

			Texture2D mapTexture = new Texture2D (width * 16, height * 16);
			MaterialPropertyBlock mpb = new MaterialPropertyBlock ();

			mpb.SetColor ("_Color", color);
			mpb.SetTexture ("_MainTex", mapTexture);
			meshRenderer.SetPropertyBlock (mpb);

			meshRenderer.material = material;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					BaseTile currentTile = map [x + y * width];
					Color [] colours = (currentTile ? currentTile.GetColors (tileMap, new Point (x, y)) :
						Enumerable.Repeat (Color.clear, 16 * 16).ToArray ());

					mapTexture.SetPixels (x * 16, y * 16, 16, 16, colours);
				}
			}
			mapTexture.filterMode = FilterMode.Point;
			mapTexture.alphaIsTransparency = true;
			mapTexture.Apply ();
		*/
		}
	}
}