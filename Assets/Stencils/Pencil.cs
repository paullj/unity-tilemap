using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Pencil : Stencil
	{
		public Pencil () : base ()
		{

		}
		public override KeyCode ShortcutKeyCode { get { return KeyCode.P; } }
		public override string Description { get { return "The simplest stencil"; } }

		public override bool Paint (Coordinate point, BaseTile tile, TileMap map)
		{
			if (tile == null && map == null)
				return false;

			return map.SetTileAt (point, tile);
		}
	}

}
