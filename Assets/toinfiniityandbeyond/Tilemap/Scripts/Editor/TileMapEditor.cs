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
	[CustomEditor (typeof (TileMap)), CanEditMultipleObjects]
	partial class TileMapEditor : Editor
	{
		private TileMap tileMap;
		private bool IsInEditMode = false;

		partial void OnInspectorEnable ();
		partial void OnInspectorDisable ();
		partial void OnSceneEnable ();
		partial void OnSceneDisable ();
		
		private void OnEnable ()
		{
			tileMap = (TileMap)target;
			
			IsInEditMode = EditorPrefs.GetBool (tileMap.GetInstanceID()+"_IsInEditMode", false);

			OnInspectorEnable ();
			OnSceneEnable ();
		}

		private void OnDisable ()
		{
			EditorPrefs.SetBool (tileMap.GetInstanceID()+"_IsInEditMode", IsInEditMode);

			OnInspectorDisable ();
			OnSceneDisable ();
		}
	}
}