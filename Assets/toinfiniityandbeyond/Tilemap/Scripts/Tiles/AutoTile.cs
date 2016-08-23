using UnityEngine;
	
namespace toinfiniityandbeyond.Tilemapping
{
	//Remember to change these names to something more meaningful!
	[CreateAssetMenu (fileName = "AutoTile", menuName = "Tilemap/Tiles/AutoTile")]
	public class AutoTile : ScriptableTile
	{
		public Sprite [] bitmaskSprites = new Sprite[16];
		public bool defaultIsFull = true;

		private Texture2D texture;
		private Color [] colors;

		//Called when the inspector has been edited
		private void OnValidate ()
		{
			RebuildTexture ();
		}
		
		//Returns if this tile is okay to be used in the tile map
		//For example: if this tile doesn't have a Read/Write enabled sprite it will return false
		public override bool IsValid
		{
			get
			{
				if(bitmaskSprites[15] == null)
					return false;
				
				try
				{
					bitmaskSprites [15].texture.GetPixel(0, 0);
				}
				catch(UnityException e)
				{
					return false;
				}
				return true;
			}
		}
		public bool IsElementValid(int index)
		{
			if (bitmaskSprites == null || bitmaskSprites [index] == null)
				return false;

			try
			{
				bitmaskSprites [index].texture.GetPixel (0, 0);
			}
			catch (UnityException e)
			{
				return false;
			}
			return true;
		}


		public override Sprite GetSprite (TileMap tilemap = null, Point position = default (Point))
		{
			if (tilemap == null)
				return bitmaskSprites[15];

			ScriptableTile left = tilemap.GetTileAt (position.Left);
			ScriptableTile up = tilemap.GetTileAt (position.Up);
			ScriptableTile right = tilemap.GetTileAt (position.Right);
			ScriptableTile down = tilemap.GetTileAt (position.Down);

			int index = 0;

			if ((!tilemap.IsInBounds (position.Up) && defaultIsFull) || (up && up.ID == this.ID))
			{
				index += 1;
			}
			if ((!tilemap.IsInBounds (position.Right) && defaultIsFull) || (right && right.ID == this.ID))
			{
				index += 8;
			}
			if ((!tilemap.IsInBounds (position.Down) && defaultIsFull) || (down && down.ID == this.ID))
			{
				index += 4;
			}
			if ((!tilemap.IsInBounds (position.Left) && defaultIsFull) || (left && left.ID == this.ID))
			{
				index += 2;
			}

			if (index < bitmaskSprites.Length && bitmaskSprites [index])
				return bitmaskSprites [index];

			return bitmaskSprites [15];
		}
		public override Texture2D GetTexture (TileMap tilemap = null, Point position = default (Point))
		{
			if (texture == null)
				RebuildTexture ();
			return texture;
		}
		public override Color [] GetColors (TileMap tilemap = null, Point position = default (Point))
		{
			if (colors.Length == 0)
				RebuildTexture ();
			return colors;
		}
		
		public void RebuildTexture ()
		{
			if (!IsValid)
				return;
			Sprite sprite = bitmaskSprites [15];
			texture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
			colors = sprite.texture.GetPixels ((int)sprite.textureRect.x,
												(int)sprite.textureRect.y,
												(int)sprite.textureRect.width,
												(int)sprite.textureRect.height);
			texture.SetPixels (colors);
			texture.filterMode = sprite.texture.filterMode;
			texture.wrapMode = sprite.texture.wrapMode;
			texture.Apply ();
		}
	}
}
