using System;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	/// <summary>
	/// The Brush tool inherits from the abstract class ScriptableTool.
	/// This means that it is automatically includeded in the tilemap editor
	/// </summary>
	public class Brush : ScriptableTool
	{
		//Public variables will automatically be exposed in the tile editor
		//This can be very useful, however it only works with some Types including:
		//
		//	- bool								- AnimationCurve
		//	- float								- Color
		//	- int								- Sprite, Texture2D
		//	- Vector2, Vector3					- GameObject, Object
		//	- Enum

		public int radius;
		public enum BrushShape { Square, Circle, }
		public BrushShape shape;

		/// <summary>
		/// The default constructor where you can set up default variable values
		/// </summary>
		public Brush () : base ()
		{
			radius = 1;
			shape = BrushShape.Square;
		}

		/// <summary>
		/// Optional override to set a shortcut used in the tile editor
		/// </summary>
		/// <value>The KeyCode, default is none. </value>
		public override KeyCode Shortcut { get { return KeyCode.B; } }

		/// <summary>
		/// Optional override to set a description for the tool
		/// </summary>
		/// <value>The description, default is nothing.</value>
		public override string Description { get { return "A simple brush"; } }

		/// <summary>
		/// Called by the tilemap editor to paint tiles
		/// </summary>
		/// <param name="point">Where you want to use the tool</param>
		/// <param name="tile">The ScriptableTile you want to use</param>
		/// <param name="map">What you want to use the tool on</param>
		public override void OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			if (tile == null || map == null)
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
			
						if (shape == BrushShape.Circle) {
							Vector2 delta = (Vector2)(offsetPoint - point);
							if (delta.sqrMagnitude > radius * radius)
								continue;
						}

					map.SetTileAt (offsetPoint, tile);
				}
			}
		}

		public override void OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			OnClick(point, tile, map);
		}
	}
}
