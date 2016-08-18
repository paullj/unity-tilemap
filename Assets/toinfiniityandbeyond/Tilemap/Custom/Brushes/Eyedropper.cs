using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Eyedropper : ScriptableBrush
	{
		public Eyedropper () : base ()
		{

		}
		public override KeyCode ShortcutKeyCode { get { return KeyCode.I; } }
		public override string Description { get { return "Sets the primary tile to whatever you click"; } }

		public override bool Paint (Coordinate point, ScriptableTile tile, TileMap map)
		{
			map.primaryTile = map.GetTileAt (point);
			return true;
		}
	}
}
