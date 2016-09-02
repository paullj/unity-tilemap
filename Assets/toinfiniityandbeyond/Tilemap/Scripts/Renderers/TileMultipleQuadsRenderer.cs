using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
    [AddComponentMenu("2D/Renderer/TileMultipleQuadsRenderer")]
    public class TileMultipleQuadsRenderer : TileRenderer
    {
        public enum TextureAtlasMode { Normal, Dynamic, SameFile }
        [SerializeField]
        List<Sprite> spriteList = new List<Sprite>();
        [SerializeField]
        Rect[] rects = new Rect[0];
        [SerializeField]
        private Texture2D atlas;
        [SerializeField]
        private MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public Texture2D Atlas {
            get { return atlas; }
        }

        public override void OnEnable() {
            base.OnEnable();
        } 
        private void SetUpMesh()
        {
            if (meshRenderer == null || meshFilter == null)
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
        public void RefreshAtlas() {
             spriteList = new List<Sprite>();
             tileMap.UpdateTileMap();
        } 
        private bool AddSpriteToAtlas(Sprite sprite)
        {
            if (spriteList.Contains(sprite))
                return false;

            spriteList.Add(sprite);
            atlas = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            rects = atlas.PackTextures(spriteList.Select(x => x.ToTexture2D()).ToArray(), 0);
            atlas.filterMode = FilterMode.Point;
            atlas.wrapMode = TextureWrapMode.Clamp;
            atlas.Apply();
            return true;
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
                    // 2 -- 3
                    // |  / |
                    // 0 -- 1
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
            meshRenderer.sharedMaterial.SetTexture("_MainTex", atlas);
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
            if (tile == null)
            {
                uv[quadIndex] = uv[quadIndex + 1] = uv[quadIndex + 2] = uv[quadIndex + 3] = Vector2.zero;

                meshFilter.sharedMesh.uv = uv;
                return;
            }

            Vector2[] prevUVs = new Vector2[] {
                uv[quadIndex],
                uv[quadIndex + 1],
                uv[quadIndex + 2],
                uv[quadIndex + 3]
            };

            Sprite sprite = tile.GetSprite(tileMap, new Point(x, y));
            bool atlasUpdated = AddSpriteToAtlas(sprite);
         
            // assign four uv coordinates to change the texture of one tile (one quad, two triangels)
            Rect spriteRect = rects[spriteList.IndexOf(sprite)];
            Vector2[] newUVs = new Vector2[] {
                new Vector2(spriteRect.xMin, spriteRect.yMin),
                new Vector2(spriteRect.xMax, spriteRect.yMin),
                new Vector2(spriteRect.xMin, spriteRect.yMax),
                new Vector2(spriteRect.xMax, spriteRect.yMax)
            };
   
            if(!newUVs.SequenceEqual(prevUVs))
            {
                uv[quadIndex] = newUVs[0];
                uv[quadIndex + 1] = newUVs[1];
                uv[quadIndex + 2] = newUVs[2];
                uv[quadIndex + 3] = newUVs[3];
                meshFilter.sharedMesh.uv = uv;
                
                if(atlasUpdated)
                    //FIXME so that it only updates tiles who have changed uvs
                   tileMap.UpdateType(tile);
            }
        }
    }
}
