using UnityEngine;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
	
	[System.Serializable]
	public class Fill : ScriptableTool
	{
		//Public variables automatically get exposed in the tile editor
		
		public Fill () : base()
		{
			//You can set any defaults for variables here
		}
		
		public override KeyCode Shortcut
		{
			get { return KeyCode.F; }
		}
		
		public override string Description
		{
			get { return "A flood fill tool"; }
		}

		
		//Called when you left click on a tile map when in edit mode
		public override bool Use (Coordinate point, ScriptableTile tile, TileMap map)
		{
			ScriptableTile start = map.GetTileAt(point);
			if (start == null || tile == null)
				return true;

			Queue<Coordinate> open = new Queue<Coordinate> ();
			List<Coordinate> closed = new List<Coordinate> ();
		
			//If we've iterated through the whole map then we've done something wrong
			int maxLoops = map.Width * map.Height;

			open.Enqueue (point);
			while(open.Count > 0) 
			{
				maxLoops--;
				if (maxLoops <= 0) {
					Debug.LogError ("Fill tool, max loops reached!");
					return false;
				}

				Coordinate c = open.Dequeue ();
				ScriptableTile t = map.GetTileAt(c);
				if((t == null && start == null) || t.ID == start.ID)
				{
					closed.Add (c);

					ScriptableTile u = map.GetTileAt(c.Up);
					ScriptableTile r = map.GetTileAt(c.Right);
					ScriptableTile d = map.GetTileAt(c.Down);
					ScriptableTile l = map.GetTileAt(c.Left);

					if (u && u != tile && !closed.Contains(c.Up)) 
						open.Enqueue (c.Up);
					if (r && r != tile && !closed.Contains(c.Right)) 
						open.Enqueue (c.Right);
					if (d && d != tile && !closed.Contains(c.Down)) 
						open.Enqueue (c.Down);
					if (l && l != tile && !closed.Contains(c.Left)) 
						open.Enqueue (c.Left);

					map.SetTileAt (c, tile);
				}
			}

			return true;
		}
	}
}
