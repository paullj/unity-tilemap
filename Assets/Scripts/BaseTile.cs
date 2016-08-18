using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	public abstract class BaseTile : ScriptableObject
	{
		public string Name { get { return this.name; } }
		public int ID { get { return Animator.StringToHash (Name); } }

		public abstract bool IsValid { get; }

		public abstract Sprite GetSprite (TileMap tilemap, Coordinate position = default (Coordinate));
		public abstract Texture2D GetTexture (TileMap tilemap, Coordinate position = default (Coordinate));
		public abstract Color [] GetColors (TileMap tilemap, Coordinate position = default (Coordinate));
	}
}