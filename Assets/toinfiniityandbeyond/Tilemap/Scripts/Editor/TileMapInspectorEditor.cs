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
			tileMap.IsInEditMode = GUILayout.Toggle (tileMap.IsInEditMode, "", "Button", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f));
			GUI.Label (GUILayoutUtility.GetLastRect(), (tileMap.IsInEditMode ? "Exit" : "Enter") + " Edit Mode", CustomStyles.centerBoldLabel);
			
			if (EditorGUI.EndChangeCheck ())
			{
				if (tileMap.IsInEditMode)
					OnEnterEditMode ();
				else
					OnExitEditMode ();
			}
			EditorGUILayout.Space ();
			GUILayout.EndHorizontal ();
			GUILayout.Label ("Settings", CustomStyles.leftBoldLabel);
			EditorGUILayout.PropertyField (spDebug);
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (spWidth);
			EditorGUILayout.PropertyField (spHeight);
			serializedObject.ApplyModifiedProperties ();

			if (EditorGUI.EndChangeCheck ())
				tileMap.Resize ();

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

			serializedObject.ApplyModifiedProperties ();
		}
	}
}