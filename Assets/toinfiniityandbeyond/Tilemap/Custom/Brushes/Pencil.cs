using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Pencil : ScriptableTool
	{
		public Pencil () : base ()
		{

		}
		public override KeyCode ShortcutKeyCode { get { return KeyCode.P; } }
		public override string Description { get { return "The simplest brush"; } }

		public override bool Use (Coordinate point, ScriptableTile tile, TileMap map)
		{
			if (tile == null && map == null)
				return false;

			return map.SetTileAt (point, tile);
		}
	}
}
