using UnityEngine;
using UnityEditor;
using System.IO;
using toinfiniityandbeyond.Tilemapping;

public static class ScriptableObjectFactory
{
//	[MenuItem ("Assets/Create/test", false)]
	public static void CreateInstance ()
	{
		GenericMenu menu = new GenericMenu ();
		AssetDatabase.Refresh ();

		string [] guids = AssetDatabase.FindAssets (string.Format ("t:{0}", typeof (ScriptableTile)));
		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath (guids [i]);
			ScriptableTile asset = AssetDatabase.LoadAssetAtPath<ScriptableTile> (assetPath);
			if (asset != null)
			{
				menu.AddItem (new GUIContent(asset.name), true, () => { CreateAsset (asset.GetType ()); });
			}
		}
//		menu.ShowAsContext ();
		menu.DropDown (new Rect (100, 100, 100, 100));
	}

	private static void CreateAsset (System.Type type)
	{
		var asset = ScriptableObject.CreateInstance (type);
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "")
		{
			path = "Assets";
		}
		else if (Path.GetExtension (path) != "")
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + type.ToString () + ".asset");
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}
}
