using UnityEditor;
using System.IO;

public class StencilEditor
{
	[MenuItem ("Assets/Create/Tilemap/New C# Stencil Script")]
	private static void CreateStencilScript ()
	{
		string name = "NewStencilScript";
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
				outfile.WriteLine ("	///");
				outfile.WriteLine ("	///<summary>Inherits from generic class Stencil, and is used to paint tiles to a tilemap");
				outfile.WriteLine ("	///");
				outfile.WriteLine ("	public class "+ name + " : Stencil");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		public override void Paint (Coordinate point, Tile tile, TileMap map)");
				outfile.WriteLine ("		{");
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
}