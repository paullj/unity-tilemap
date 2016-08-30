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
		[MenuItem("GameObject/2D Object/TileMap")]
		private static void CreateTileMapGameObject()
		{
			GameObject tileObject = new GameObject ("New TileMap", typeof(TileMap));
		}

		private TileMap tileMap;

		partial void OnInspectorEnable ();
		partial void OnInspectorDisable ();
		partial void OnSceneEnable ();
		partial void OnSceneDisable ();
		
		private void OnEnable ()
		{
			tileMap = (TileMap)target;

			OnInspectorEnable ();
			OnSceneEnable ();
		}

		private void OnDisable ()
		{
			OnInspectorDisable ();
			OnSceneDisable ();
		}
	}
}