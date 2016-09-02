using UnityEditor;
using System.IO;

public class ScriptFactory
{
	[MenuItem ("Assets/Create/Tilemap/New C# Tool Script")]
	private static void CreatScriptableToolScript ()
	{
		string name = "NewScriptableTool";
		string filename = name + ".cs";

		string path;
		try
		{
			// Private implementation of a filenaming function which puts the file at the selected path.
			System.Type assetdatabase = typeof (UnityEditor.AssetDatabase);
			path = (string)assetdatabase.GetMethod ("GetUniquePathNameAtSelectedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke (assetdatabase, new object [] { filename });
		}
		catch
		{
			// Protection against implementation changes.
			path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath ("Assets/" + filename);
		}
		
		if (File.Exists (path) == false)
		{
			using (StreamWriter outfile = new StreamWriter (path))
			{
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("");
				outfile.WriteLine ("namespace toinfiniityandbeyond.Tilemapping");
				outfile.WriteLine ("{");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	[System.Serializable]");
				outfile.WriteLine ("	public class " + name + " : ScriptableTool");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		//Public variables automatically get exposed in the tile editor");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		public "+name+"() : base()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			//You can set any defaults for variables here");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		/*");
				outfile.WriteLine ("		//If you want this ScriptableTool to have a shortcut set it here");
				outfile.WriteLine ("		public override KeyCode Shortcut");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			get { return KeyCode.None; }");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		//If you want this ScriptableTool to have a description set it here");
				outfile.WriteLine ("		public override string Description");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			get { return string.Empty; }");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		*/");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		//");
				outfile.WriteLine ("		//Called when LMB is held down");
				outfile.WriteLine ("		public override void OnClick (Point point, ScriptableTile tile, TileMap map)");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (tile == null && map == null)");
				outfile.WriteLine ("				return;");
				outfile.WriteLine ("			");
				outfile.WriteLine ("			map.SetTileAt (point, tile);");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		/*");
				outfile.WriteLine ("		//Called when LMB is clicked down");
				outfile.WriteLine ("		public override void OnClickDown (Point point, ScriptableTile tile, TileMap map)");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			base.OnClickDown(point, tile, map);");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		//Called when LMB is released");
				outfile.WriteLine ("		public override void OnClickUp (Point point, ScriptableTile tile, TileMap map)");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			base.OnClickUp(point, tile, map);");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		//The region to draw a tool preview for");
				outfile.WriteLine ("		public override List<Point> GetRegion (Point point, ScriptableTile tile, TileMap map)");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			base.GetRegion(point, tile, map);");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		*/");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("}");

				outfile.Close ();
			}
		}
		AssetDatabase.Refresh ();

		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (path);
		EditorGUIUtility.PingObject (Selection.activeObject);
	}

	[MenuItem ("Assets/Create/Tilemap/New C# Tile Script")]
	private static void CreateTileScript ()
	{
		string name = "NewTileScript";
		string filename = name + ".cs";

		string path;
		try
		{
			// Private implementation of a filenaming function which puts the file at the selected path.
			System.Type assetdatabase = typeof (UnityEditor.AssetDatabase);
			path = (string)assetdatabase.GetMethod ("GetUniquePathNameAtSelectedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke (assetdatabase, new object [] { filename });
		}
		catch
		{
			// Protection against implementation changes.
			path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath ("Assets/" + filename);
		}

		if (File.Exists (path) == false)
		{
			using (StreamWriter outfile = new StreamWriter (path))
			{
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("	");
				outfile.WriteLine ("namespace toinfiniityandbeyond.Tilemapping");
				outfile.WriteLine ("{");
				outfile.WriteLine ("	//Remember to change these names to something more meaningful!");
				outfile.WriteLine (@"	[CreateAssetMenu (fileName = ""New Script Tile"", menuName = ""Tilemap/Tiles/NewTileScript"")]");
				outfile.WriteLine ("	public class " + name + " : ScriptableTile");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		[SerializeField]");
				outfile.WriteLine ("		private Sprite sprite;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		//Returns if this tile is okay to be used in the tile map");
				outfile.WriteLine ("		//For example: if this tile doesn't have a Read/Write enabled sprite it will return false");
				outfile.WriteLine ("		public override bool IsValid");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			get");
				outfile.WriteLine ("			{");
				outfile.WriteLine ("				if(sprite == null)");
				outfile.WriteLine ("					return false;");
				outfile.WriteLine ("				");
				outfile.WriteLine ("				try");
				outfile.WriteLine ("				{");
				outfile.WriteLine ("					sprite.texture.GetPixel(0, 0);");
				outfile.WriteLine ("				}");
				outfile.WriteLine ("				catch(UnityException e)");
				outfile.WriteLine ("				{");
				outfile.WriteLine ("					return false;");
				outfile.WriteLine ("				}");
				outfile.WriteLine ("				return true;");
				outfile.WriteLine ("			}");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		public override Sprite GetSprite (TileMap tilemap, Point position = default (Point))");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			return sprite;");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		public override Texture2D GetIcon ()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (!IsValid) return null;");
				outfile.WriteLine ("			return sprite.ToTexture2D();");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		//Called when the inspector has been edited");
				outfile.WriteLine ("		private void OnValidate ()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			//Can be used to check if variables are valid");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("}");

				outfile.Close ();
			}
		}
		AssetDatabase.Refresh ();

		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (path);
		EditorGUIUtility.PingObject (Selection.activeObject);
	}
}