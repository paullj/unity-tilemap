using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Eyedropper : ScriptableTool
	{
		public Eyedropper () : base ()
		{

		}
		public override KeyCode Shortcut { get { return KeyCode.I; } }
		public override string Description { get { return "Sets the primary tile to whatever you click"; } }

		public override bool OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			map.primaryTile = map.GetTileAt (point);
			return true;
		}

		public override bool OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			return false;
		}

		public override bool OnClickUp (Point point, ScriptableTile tile, TileMap map)
		{
			return false;
		}
	}
}
