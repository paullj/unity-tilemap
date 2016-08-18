using UnityEditor;
using System.IO;

public class ScriptFactory
{
	[MenuItem ("Assets/Create/Tilemap/New C# Brush Script")]
	private static void CreatScriptableBrushScript ()
	{
		string name = "NewScriptableBrush";
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
				outfile.WriteLine ("	//Public variables automatically get exposed in the tile editor");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	");

				outfile.WriteLine ("	public class " + name + " : ScriptableBrush");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		public "+name+"() : base()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			//You can set any default variables here");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		//If you want this ScriptableBrush to have a shortcut set it here");
				outfile.WriteLine ("		public override KeyCode ShortcutKeyCode { get { return KeyCode.None; } }");
				outfile.WriteLine ("		//If you want this ScriptableBrush to have a description set it here");
				outfile.WriteLine ("		public override string Description { get { return string.Empty; } }");
				outfile.WriteLine ("	 ");
				outfile.WriteLine ("		//Called when you left click on a tile map when in edit mode");
				outfile.WriteLine ("		public override void Paint (Coordinate point, Tile tile, TileMap map)");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (tile == null && map == null)");
				outfile.WriteLine ("				return false;");
				outfile.WriteLine ("	");
				outfile.WriteLine ("			map.SetTileAt (point, tile);");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("}");

				outfile.Close ();
			}
		}
		AssetDatabase.Refresh ();

		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (path);
		EditorGUIUtility.PingObject (Selection.activeObject);
	}

	[MenuItem ("Assets/Create/Tilemap/New C# Tile Scripts")]
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
				outfile.WriteLine (@"	[CreateAssetMenu (fileName = ""New Script Tile"", menuName = ""Tilemap / Tiles / NewTileScript"", order = 0)]");
				outfile.WriteLine ("	public class " + name + " : ScriptableTile");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		[SerializeField]");
				outfile.WriteLine ("		private Sprite sprite;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		private Texture2D texture;");
				outfile.WriteLine ("		private Color [] colors;");
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
				outfile.WriteLine ("		public override Sprite GetSprite (TileMap tilemap, Coordinate position = default (Coordinate))");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			return sprite;");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		public override Texture2D GetTexture (TileMap tilemap, Coordinate position = default (Coordinate))");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (texture == null)");
				outfile.WriteLine ("				RebuildTexture ();");
				outfile.WriteLine ("			return texture;");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		public override Color [] GetColors (TileMap tilemap, Coordinate position = default (Coordinate))");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (colors.Length == 0)");
				outfile.WriteLine ("				RebuildTexture ();");
				outfile.WriteLine ("			return colors;");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		//Called when the inspector has been edited");
				outfile.WriteLine ("		private void OnValidate ()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			RebuildTexture ();");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		public void RebuildTexture ()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("			if (!IsValid)");
				outfile.WriteLine ("				return;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("			texture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);");
				outfile.WriteLine ("			colors = sprite.texture.GetPixels ((int)sprite.textureRect.x,");
				outfile.WriteLine ("												(int)sprite.textureRect.y,");
				outfile.WriteLine ("												(int)sprite.textureRect.width,");
				outfile.WriteLine ("												(int)sprite.textureRect.height);");
				outfile.WriteLine ("			texture.SetPixels (colors);");
				outfile.WriteLine ("			texture.filterMode = sprite.texture.filterMode;");
				outfile.WriteLine ("			texture.wrapMode = sprite.texture.wrapMode;");
				outfile.WriteLine ("			texture.Apply ();");
				outfile.WriteLine ("		}");
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