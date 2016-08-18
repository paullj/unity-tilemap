using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

// For obtaining list of sorting layers.
using UnityEditorInternal;
using System.Reflection;

namespace toinfiniityandbeyond.Tilemapping
{
	partial class TileMapEditor : Editor
	{
		private TransformData transformData;
		private Rect toolbarWindowPosition;
		private Rect tilePickerWindowPosition;
		private Vector2 tilePickerScrollView;

		private int selectedStencil = 0;
		private bool primaryTilePickerToggle = false;
		private bool secondaryTilePickerToggle = false;

		private List<Stencil> stencilCache = new List<Stencil> ();
		private List<BaseTile> tileCache = new List<BaseTile> ();

		partial void OnSceneEnable ()
		{
			RefreshStencilCache ();
			RefreshTileCache ();
		}
		partial void OnSceneDisable ()
		{

		}

		private void OnSceneGUI ()
		{
			if (IsInEditMode)
			{
				

				toolbarWindowPosition = ClampToScreen(GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), toolbarWindowPosition, ToolbarWindow, new GUIContent ("Toolbar"), GUILayout.Width(80)));

				if (primaryTilePickerToggle)
				{
					if (secondaryTilePickerToggle)
						primaryTilePickerToggle = false;

					tilePickerWindowPosition = ClampToScreen(GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), tilePickerWindowPosition, (int id) => { TilepickerWindow (id, ref tileMap.primaryTile); }, new GUIContent ("Pick primary tile..."), GUILayout.Width (100)));
				} else if (secondaryTilePickerToggle)
				{
					if (primaryTilePickerToggle)
						secondaryTilePickerToggle = false;

					tilePickerWindowPosition = ClampToScreen (GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), tilePickerWindowPosition, (int id) => { TilepickerWindow (id, ref tileMap.secondaryTile); }, new GUIContent ("Pick secondary tile..."), GUILayout.Width (100)));
				}
				if(!secondaryTilePickerToggle && !primaryTilePickerToggle)
				{
					Vector2 cursorPos = GUIUtility.ScreenToGUIPoint (Event.current.mousePosition);
					tilePickerWindowPosition = new Rect (cursorPos + new Vector2 (20, 0), Vector2.zero);
				}
				
				DrawGrid ();

				HandleShortcutEvents ();
				HandleMouseEvents ();

				tileMap.transform.position = transformData.position;
				tileMap.transform.rotation = transformData.rotation;
				tileMap.transform.localScale = Vector3.one;

				HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
				SceneView.RepaintAll ();
			}
			DrawOutline ();
		}

		private void ToolbarWindow (int id)
		{
			var boldCentredStyle = EditorStyles.whiteBoldLabel;
			boldCentredStyle.alignment = TextAnchor.MiddleCenter;
			boldCentredStyle.wordWrap = true;

			var smallCenteredStyle = EditorStyles.whiteMiniLabel;
			smallCenteredStyle.alignment = TextAnchor.MiddleCenter;
			smallCenteredStyle.wordWrap = true;
			string text = "None";

			Rect secondaryTileRect = new Rect (toolbarWindowPosition.width - 5 - toolbarWindowPosition.width * 0.4f, 25 + toolbarWindowPosition.width * 0.4f, toolbarWindowPosition.width * 0.4f, toolbarWindowPosition.width * 0.4f);
			secondaryTilePickerToggle = GUI.Toggle (secondaryTileRect, secondaryTilePickerToggle, GUIContent.none, "Button");

			GUI.contentColor = Color.black;
			if (tileMap.secondaryTile)
			{
				GUI.DrawTexture (secondaryTileRect, tileMap.secondaryTile.GetTexture (tileMap));
				GUI.contentColor = Color.white;
				text = tileMap.secondaryTile.Name;
			}
			GUI.Label (secondaryTileRect, text, smallCenteredStyle);

			Rect primaryTileRect = new Rect (5, 25, toolbarWindowPosition.width * 0.6f, toolbarWindowPosition.width * 0.6f);
			primaryTilePickerToggle = GUI.Toggle (primaryTileRect, primaryTilePickerToggle, GUIContent.none, "Button");

			text = "None";

			GUI.contentColor = Color.black;
			if (tileMap.primaryTile)
			{
				GUI.DrawTexture (primaryTileRect, tileMap.primaryTile.GetTexture (tileMap));
				GUI.contentColor = Color.white;
				text = tileMap.primaryTile.Name;
			}
			GUI.Label (primaryTileRect, text, boldCentredStyle);
			GUI.contentColor = Color.white;

			GUILayout.Space (toolbarWindowPosition.width);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Swap [X]"))
			{
				SwapCurrentTiles ();
			}
			GUI.color = new Color (1, 0.5f, 0.5f);
			if (GUILayout.Button ("Clear"))
			{
				tileMap.primaryTile = null;
				tileMap.secondaryTile = null;
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal ();

			CustomGUI.Splitter();
			GUIStyle title = EditorStyles.boldLabel;
			title.alignment = TextAnchor.MiddleLeft;
			GUILayout.Label ("Stencils", title);
			GUILayout.Space (10);

			for (int i = 0; i < stencilCache.Count; i++)
			{
				bool selected = (i == selectedStencil);
				EditorGUI.BeginChangeCheck ();
				string labelName = stencilCache [i].ShortcutKeyCode != KeyCode.None ?
							string.Format("{1} [{0}]", stencilCache [i].ShortcutKeyCode.ToString(), stencilCache [i].Name) :
							stencilCache [i].Name;

				GUIContent content = new GUIContent (labelName, stencilCache [i].Description);
				GUILayout.Toggle (selected, content, EditorStyles.radioButton, GUILayout.Width (100));
				if (EditorGUI.EndChangeCheck())
				{
					selectedStencil = i;

					if (selected)
						selectedStencil = -1;
				}
			}
			if (selectedStencil == -1)
			{
				EditorGUILayout.HelpBox ("No stencil selected, select one from above.", MessageType.Warning, true);
			}
			if(selectedStencil >= 0 && selectedStencil < stencilCache.Count)
			{
				const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				FieldInfo [] fields = stencilCache [selectedStencil].GetType ().GetFields (flags);

				if (fields.Length > 0)
				{
					GUILayout.Label ("Settings", EditorStyles.boldLabel, GUILayout.Width (100));
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo fieldInfo = fields [i];
						DrawFieldInfo (fieldInfo);
					}
				} else
				{
					EditorGUILayout.HelpBox ("Stencil has no public variables to edit.", MessageType.Info, true);
				}
			}

			
			GUI.DragWindow ();
		}
		private void TilepickerWindow (int id, ref BaseTile tileToChange)
		{
			GUI.BringWindowToFront (id);

			if (tileCache.Count > 0)
			{
				int columns = 4;
				int rows = Mathf.CeilToInt (1 + (tileCache.Count) / columns);
				float tileWidth = (tilePickerWindowPosition.width - 50) / columns;

				tilePickerScrollView = GUILayout.BeginScrollView (tilePickerScrollView, false, true, GUILayout.Height(tileWidth * 4));

				GUILayout.BeginVertical ();
				for (int y = 0; y < rows; y++)
				{
					GUILayout.BeginHorizontal ();
					for (int x = 0; x < columns; x++)
					{
						int index = x + y * columns - 1;


						if (index < 0)
						{
							var style = EditorStyles.boldLabel;
							style.alignment = TextAnchor.MiddleCenter;
							style.wordWrap = true;

							if (GUILayout.Button (GUIContent.none, GUILayout.Height (tileWidth), GUILayout.Width (tileWidth)))
							{
								tileToChange = null;
								primaryTilePickerToggle = secondaryTilePickerToggle = false;
							}
							GUI.Label (GUILayoutUtility.GetLastRect(), "None", style);
						}

						if (index >= tileCache.Count ||index < 0)
							continue;

						GUI.color = tileCache [index].IsValid ? Color.white : new Color(1, 0.5f, 0.5f);

						if (GUILayout.Button (GUIContent.none, GUILayout.Height (tileWidth), GUILayout.Width (tileWidth)))
						{
							if (tileCache [index].IsValid)
							{
								tileToChange = tileCache [index];
								primaryTilePickerToggle = secondaryTilePickerToggle = false;
							}
							else
							{
								Type pt = Assembly.GetAssembly (typeof (Editor)).GetType ("UnityEditor.ProjectBrowser");
								EditorWindow.GetWindow (pt).Show ();
								EditorGUIUtility.PingObject (tileCache [index]);
							}
						}

					
						Rect buttonRect = GUILayoutUtility.GetLastRect ();
						string tileText = "...";
						if (tileCache [index].IsValid)
						{
							GUI.DrawTexture (buttonRect, tileCache [index].GetTexture (tileMap));
							tileText = tileCache [index].Name;
						}
						GUI.color = Color.white;

						var centeredStyle = EditorStyles.whiteBoldLabel;
						centeredStyle.alignment = TextAnchor.MiddleCenter;
						centeredStyle.wordWrap = true;

						GUI.Label (buttonRect, tileText, centeredStyle);
					}
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndVertical ();
				GUILayout.EndScrollView ();
			}
			else
			{
				EditorGUILayout.HelpBox ("No Tiles found. Try force refresh or \nIn the project window, 'Create > Tilemap > Tiles > ...' to create one", MessageType.Warning);
			}
			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Force Refresh"))
			{
				RefreshTileCache ();
			}
			GUI.color = new Color (1f, 0.5f, 0.5f);
			if (GUILayout.Button ("Close Tile Picker"))
			{
				primaryTilePickerToggle = secondaryTilePickerToggle = false;
			}
			GUI.color = Color.white;

			GUILayout.EndHorizontal ();

			GUI.DragWindow ();
		}
		private void SwapCurrentTiles()
		{
			BaseTile temp = tileMap.primaryTile;
			tileMap.primaryTile = tileMap.secondaryTile;
			tileMap.secondaryTile = temp;
		}
		private void DrawFieldInfo (FieldInfo field)
		{
			Type type = field.FieldType;
			Stencil stencil = stencilCache [selectedStencil];
			string fieldName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase (field.Name);

			GUILayout.BeginHorizontal (GUILayout.Width (100));
			GUILayout.Label (fieldName, EditorStyles.miniLabel);
			if (type == typeof (bool))
			{
				bool v = (bool)field.GetValue (stencil);
				bool nv = EditorGUILayout.Toggle (v);
				field.SetValue (stencil, nv);
			}
			else if (type == typeof (float))
			{
				float v = (float)field.GetValue (stencil);
				float nv = EditorGUILayout.FloatField (v);
				field.SetValue (stencil, nv);
			}
			else if (type == typeof (int))
			{
				int v = (int)field.GetValue (stencil);
				int nv = EditorGUILayout.IntField (v);
				field.SetValue (stencil, nv);
			}
			else if (type == typeof (Enum))
			{
				int v = (int)field.GetValue (stencil);
				int nv = EditorGUILayout.IntField (v);
				field.SetValue (stencil, nv);
			}
			else if (type == typeof (Vector2))
			{
				Vector2 v = (Vector2)field.GetValue (stencil);
				Vector2 nv = Vector2.zero;
				nv.x = EditorGUILayout.FloatField (v.x);
				nv.y = EditorGUILayout.FloatField (v.y);
				field.SetValue (stencil, nv);
			}
			else if (type == typeof (Vector3))
			{
				Vector3 v = (Vector3)field.GetValue (stencil);
				Vector3 nv = Vector3.zero;
				nv.x = EditorGUILayout.FloatField (v.x);
				nv.y = EditorGUILayout.FloatField (v.y);
				nv.z = EditorGUILayout.FloatField (v.z);
				field.SetValue (stencil, nv);
			}
			else if(type.IsEnum)
			{
				int v = (int)field.GetValue (stencil);
				int nv = EditorGUILayout.Popup (v, Enum.GetNames(type));
				field.SetValue (stencil, nv);
			}
			else
			{
				Debug.LogErrorFormat (this, "The public variable type '{0}' is currently not supported by stencils \n Feel free to add support for it though!", type.Name);
			}
			GUILayout.EndHorizontal ();
		}

		private void RefreshStencilCache()
		{
			foreach (Type type in Assembly.GetAssembly (typeof (Stencil)).GetTypes ()
					 .Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf (typeof (Stencil))))
			{
				bool containsType = false;
				for (int i = 0; i < stencilCache.Count; i++)
				{
					if (stencilCache[i].GetType () == type)
					{
						containsType = true;
						break;
					}
				}
				if(!containsType)
					stencilCache.Add ((Stencil)Activator.CreateInstance (type));
			}
		}
		private void RefreshTileCache ()
		{
			string [] guids = AssetDatabase.FindAssets (string.Format ("t:{0}", typeof (BaseTile)));
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath (guids [i]);
				BaseTile asset = AssetDatabase.LoadAssetAtPath<BaseTile> (assetPath);
				if (asset != null && !tileCache.Contains(asset))
				{
					tileCache.Add (asset);
				}
			}
		}

		private void DrawGrid ()
		{
			float width = tileMap.Width;
			float height = tileMap.Height;

			Vector3 position = transformData.position;

			Handles.color = Color.gray;
			for (float i = 1; i < width; i++)
			{
				Handles.DrawLine (new Vector3 (i + position.x, position.y), new Vector3 (i + position.x, height + position.y));
			}
			for (float i = 1; i < height; i++)
			{
				Handles.DrawLine (new Vector3 (position.x, i + position.y), new Vector3 (width + position.x, i + position.y));
			}
		}
		private void DrawOutline()
		{
			float width = tileMap.Width;
			float height = tileMap.Height;

			Vector3 position = transformData.position;

			Handles.color = Color.white;
			Handles.DrawLine (position, new Vector3 (width + position.x, position.y));
			Handles.DrawLine (position, new Vector3 (position.x, height + position.y));
			Handles.DrawLine (new Vector3 (width + position.x, position.y), new Vector3 (width + position.x, height + position.y));
			Handles.DrawLine (new Vector3 (position.x, height + position.y), new Vector3 (width + position.x, height + position.y));
		}
		private void HandleShortcutEvents()
		{
			for (int i = 0; i < stencilCache.Count; i++)
			{
				if (Event.current.isKey && Event.current.keyCode == stencilCache [i].ShortcutKeyCode)
				{
					selectedStencil = i;
				}
			}
			if (Event.current.type == EventType.KeyDown && Event.current.isKey && Event.current.keyCode == KeyCode.X)
			{
				SwapCurrentTiles ();
			}
		}
		private void HandleMouseEvents ()
		{
			Event e = Event.current;
			Coordinate point = new Coordinate (0, 0);

			if (GetMousePosition (ref point))
			{
				if ( (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0)
				{
					if (selectedStencil >= 0 && selectedStencil < stencilCache.Count)
					{
						if (stencilCache [selectedStencil].Paint (point, tileMap.primaryTile, tileMap))
						{
							Undo.RecordObject (tileMap, "Paint Tilemap");
						}
					}
				}
			}
		}
		private bool GetMousePosition (ref Coordinate point)
		{
			if (SceneView.currentDrawingSceneView == null)
				return false;

			Plane plane = new Plane (tileMap.transform.TransformDirection (Vector3.forward), transformData.position);
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			Vector3 position = transformData.position;

			float distance;
			if (plane.Raycast (ray, out distance))
				point = (Coordinate)(ray.origin + (ray.direction.normalized * distance)- position);

			bool result = (point.x >= 0 && point.x < position.x + tileMap.Width && point.y >= 0 && point.y < tileMap.Height);
			Handles.color = result ? new Color (0.5f, 1, 0.5f) : new Color (1, 0.5f, 0.5f);
			Handles.DrawWireCube ((Vector2)position + (Vector2)point + Vector2.one * 0.5f, Vector2.one);
			
			return result;
		}

		private Rect ClampToScreen (Rect r)
		{
			r.x = Mathf.Clamp (r.x, 5, Screen.width - r.width - 5);
			r.y = Mathf.Clamp (r.y, 25, Screen.height - r.height - 25);
			return r;
		}
		private struct TransformData
		{
			public Vector3 position;
			public Quaternion rotation;
			//public Vector3 scale;

			public TransformData (Vector3 p, Quaternion r)// Vector3 s)
			{
				position = p;
				rotation = r;
				//scale = s;
			}
			public TransformData (Transform t)
			{
				position = t.position;
				rotation = t.rotation;
			//	scale = t.localScale;
			}
		}
	}
}