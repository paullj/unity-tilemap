using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using toinfiniityandbeyond.UI;

namespace toinfiniityandbeyond.Tilemapping {
	public class ScriptableTileEditor : Editor {
		ScriptableTile scriptableTile;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
		}
		protected override void OnHeaderGUI ()
		{
			scriptableTile = target as ScriptableTile;
			GUILayout.Space (10);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (scriptableTile.GetTexture (null, Point.zero));
			GUILayout.BeginVertical ();
			GUILayout.Label (scriptableTile.name, CustomStyles.leftBoldLabel);
			GUILayout.Label ("Tile Type: "+scriptableTile.GetType ().Name, CustomStyles.leftMiniLabel);
			GUILayout.EndVertical ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Help", EditorStyles.miniButton))
			{
				Application.OpenURL ("https://github.com/toinfiniityandbeyond/Tilemap/wiki/"+ scriptableTile.GetType ().Name);
			}
			GUILayout.EndHorizontal ();
			CustomGUILayout.Splitter ();
		}
		/*
		[DidReloadScripts]
		static ScriptableTileEditor ()
		{
			EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
		}

		static void ItemOnGUI (string guid, Rect rect)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath (guid);

			ScriptableTile obj = AssetDatabase.LoadAssetAtPath (assetPath, typeof (ScriptableTile)) as ScriptableTile;

			if (obj != null)
			{
				rect.width = rect.height;
				GUI.DrawTexture (new Rect (rect.x + 10, rect.y + 10, rect.width - 20, rect.width - 20), obj.GetTexture (null, Point.zero));
			}
		}
		*/
	}
}