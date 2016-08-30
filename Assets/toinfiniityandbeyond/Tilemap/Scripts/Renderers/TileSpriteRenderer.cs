using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    [AddComponentMenu("2D/Renderer/TileSpriteRenderer")]
    public class TileSpriteRenderer : TileRenderer
    {
        [SerializeField]
        private SpriteRenderer[] spriteMap = new SpriteRenderer[0];

        public override void Resize(int width, int height)
        {
			if(width * height == spriteMap.Length)
				return;
            
            ClearChildren();

            spriteMap = new SpriteRenderer[width * height];
        }

        public override void UpdateTileAt(int x, int y)
        {
            int index = x + y * tileMap.Width;
            SpriteRenderer current = spriteMap[index];
            if (current == null)
            {
                current = new GameObject(string.Format("[{0}, {1}]", x, y)).AddComponent<SpriteRenderer>();
                current.transform.SetParent(parent);
                current.transform.localPosition = new Vector2(x, y);
                current.transform.localScale = Vector2.one;

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