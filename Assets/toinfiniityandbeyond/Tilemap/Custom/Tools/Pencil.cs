using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	[Serializable]
	public class Pencil : ScriptableTool
	{
		//An empty constructor
		public Pencil () : base ()
		{

		}
		//Sets the shortcut key to 'P'
		public override KeyCode Shortcut
		{
			get { return KeyCode.P; }
		}
		//Sets the tooltip description
		public override string Description
		{
			get { return "The simplest brush"; }
		}
		//Called when the left mouse button is held down
		public override bool OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			//Return if the tilemap is null/empty
			if (map == null)
				return false;
			//Set the tile at the specified point to the specified tile
			return map.SetTileAt (point, tile);
		}
		//Called when the left mouse button is initially held down
		public override bool OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			return OnClick(point, tile, map);
		}
		//Called when the left mouse button is finally let go
		public override bool OnClickUp (Point point, ScriptableTile tile, TileMap map)
		{
			map.UpdateTileMap ();
			return false;
		}
	}
}
