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

			var boldCentredStyle = EditorStyles.boldLabel;
			boldCentredStyle.alignment = TextAnchor.MiddleCenter;
			boldCentredStyle.wordWrap = true;

			EditorGUI.BeginChangeCheck ();
			IsInEditMode = GUILayout.Toggle (IsInEditMode, "", "Button", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f));
			GUI.Label (GUILayoutUtility.GetLastRect(), (IsInEditMode ? "Exit" : "Enter") + " Edit Mode", boldCentredStyle);
			
			if (EditorGUI.EndChangeCheck ())
			{
				if (IsInEditMode)
				{
					(SceneView.sceneViews [0] as SceneView).in2DMode = true;
					Tools.hidden = true;
					Tools.current = Tool.None;
					OnSceneGUI ();
				}
				else
				{
					Tools.hidden = false;
					Tools.current = Tool.Move;
				}
			}
			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (spDebug);
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (spWidth);
			EditorGUILayout.PropertyField (spHeight);
			serializedObject.ApplyModifiedProperties ();

			if (EditorGUI.EndChangeCheck ())
				tileMap.Resize ();

			EditorGUILayout.Space ();


			serializedObject.ApplyModifiedProperties ();
		}
	}
}