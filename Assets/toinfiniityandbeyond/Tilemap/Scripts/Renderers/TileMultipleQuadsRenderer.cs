using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    [AddComponentMenu("2D/Renderer/TileMultipleQuadsRenderer")]
    public class TileMultipleQuadsRenderer : TileRenderer
    {
        [SerializeField]
        private Texture2D textureAtlas;
        [SerializeField]
        private MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        private void SetUpMesh()
        {
            if(meshRenderer == null || meshFilter == null)
            {
                ClearChildren();
                meshFilter = new GameObject("_MESH").AddComponent<MeshFilter>();
                meshFilter.transform.SetParent(parent);
                meshFilter.transform.localPosition = Vector2.zero;
                meshFilter.transform.localScale = Vector2.one;

                meshRenderer = meshFilter.gameObject.AddComponent<MeshRenderer>();
                meshRenderer.transform.SetParent(parent);
                meshRenderer.transform.localPosition = Vector2.zero;
                meshRenderer.transform.localScale = Vector2.one;
            }
        }

        public override void Resize(int width, int height)
        {
            var quads = width * height; // one quad per tile

            var vertices = new Vector3[quads * 4];
            var triangles = new int[quads * 6];
            var normals = new Vector3[vertices.Length];
            var uv = new Vector2[vertices.Length];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var i = (y * width) + x; // quad index

                    var qi = i * 4; // vertex index
                    var ti = i * 6;

                    // vertices going clockwise
                    // 2--3
                    // | /|
                    // |/ |
                    // 0--1
                    vertices[qi] = new Vector3(x, y, 0);
                    vertices[qi + 1] = new Vector3(x + 1, y, 0);
                    vertices[qi + 2] = new Vector3(x, y + 1, 0);
                    vertices[qi + 3] = new Vector3(x + 1, y + 1, 0);

                    triangles[ti] = qi;
                    triangles[ti + 1] = qi + 2;
                    triangles[ti + 2] = qi + 3;

                    triangles[ti + 3] = qi;
                    triangles[ti + 4] = qi + 3;
                    triangles[ti + 5] = qi + 1;
                }
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                normals[i] = Vector3.forward;
                uv[i] = Vector2.zero; // uv are set by assigning a tile
            }

            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                normals = normals,
                uv = uv,
                name = "mesh"
            };
            SetUpMesh();
            meshFilter.sharedMesh = mesh;
            UpdateTileMap();
        }

        public override void UpdateTileMap()
        {
			base.UpdateTileMap();
            meshRenderer.sharedMaterial = material;
            meshRenderer.sharedMaterial.SetTexture("_MainTex", textureAtlas);
            meshRenderer.sharedMaterial.color = color;
            meshRenderer.sortingLayerID = sortingLayer;
            meshRenderer.sortingOrder = orderInLayer;
        }
        public override void UpdateTileAt(int x, int y)
        {
            //var currentTexture = textureAtlas;
            ScriptableTile tile = tileMap.GetTileAt(x, y);

            int quadIndex = x + y * tileMap.Width;

            quadIndex *= 4;
            var uv = meshFilter.sharedMesh.uv;

            if (tile == null) {
                uv[quadIndex] = uv[quadIndex + 1] = uv[quadIndex + 2] = uv[quadIndex + 3] = Vector2.zero;
          
                meshFilter.sharedMesh.uv = uv;
                return;
            }

            Sprite sprite = tile.GetSprite(tileMap, new Point(x, y));
            textureAtlas = sprite.texture;
            var r = sprite.textureRect;
            
            // if (currentTexture != null && currentTexture != tile.GetSprite(tileMap, new Point(x, y)))
            //      throw new ArgumentException("Sprites from different textures is not supported in QuadGrid mode.");
            

            // assign four uv coordinates to change the texture of one tile (one quad, two triangels)
            uv[quadIndex] = RectToUV(new Vector2(r.xMin, r.yMin), sprite.texture);
            uv[quadIndex + 1] = RectToUV(new Vector2(r.xMax, r.yMin), sprite.texture);
            uv[quadIndex + 2] = RectToUV(new Vector2(r.xMin, r.yMax), sprite.texture);
            uv[quadIndex + 3] = RectToUV(new Vector2(r.xMax, r.yMax), sprite.texture);
          
            meshFilter.sharedMesh.uv = uv;
        }
        private Vector2 RectToUV(Vector2 xy, Texture2D texture)
        {
            return new Vector2(xy.x / texture.width, xy.y / texture.height);
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
