using System;
using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	[ExecuteInEditMode, AddComponentMenu ("Tilemapping/Tilemap")]
	public class TileMap : MonoBehaviour
	{
		[SerializeField]
		private bool debugMode;

		public ScriptableTile primaryTile;
		public ScriptableTile secondaryTile;

		[SerializeField]
		private int width = 20, height = 20;
		[SerializeField]
		private ScriptableTile [] map = new ScriptableTile [0];

		public Action<int, int> OnTileChanged = (x, y) => { };
		public Action<int, int> OnTilemapRebuild = (width, height) => { };

		public int Width { get { return width; } }
		public int Height { get { return height; } }
		//public BaseTile [] Map { get { return map; } set { map = value; } }

		public void Resize ()
		{
			if (map.Length == width * height)
				return;

		/*	BaseTile [,] old = new BaseTile [width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int index = x + y * width;
					old [x, y] = map [index];
				}
			}
			*/
			map = new ScriptableTile [width * height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int index = x + y * width;
				//	if (x < width && y < height)
				//		map [index] = old [x, y];
				}
			}
			OnTilemapRebuild.Invoke (width, height);
		//	width = nWidth;
		//	height = nHeight;
		}

		private bool IsInBounds(int x, int y)
		{
			return (x >= 0 && x < width && y >= 0 && y < height);
		}
		private Coordinate WorldPointToCoordinate (Vector2 worldPoint, bool clamp = false)
		{
			Coordinate offset = (Coordinate)transform.position;
			Coordinate point = (Coordinate)worldPoint;

			int x = point.x - offset.x;
			int y = point.y - offset.y;

			if(clamp)
			{
				x = Mathf.Clamp (x, 0, width - 1);
				y = Mathf.Clamp (y, 0, height - 1);
			}
			return new Coordinate (x, y);
		}

		public ScriptableTile GetTileAt (Vector2 worldPoint)
		{
			return GetTileAt (WorldPointToCoordinate(worldPoint));
		}
		public ScriptableTile GetTileAt (Coordinate coordinate)
		{
			return GetTileAt (coordinate.x, coordinate.y);
		}
		public ScriptableTile GetTileAt (int x, int y)
		{
			if (!IsInBounds (x, y))
				return null;

			int index = x + y * width;

			if (index >= map.Length)
				Resize ();

			return map [x + y * width];
		}

		public bool SetTileAt (Vector2 worldPoint, ScriptableTile to)
		{
			return SetTileAt (WorldPointToCoordinate (worldPoint), to);
		}
		public bool SetTileAt (Coordinate coordinate, ScriptableTile to)
		{
			return SetTileAt (coordinate.x, coordinate.y, to);
		}
		public bool SetTileAt (int x, int y, ScriptableTile to)
		{
			ScriptableTile from = GetTileAt (x, y);
			//Conditions for returning
			if (IsInBounds (x, y) &&
				!(from == null && to == null) &&
				(((from == null || to == null) && (from != null || to != null)) ||
				from.ID != to.ID))
			{
				map [x + y * width] = to;

				OnTileChanged.Invoke (x, y);

				if (debugMode)
					Debug.LogFormat ("Set [{0}, {1}] from {2} to {3}", x, y, from ? from.Name : "nothing", to ? to.Name : "nothing");

				return true;
			}
			return false;
		}

#if UNITY_EDITOR
		public Rect toolbarWindowPosition;
		public Rect tilePickerWindowPosition;
		public Vector2 tilePickerScrollView;

		public int selectedScriptableTool = -1;
		public int lastSelectedScriptableTool = -1;

		public bool primaryTilePickerToggle = false;
		public bool secondaryTilePickerToggle = false;

		public List<ScriptableTool> scriptableToolCache = new List<ScriptableTool> ();
		public List<ScriptableTile> scriptableTileCache = new List<ScriptableTile> ();

		public Vector3 position;
		public Quaternion rotation;

#endif
	}
}