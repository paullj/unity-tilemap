using UnityEngine;
using System.Collections.Generic;
	
namespace toinfiniityandbeyond.Tilemapping
{
	//Remember to change these names to something more meaningful!
	[CreateAssetMenu (fileName = "New RandomTile", menuName = "Tilemap/Tiles/RandomTile")]
	public class RandomTile : ScriptableTile
	{
		public int globalSeed = 0;
		public List<Sprite> sprites;

		//Returns if this tile is okay to be used in the tile map
		//For example: if this tile doesn't have a Read/Write enabled sprite it will return false
		public override bool IsValid
		{
			get
			{
				if(sprites == null || sprites.Count == 0)
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
			//Get a unique seed based on position
			int positionSeed = position.x >= position.y ? position.x * position.x + position.x + position.y : position.x + position.y * position.y;
			//Use it alongside the global seed
			System.Random prng = new System.Random (globalSeed + positionSeed);
			//Get a pseudo random index based on position
			int index = prng.Next (0, sprites.Count);
			return sprites[index];
		}
		public override Texture2D GetIcon ()
		{
			if (!IsValid) return null;
			return sprites[0].ToTexture2D();
		}
	}
}
