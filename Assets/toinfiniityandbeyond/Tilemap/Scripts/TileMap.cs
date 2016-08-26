using System;
using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	[ExecuteInEditMode, AddComponentMenu ("Tilemapping/Tilemap"), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileMap-Component")]
	public class TileMap : MonoBehaviour
	{
		[SerializeField]
		private int width = 20, height = 20;
		[SerializeField]
		private ScriptableTile [] map = new ScriptableTile [0];

		private bool CurrentOperation = false;
		private List<ChangeElement> CurrentEdit;
		private Timeline timeline;

		public Action<int, int> OnTileChanged = (x, y) => { };
		public Action<int, int> OnTilemapRebuild = (width, height) => { };
		public int Width { get { return width; } }
		public int Height { get { return height; } }


		public void undo()
		{
			if (timeline == null)
				return;
			List<ChangeElement> changesToRevert = timeline.undo ();

			foreach (var c in changesToRevert) {
				map [c.x + c.y * width] = c.from;
			}

			UpdateTileMap ();
		}

		public void redo()
		{
			if (timeline == null)
				return;
			List<ChangeElement> changesToRevert = timeline.redo ();

			foreach (var c in changesToRevert) {
				map [c.x + c.y * width] = c.to;
			}

			UpdateTileMap ();
		}


		private void Update()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					ScriptableTile tile = GetTileAt(x, y);
					if (tile && tile.CheckIfCanTick ())
						UpdateTileAt (x, y);
				}
			}
		}

		private void Start()
		{
			timeline = new Timeline ();
		}

		private Point WorldPositionToPoint (Vector2 worldPosition, bool clamp = false)
		{
			Point offset = (Point)transform.position;
			Point point = (Point)worldPosition;

			int x = point.x - offset.x;
			int y = point.y - offset.y;

			if(clamp)
			{
				x = Mathf.Clamp (x, 0, width - 1);
				y = Mathf.Clamp (y, 0, height - 1);
			}
			return new Point (x, y);
		}

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
		public bool IsInBounds (Point point)
		{
			return IsInBounds(point.x, point.y);
		}
		public bool IsInBounds(int x, int y)
		{
			return (x >= 0 && x < width && y >= 0 && y < height);
		}

		public ScriptableTile GetTileAt (Vector2 worldPosition)
		{
			return GetTileAt (WorldPositionToPoint(worldPosition));
		}
		public ScriptableTile GetTileAt (Point point)
		{
			return GetTileAt (point.x, point.y);
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

		public bool SetTileAt (Vector2 worldPosition, ScriptableTile to)
		{
			return SetTileAt (WorldPositionToPoint (worldPosition), to);
		}
		public bool SetTileAt (Point point, ScriptableTile to)
		{
			return SetTileAt (point.x, point.y, to);
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

				currentEdit.Add (new ChangeElement (x, y, from, to));


				return true;
			}
			return false;
		}
		public void UpdateTileAt (Point point)
		{
			OnTileChanged.Invoke (point.x, point.y);
		}
		public void UpdateTileAt (int x, int y)
		{
			OnTileChanged.Invoke (x, y);
		}
		public void UpdateTileMap ()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					UpdateTileAt (x, y);
				}
			}
		}

		public void BeginOperation ()
		{
			CurrentOperation = true;
			CurrentEdit = new List<ChangeElement> ();
			Debug.Log ("Starting Operation");
		}

		public void FinishOperation ()
		{
			CurrentOperation = false;
			if (timeline == null)
				timeline = new Timeline ();
			timeline.pushChanges (currentEdit);
			Debug.Log ("Finishing Operation");
		}

		public bool OperationInProgress ()
		{
			return CurrentOperation;
		}

#if UNITY_EDITOR
		[SerializeField]
		private bool debugMode;

		public ScriptableTile primaryTile;
		public ScriptableTile secondaryTile;

		public bool IsInEditMode = false;
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