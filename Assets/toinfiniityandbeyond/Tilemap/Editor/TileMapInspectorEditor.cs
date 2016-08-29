using UnityEngine;
using UnityEditor;
using toinfiniityandbeyond.UI;

namespace toinfiniityandbeyond.Tilemapping
{
	partial class TileMapEditor : Editor
	{
		SerializedProperty spDebug;
		SerializedProperty spWidth;
		SerializedProperty spHeight;

		partial void OnInspectorEnable ()
		{
			spDebug = serializedObject.FindProperty ("debugMode");
			spWidth = serializedObject.FindProperty ("width");
			spHeight = serializedObject.FindProperty ("height");
		}

		partial void OnInspectorDisable ()
		{

		}
		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
						
			EditorGUILayout.Space ();
			EditorGUI.BeginChangeCheck ();
			
			tileMap.isInEditMode = GUILayout.Toggle (tileMap.isInEditMode, "", "Button", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f));
			string toggleButtonText = (tileMap.isInEditMode ? "Exit" : "Enter") + " Edit Mode [TAB]";
			GUI.Label (GUILayoutUtility.GetLastRect(), toggleButtonText, CustomStyles.centerBoldLabel);
		
			if (EditorGUI.EndChangeCheck ())
			{
				if (tileMap.isInEditMode)
					OnEnterEditMode ();
				else
					OnExitEditMode ();
			}
			EditorGUILayout.Space ();
			GUILayout.EndHorizontal ();
			GUILayout.Label ("Settings", CustomStyles.leftBoldLabel);
			EditorGUILayout.PropertyField (spDebug);

			int width = spWidth.intValue;
			width = EditorGUILayout.IntField (spWidth.displayName, width);
			int height = spHeight.intValue;
			height = EditorGUILayout.IntField (spHeight.displayName, height);

			if (width != spWidth.intValue || height != spHeight.intValue) {
				tileMap.Resize (width, height);
			}
			EditorGUILayout.Space ();

			if(GUILayout.Button("Force Refresh"))
			{
				tileMap.UpdateTileMap ();
			}
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Import"))
			{

			}
			if (GUILayout.Button ("Export"))
			{
				
			}

			if (Event.current.type == EventType.KeyDown && Event.current.isKey && Event.current.keyCode == KeyCode.Tab)
			{
				tileMap.isInEditMode = !tileMap.isInEditMode;
			}
			if (Event.current.type == EventType.KeyDown && Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
			{
				tileMap.isInEditMode = false;
			}
			
			serializedObject.ApplyModifiedProperties ();
		}
	}
}