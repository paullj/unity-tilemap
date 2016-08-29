using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Eraser : ScriptableTool
	{
		public int radius;

		public Eraser () : base ()
		{
			radius = 1;
		}
		public override KeyCode Shortcut { get { return KeyCode.E; } }
		public override string Description { get { return "Sets the painted tile to nothing"; } }

		public override void OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			if (tile == null && map == null)
				return;

			//If we haven't already started an operation, start one now
			//This is for undo/ redo support
			if (!map.OperationInProgress())
				map.BeginOperation ();

			int correctedRadius = radius - 1;

			for (int x = -correctedRadius; x <= correctedRadius; x++)
			{
				for (int y = -correctedRadius; y <= correctedRadius; y++)
				{
					Point offsetPoint = point + new Point (x, y);

					map.SetTileAt (offsetPoint, null);
				}
			}
		}

		public override void OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			OnClick(point, tile, map);
		}
	}
}
