using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

// For obtaining list of sorting layers.
using UnityEditorInternal;
using System.Reflection;

using toinfiniityandbeyond.Utility;

namespace toinfiniityandbeyond.Tilemapping
{
	partial class TileMapEditor : Editor
	{
		partial void OnSceneEnable ()
		{
			RefreshScriptableToolCache ();
			RefreshScriptableTileCache ();
		}
		partial void OnSceneDisable ()
		{

		}

		private void OnSceneGUI ()
		{
			if (IsInEditMode)
			{
				tileMap.toolbarWindowPosition = ClampToScreen (GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), tileMap.toolbarWindowPosition, ToolbarWindow, new GUIContent ("Toolbar"), GUILayout.Width (80)));

				if (tileMap.primaryTilePickerToggle)
				{
					if (tileMap.secondaryTilePickerToggle)
						tileMap.primaryTilePickerToggle = false;

					tileMap.tilePickerWindowPosition = ClampToScreen (GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), tileMap.tilePickerWindowPosition, (int id) => { TilepickerWindow (id, ref tileMap.primaryTile); }, new GUIContent ("Pick primary tile..."), GUILayout.Width (100)));
				}
				else if (tileMap.secondaryTilePickerToggle)
				{
					if (tileMap.primaryTilePickerToggle)
						tileMap.secondaryTilePickerToggle = false;

					tileMap.tilePickerWindowPosition = ClampToScreen (GUILayout.Window (GUIUtility.GetControlID (FocusType.Passive), tileMap.tilePickerWindowPosition, (int id) => { TilepickerWindow (id, ref tileMap.secondaryTile); }, new GUIContent ("Pick secondary tile..."), GUILayout.Width (100)));
				}
				if (!tileMap.secondaryTilePickerToggle && !tileMap.primaryTilePickerToggle)
				{
					Vector2 cursorPos = GUIUtility.ScreenToGUIPoint (Event.current.mousePosition);
					tileMap.tilePickerWindowPosition = new Rect (cursorPos + new Vector2 (20, 0), Vector2.zero);
				}

				DrawGrid ();

				HandleShortcutEvents ();
				HandleMouseEvents ();

				tileMap.transform.position = tileMap.position;
				tileMap.transform.rotation = tileMap.rotation;
				tileMap.transform.localScale = Vector3.one;

				HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
				SceneView.RepaintAll ();
			} else
			{
				tileMap.position = tileMap.transform.position;
				tileMap.rotation = tileMap.transform.rotation;
			}
			DrawOutline ();
		}

		private void ToolbarWindow (int id)
		{
			string text = "None";
			Rect secondaryTileRect = new Rect (tileMap.toolbarWindowPosition.width - 5 - tileMap.toolbarWindowPosition.width * 0.4f, 25 + tileMap.toolbarWindowPosition.width * 0.4f, tileMap.toolbarWindowPosition.width * 0.4f, tileMap.toolbarWindowPosition.width * 0.4f);
			tileMap.secondaryTilePickerToggle = GUI.Toggle (secondaryTileRect, tileMap.secondaryTilePickerToggle, GUIContent.none, "Button");

			GUI.contentColor = Color.black;
			if (tileMap.secondaryTile)
			{
				GUI.DrawTexture (secondaryTileRect, tileMap.secondaryTile.GetTexture (tileMap));
				GUI.contentColor = Color.white;
				text = tileMap.secondaryTile.Name;
			}
			GUI.Label (secondaryTileRect, text, CustomStyles.centerWhiteMiniLabel);

			Rect primaryTileRect = new Rect (5, 25, tileMap.toolbarWindowPosition.width * 0.6f, tileMap.toolbarWindowPosition.width * 0.6f);
			tileMap.primaryTilePickerToggle = GUI.Toggle (primaryTileRect, tileMap.primaryTilePickerToggle, GUIContent.none, "Button");

			text = "None";

			GUI.contentColor = Color.black;
			if (tileMap.primaryTile)
			{
				GUI.DrawTexture (primaryTileRect, tileMap.primaryTile.GetTexture (tileMap));
				GUI.contentColor = Color.white;
				text = tileMap.primaryTile.Name;
			}
			GUI.Label (primaryTileRect, text, CustomStyles.centerWhiteBoldLabel);
			GUI.contentColor = Color.white;

			float tileHeight = 10 + primaryTileRect.height + 0.5f * secondaryTileRect.height;

			GUILayout.Space (tileHeight);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Swap [X]"))
			{
				Swap<ScriptableTile> (ref tileMap.primaryTile, ref tileMap.secondaryTile);
			}
			GUI.color = new Color (1, 0.5f, 0.5f);
			if (GUILayout.Button ("Clear"))
			{
				tileMap.primaryTile = null;
				tileMap.secondaryTile = null;
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal ();

			CustomGUILayout.Splitter();

			GUILayout.Label ("Tools", CustomStyles.leftBoldLabel);
			EditorGUILayout.HelpBox ("[RMB] to toggle last tool", MessageType.Info, true);

			for (int i = 0; i < tileMap.scriptableToolCache.Count; i++)
			{
				bool selected = (i == tileMap.selectedScriptableTool);
				EditorGUI.BeginChangeCheck ();
				string labelName = tileMap.scriptableToolCache [i].Shortcut != KeyCode.None ?
							string.Format("{1} [{0}]", tileMap.scriptableToolCache [i].Shortcut.ToString(), tileMap.scriptableToolCache [i].Name) :
							tileMap.scriptableToolCache [i].Name;

				GUIContent content = new GUIContent (labelName, tileMap.scriptableToolCache [i].Description);
				GUILayout.Toggle (selected, content, EditorStyles.radioButton, GUILayout.Width (100));
				if (EditorGUI.EndChangeCheck())
				{
					tileMap.lastSelectedScriptableTool = tileMap.selectedScriptableTool;

					tileMap.selectedScriptableTool = i;

					if (selected)
						tileMap.selectedScriptableTool = -1;
				}
			}
			if (tileMap.selectedScriptableTool == -1)
			{
				EditorGUILayout.HelpBox ("No Tool selected, select one from above.", MessageType.Warning, true);
			}
			if(tileMap.selectedScriptableTool >= 0 && tileMap.selectedScriptableTool < tileMap.scriptableToolCache.Count)
			{
				const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				FieldInfo [] fields = tileMap.scriptableToolCache [tileMap.selectedScriptableTool].GetType ().GetFields (flags);

				if (fields.Length > 0)
				{
					GUILayout.Label ("Settings", CustomStyles.leftBoldLabel, GUILayout.Width (100));
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo field = fields [i];
						Type type = field.FieldType;
						string fieldName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase (field.Name);

						GUILayout.BeginHorizontal (GUILayout.Width (100));
						GUILayout.Label (fieldName, EditorStyles.miniLabel);
						if (type == typeof (bool))
						{
							bool v = (bool)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							bool nv = EditorGUILayout.Toggle (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (float))
						{
							float v = (float)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							float nv = EditorGUILayout.FloatField (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (int))
						{
							int v = (int)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							int nv = EditorGUILayout.IntField (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Enum))
						{
							int v = (int)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							int nv = EditorGUILayout.IntField (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Vector2))
						{
							Vector2 v = (Vector2)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							Vector2 nv = Vector2.zero;
							nv.x = EditorGUILayout.FloatField (v.x);
							nv.y = EditorGUILayout.FloatField (v.y);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Vector3))
						{
							Vector3 v = (Vector3)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							Vector3 nv = Vector3.zero;
							nv.x = EditorGUILayout.FloatField (v.x);
							nv.y = EditorGUILayout.FloatField (v.y);
							nv.z = EditorGUILayout.FloatField (v.z);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Color))
						{
							Color v = (Color)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							Color nv = EditorGUILayout.ColorField (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (AnimationCurve))
						{
							AnimationCurve v = (AnimationCurve)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							AnimationCurve nv = EditorGUILayout.CurveField (v);
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (GameObject))
						{
							GameObject v = (GameObject)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							GameObject nv = EditorGUILayout.ObjectField (v, typeof(GameObject), false) as GameObject;
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Texture2D))
						{
							Texture2D v = (Texture2D)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							Texture2D nv = EditorGUILayout.ObjectField (v, typeof (Texture2D), false) as Texture2D;
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (Sprite))
						{
							Sprite v = (Sprite)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							Sprite nv = EditorGUILayout.ObjectField (v, typeof (Sprite), false) as Sprite;
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type == typeof (UnityEngine.Object))
						{
							UnityEngine.Object v = (UnityEngine.Object)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							UnityEngine.Object nv = EditorGUILayout.ObjectField (v, typeof (UnityEngine.Object), false) as UnityEngine.Object;
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else if (type.IsEnum)
						{
							int v = (int)field.GetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool]);
							int nv = EditorGUILayout.Popup (v, Enum.GetNames (type));
							field.SetValue (tileMap.scriptableToolCache [tileMap.selectedScriptableTool], nv);
						}
						else
						{
							Debug.LogErrorFormat ("Exposing public variable type '{0}' is currently not supported by Tooles \n Feel free to add support for it though!", type.Name);
						}
						GUILayout.EndHorizontal ();

					}
				}
				else
				{
					EditorGUILayout.HelpBox ("Tool has no public variables to edit.", MessageType.Info, true);
				}
			}

			
			GUI.DragWindow ();
		}
		private void TilepickerWindow (int id, ref ScriptableTile tileToChange)
		{
			GUI.BringWindowToFront (id);

			if (tileMap.scriptableTileCache.Count > 0)
			{
				int columns = 4;
				int rows = Mathf.CeilToInt (1 + (tileMap.scriptableTileCache.Count) / columns);
				float tileWidth = (tileMap.tilePickerWindowPosition.width - 50) / columns;

				tileMap.tilePickerScrollView = GUILayout.BeginScrollView (tileMap.tilePickerScrollView, false, true, GUILayout.Height(tileWidth * 4));

				GUILayout.BeginVertical ();
				for (int y = 0; y < rows; y++)
				{
					GUILayout.BeginHorizontal ();
					for (int x = 0; x < columns; x++)
					{
						int index = x + y * columns - 1;


						if (index < 0)
						{
							if (GUILayout.Button (GUIContent.none, GUILayout.Height (tileWidth), GUILayout.Width (tileWidth)))
							{
								tileToChange = null;
								tileMap.primaryTilePickerToggle = tileMap.secondaryTilePickerToggle = false;
							}
							GUI.Label (GUILayoutUtility.GetLastRect(), "None", CustomStyles.centerBoldLabel);
						}

						if (index >= tileMap.scriptableTileCache.Count ||index < 0)
							continue;

						GUI.color = tileMap.scriptableTileCache [index].IsValid ? Color.white : new Color(1, 0.5f, 0.5f);

						if (GUILayout.Button (GUIContent.none, GUILayout.Height (tileWidth), GUILayout.Width (tileWidth)))
						{
							if (tileMap.scriptableTileCache [index].IsValid)
							{
								tileToChange = tileMap.scriptableTileCache [index];
								tileMap.primaryTilePickerToggle = tileMap.secondaryTilePickerToggle = false;
							}
							else
							{
								Type pt = Assembly.GetAssembly (typeof (Editor)).GetType ("UnityEditor.ProjectBrowser");
								EditorWindow.GetWindow (pt).Show ();
								EditorGUIUtility.PingObject (tileMap.scriptableTileCache [index]);
							}
						}

					
						Rect buttonRect = GUILayoutUtility.GetLastRect ();
						string tileText = "...";
						if (tileMap.scriptableTileCache [index].IsValid)
						{
							GUI.DrawTexture (buttonRect, tileMap.scriptableTileCache [index].GetTexture (tileMap));
							tileText = tileMap.scriptableTileCache [index].Name;
						}
						GUI.color = Color.white;

						GUI.Label (buttonRect, tileText, CustomStyles.centerWhiteBoldLabel);
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
				RefreshScriptableTileCache ();
			}
			GUI.color = new Color (1f, 0.5f, 0.5f);
			if (GUILayout.Button ("Close Tile Picker"))
			{
				tileMap.primaryTilePickerToggle = tileMap.secondaryTilePickerToggle = false;
			}
			GUI.color = Color.white;

			GUILayout.EndHorizontal ();

			GUI.DragWindow ();
		}
		private void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		private void RefreshScriptableToolCache()
		{
			List<ScriptableTool> toRemove = new List<ScriptableTool> ();
			for (int i = 0; i < tileMap.scriptableToolCache.Count; i++) {
				if (tileMap.scriptableToolCache [i] == null)
					toRemove.Add (tileMap.scriptableToolCache [i]);
			}
			tileMap.scriptableToolCache = tileMap.scriptableToolCache.Except (toRemove).ToList ();
			foreach (Type type in Assembly.GetAssembly (typeof (ScriptableTool)).GetTypes ()
					 .Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf (typeof (ScriptableTool))))
			{
				bool containsType = false;
				for (int i = 0; i < tileMap.scriptableToolCache.Count; i++)
				{
					if (tileMap.scriptableToolCache [i].GetType () == type)
					{
						containsType = true;
						break;
					}
				}
				if(!containsType)
					tileMap.scriptableToolCache.Add ((ScriptableTool)Activator.CreateInstance (type));
			}
		}
		private void RefreshScriptableTileCache ()
		{
			List<ScriptableTile> toRemove = new List<ScriptableTile> ();
			for (int i = 0; i < tileMap.scriptableTileCache.Count; i++) {
				if (tileMap.scriptableTileCache [i] == null)
					toRemove.Add (tileMap.scriptableTileCache [i]);
			}
			tileMap.scriptableTileCache = tileMap.scriptableTileCache.Except (toRemove).ToList ();
			string [] guids = AssetDatabase.FindAssets (string.Format ("t:{0}", typeof (ScriptableTile)));
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath (guids [i]);
				ScriptableTile asset = AssetDatabase.LoadAssetAtPath<ScriptableTile> (assetPath);
				if (asset != null && !tileMap.scriptableTileCache.Contains(asset))
				{
					tileMap.scriptableTileCache.Add (asset);
				}
			}
		}

		private void DrawGrid ()
		{
			float width = tileMap.Width;
			float height = tileMap.Height;

			Vector3 position = tileMap.position;

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

			Vector3 position = tileMap.position;

			Handles.color = Color.white;
			Handles.DrawLine (position, new Vector3 (width + position.x, position.y));
			Handles.DrawLine (position, new Vector3 (position.x, height + position.y));
			Handles.DrawLine (new Vector3 (width + position.x, position.y), new Vector3 (width + position.x, height + position.y));
			Handles.DrawLine (new Vector3 (position.x, height + position.y), new Vector3 (width + position.x, height + position.y));
		}
		private void HandleShortcutEvents()
		{
			for (int i = 0; i < tileMap.scriptableToolCache.Count; i++)
			{
				if (Event.current.isKey && Event.current.keyCode == tileMap.scriptableToolCache [i].Shortcut)
				{
					tileMap.lastSelectedScriptableTool = tileMap.selectedScriptableTool;
					tileMap.selectedScriptableTool = i;
				}
			}

			if (Event.current.type == EventType.KeyDown && Event.current.isKey && Event.current.keyCode == KeyCode.X)
			{
				Swap <ScriptableTile>(ref tileMap.primaryTile, ref tileMap.secondaryTile);
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
					if (tileMap.selectedScriptableTool >= 0 && tileMap.selectedScriptableTool < tileMap.scriptableToolCache.Count)
					{
						if (tileMap.scriptableToolCache [tileMap.selectedScriptableTool].Use (point, tileMap.primaryTile, tileMap))
						{
							Undo.RecordObject (tileMap, "Paint Tilemap");
						}
					}
				}
			}
			if ((e.type == EventType.MouseDown) && e.button == 1)
			{
				Swap<int> (ref tileMap.selectedScriptableTool, ref tileMap.lastSelectedScriptableTool);
			}

		}
		private bool GetMousePosition (ref Coordinate point)
		{
			if (SceneView.currentDrawingSceneView == null)
				return false;

			Plane plane = new Plane (tileMap.transform.TransformDirection (Vector3.forward), tileMap.position);
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			Vector3 position = tileMap.position;

			float distance;
			if (plane.Raycast (ray, out distance))
				point = (Coordinate)(ray.origin + (ray.direction.normalized * distance)- position);

			bool result = (point.x >= 0 && point.x < position.x + tileMap.Width && point.y >= 0 && point.y < tileMap.Height);
			Handles.color = result ? (tileMap.selectedScriptableTool  >= 0 ?  new Color (0.5f, 1, 0.5f) : new Color(1, 0.75f, 0.5f)) : new Color (1, 0.5f, 0.5f);
			Handles.DrawWireCube ((Vector2)position + (Vector2)point + Vector2.one * 0.5f, Vector2.one);
			Handles.color = new Color (Handles.color.r, Handles.color.g, Handles.color.b, 0.3f);
			Handles.CubeCap (0, (Vector2)position + (Vector2)point + Vector2.one * 0.5f, Quaternion.identity, 1f);
			
			return result;
		}

		private Rect ClampToScreen (Rect r)
		{
			r.x = Mathf.Clamp (r.x, 5, Screen.width - r.width - 5);
			r.y = Mathf.Clamp (r.y, 25, Screen.height - r.height - 25);
			return r;
		}
	}
}