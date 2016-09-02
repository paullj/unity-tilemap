using UnityEngine;
	
namespace toinfiniityandbeyond.Tilemapping
{
	//Remember to change these names to something more meaningful!
	[CreateAssetMenu (fileName = "New RandomTile", menuName = "Tilemap/Tiles/RandomTile")]
	public class RandomTile : ScriptableTile
	{
		[SerializeField]
		private int seed = 0;
		[SerializeField]
		private Sprite[] sprites;
		
		private Texture2D texture;
		
		//Returns if this tile is okay to be used in the tile map
		//For example: if this tile doesn't have a Read/Write enabled sprite it will return false
		public override bool IsValid
		{
			get
			{
				if(sprites == null || sprites.Length == 0)
					return false;
				
				try
				{
					sprites[0].texture.GetPixel(0, 0);
				}
				catch(UnityException e)
				{
					return false;
				}
				return true;
			}
		}
		
		public override Sprite GetSprite (TileMap tilemap, Point position = default (Point))
		{
			int positionSeed = position.x >= position.y ? position.x * position.x + position.x + position.y : position.x + position.y * position.y;
			System.Random prng = new System.Random (seed + positionSeed);
			int index = prng.Next (0, sprites.Length);
			return sprites[index];
		}
		public override Texture2D GetIcon (TileMap tilemap, Point position = default (Point))
		{
			if (texture == null)
				RebuildTexture ();
			return texture;
		}

		//Called when the inspector has been edited
		private void OnValidate ()
		{
			RebuildTexture ();
		}
		
		public void RebuildTexture ()
		{
			if (!IsValid)
				return;
		
			texture = new Texture2D ((int)sprites [0].rect.width, (int)sprites [0].rect.height, sprites [0].texture.format, false);

			texture.filterMode = sprites [0].texture.filterMode;
			texture.wrapMode = sprites [0].texture.wrapMode;
			texture.Apply ();
		}
	}
}
