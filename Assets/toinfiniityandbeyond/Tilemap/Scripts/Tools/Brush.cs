using UnityEngine;
using System;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	// The Brush tool inherits from the abstract class ScriptableTool.
	// This means that it is automatically included in the tilemap editor
	public class Brush : ScriptableTool
	{
		public int radius;
		public enum BrushShape { Square, Circle, }
		public BrushShape shape;

		// The default constructor where you can set up default variable values
		public Brush () : base ()
		{
			radius = 1;
			shape = BrushShape.Square;
		}

		// Optional override to set a shortcut used in the tile editor
		public override KeyCode Shortcut { get { return KeyCode.B; } }

		// Optional override to set a description for the tool
		public override string Description { get { return "A simple brush"; } }

		// Called by the tilemap editor to paint tiles
		public override void OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			if (map == null)
				return;

			//If we haven't already started an operation, start one now
			//This is for undo/ redo support
			if (!map.OperationInProgress())
				map.BeginOperation ();

			for(int i = 0; i < region.Count; i ++) {
				Point offsetPoint = region[i];
				
				map.SetTileAt (offsetPoint, tile);
			}
		}

		public override void OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			OnClick(point, tile, map);
		}
		public override List<Point> GetToolRegion (Point point, ScriptableTile tile, TileMap map) 
		{
			region = new List<Point>();
			int correctedRadius = radius - 1;
			for (int x = -correctedRadius; x <= correctedRadius; x++)
			{
				for (int y = -correctedRadius; y <= correctedRadius; y++)
				{
					Point offsetPoint = point + new Point (x, y);
					if (shape == BrushShape.Circle) {
						Vector2 delta = (Vector2)(offsetPoint - point);
						if (delta.sqrMagnitude > correctedRadius * correctedRadius)
							continue;
					}
					region.Add(offsetPoint);
				}
			}
			return region;
		}
	}
}
