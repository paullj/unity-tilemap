using UnityEngine;
	
namespace toinfiniityandbeyond.Tilemapping
{
	//Remember to change these names to something more meaningful!
	[CreateAssetMenu (fileName = "AutoTile", menuName = "Tilemap/Tiles/AutoTile")]
	public class AutoTile : ScriptableTile
	{
		public Sprite [] bitmaskSprites = new Sprite[16];
		public enum AutoTileMode { Everything, SameTile, None}
		public AutoTileMode mode;
		public bool edgesAreFull = false;
		
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

			ScriptableTile[] tiles = new ScriptableTile[] {
				tilemap.GetTileAt (position.Up),
				tilemap.GetTileAt (position.Left),
				tilemap.GetTileAt (position.Down),
				tilemap.GetTileAt (position.Right),
			};
			int[] bitmasks = new int[] {
				1, 2, 4, 8
			};
			Point[] points = new Point[] {
				position.Up,
				position.Left,
				position.Down,
				position.Right,
			};

			int index = 0;
			for(int i = 0; i < 4; i++) {
				bool exists = tiles[i] != null;
				bool isSame = exists && tiles[i].ID == this.ID;
				bool isEdge = !tilemap.IsInBounds (points[i]);
				
				if((isEdge && edgesAreFull) || (exists && mode == AutoTileMode.Everything) || (exists && (mode == AutoTileMode.SameTile && isSame)))
					index += bitmasks[i];
			}

			if (index < bitmaskSprites.Length && bitmaskSprites [index])
				return bitmaskSprites [index];

			return bitmaskSprites [15];
		}
		public override Texture2D GetIcon ()
		{
			if (!IsValid) return null;
			return bitmaskSprites[15].ToTexture2D();
		}
	}
}
