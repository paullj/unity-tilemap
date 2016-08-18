using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Eraser : Brush
	{
		public Eraser () : base ()
		{
		}
		public override KeyCode ShortcutKeyCode { get { return KeyCode.E; } }
		public override string Description { get { return "Sets the painted tile to nothing"; } }

		public override bool Use (Coordinate point, ScriptableTile tile, TileMap map)
		{
			return base.Use (point, null, map);
		}
	}
}
