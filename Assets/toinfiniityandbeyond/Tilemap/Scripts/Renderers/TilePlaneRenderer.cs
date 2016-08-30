using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    [ExecuteInEditMode, AddComponentMenu("Tilemapping/Renderers/Plane"), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileRenderer-Component")]
    public class TilePlaneRenderer : TileRenderer
    {
		public override void Resize(int width, int height)
		{
			
		}
		public override void UpdateTileMap()
		{
		
		}
		public override void UpdateTileAt(int x, int y)
		{
			
		}
			//	private void Build (int width, int height, ScriptableTile [] map)
	//	{
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
//		}
    }
}
