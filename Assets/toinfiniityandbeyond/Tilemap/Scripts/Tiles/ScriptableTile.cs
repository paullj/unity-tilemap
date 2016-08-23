using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	public abstract class ScriptableTile : ScriptableObject
	{
		public string Name { get { return this.name; } }
		public int ID { get { return GetInstanceID(); } }
		public abstract bool IsValid { get; }

		public abstract Sprite GetSprite (TileMap tilemap = null, Point position = default (Point));
		public abstract Color [] GetColors (TileMap tilemap = null, Point position = default (Point));
		public abstract Texture2D GetTexture (TileMap tilemap = null, Point position = default (Point));
		
		//Handle Ticking
		private float timeOffset = 0;
		public virtual float TickRate { get { return 0; } }
		
		//Yeah, this is a bad name :/ rename asap
		public bool CheckIfCanTick ()
		{
			if(timeOffset - Time.time > TickRate)
				timeOffset = 0;
				
			if(Time.realtimeSinceStartup >= timeOffset)
			{
				timeOffset = Time.time + TickRate;
				return Tick ();
			}
			return false;
		}
		protected virtual bool Tick()
		{
			return false;
		}
	}
}