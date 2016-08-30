using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    [ExecuteInEditMode, AddComponentMenu("Tilemapping/Renderers/Sprite"), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileRenderer-Component")]
    public class TileSpriteRenderer : TileRenderer
    {
        [SerializeField]
        private SpriteRenderer[] spriteMap;

        public override void Resize(int width, int height)
        {
            if (!Application.isPlaying)
            {
                while (transform.childCount > 0)
                {
                    Object.DestroyImmediate(transform.GetChild(0).gameObject);
                }
			}
			else
			{
                foreach (Transform child in transform)
                {
                    Object.Destroy(child.gameObject);
                }
            }
            spriteMap = new SpriteRenderer[width * height];
        }
        public override void UpdateTileMap()
        {
            for (int x = 0; x < tileMap.Width; x++)
            {
                for (int y = 0; y < tileMap.Height; y++)
                {
                    UpdateTileAt(x, y);
                }
            }
        }
        public override void UpdateTileAt(int x, int y)
        {
            int index = x + y * tileMap.Width;
            SpriteRenderer current = spriteMap[index];
            if (current == null)
            {
                current = new GameObject(string.Format("[{0}, {1}]", x, y)).AddComponent<SpriteRenderer>();
                current.transform.SetParent(transform);
                current.transform.localPosition = new Vector2(x, y);
                current.transform.localScale = Vector2.one;
                current.gameObject.hideFlags = HideFlags.HideInHierarchy;

                spriteMap[index] = current;
            }
            ScriptableTile tile = tileMap.GetTileAt(x, y);
            current.sharedMaterial = material;
            current.color = color;
            current.sortingLayerID = sortingLayer;
            current.sortingOrder = orderInLayer;

            current.gameObject.SetActive(tile != null);

            current.sprite = tile ? tile.GetSprite(tileMap, new Point(x, y)) : null;
        }
    }
}