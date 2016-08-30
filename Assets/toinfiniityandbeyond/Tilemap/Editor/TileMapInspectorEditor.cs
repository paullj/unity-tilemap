using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
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
			spDebug = serializedObject.FindProperty ("debug");
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
			TileRenderer renderer = tileMap.GetComponent<TileRenderer>();
			if(renderer == null) {
				Texture2D tex = EditorGUIUtility.FindTexture("console.erroricon");
				if(GUILayout.Button(new GUIContent( "No TileRenderer attached! Click here to add one.", tex), EditorStyles.helpBox)) {
					GenericMenu menu = new GenericMenu();
					foreach (Type type in Assembly.GetAssembly (typeof (TileRenderer)).GetTypes ().Where (t => t.IsClass && !t.IsAbstract && t.IsSubclassOf (typeof (TileRenderer))))
					{
						menu.AddItem(new GUIContent(type.Name), false, () => { tileMap.gameObject.AddComponent(type); });
					}
					menu.ShowAsContext();
				} 
			}
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

			GUILayout.Label ("Settings", CustomStyles.leftBoldLabel);
			EditorGUILayout.PropertyField (spDebug);
			if(spDebug.boolValue)
				EditorGUILayout.HelpBox("Enabling debug will decrease performace becaues of the use of Debug.Log", MessageType.Warning);

			int width = spWidth.intValue;
			width = EditorGUILayout.IntField (spWidth.displayName, width);
			int height = spHeight.intValue;
			height = EditorGUILayout.IntField (spHeight.displayName, height);
			
			if (width != spWidth.intValue || height != spHeight.intValue) {
				tileMap.Resize (width, height);
				OnSceneGUI();
			}
			EditorGUILayout.Space ();

			GUILayout.Label ("Tools", CustomStyles.leftBoldLabel);
			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Force Refresh"))
			{
				tileMap.UpdateTileMap ();
			}
			GUI.color = new Color(1f, 0.5f, 0.5f);
			if(GUILayout.Button("Clear All Tiles"))
			{
				if(EditorUtility.DisplayDialog("Are you sure?", "Do you really want to clear this tilemap? You will not be able to undo this!", "Okay", "Cancel"))
					tileMap.Reset();
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Import"))
			{

			}
			if (GUILayout.Button ("Export"))
			{
				
			}
			GUILayout.EndHorizontal();
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