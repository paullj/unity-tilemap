using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Eraser : Stencil
	{
		public int radius;

		public Eraser () : base ()
		{
			radius = 1;
		}
		public override KeyCode ShortcutKeyCode { get { return KeyCode.E; } }
		public override string Description { get { return "Sets the painted tile to nothing"; } }

		public override bool Paint (Coordinate point, BaseTile tile, TileMap map)
		{
			if (map == null)
				return false;

			bool result = false;
			for (int x = -radius; x <= radius; x++)
			{
				for (int y = -radius; y <= radius; y++)
				{
					Coordinate offsetPoint = point + new Coordinate (x, y);
					if (map.SetTileAt (offsetPoint, null))
					{
						result = true;
					}
				}
			}
			return result;
		}
	}
}
