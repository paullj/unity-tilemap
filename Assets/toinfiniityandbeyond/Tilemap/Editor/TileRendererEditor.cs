using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace toinfiniityandbeyond.Tilemapping
{
    [CustomEditor(typeof(TileRenderer))]
    public class TileRendererEditor : Editor
    {
		SerializedProperty spColor;
		SerializedProperty spMaterial;
		SerializedProperty spSortingLayer;
		SerializedProperty spOrderInLayer;

		public virtual void OnEnable() {
			spColor = serializedObject.FindProperty("color");
			spMaterial = serializedObject.FindProperty("material");
			spSortingLayer = serializedObject.FindProperty("sortingLayer");
			spOrderInLayer = serializedObject.FindProperty("orderInLayer");
		}

		public virtual void OnDisable() {

		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(spColor);
			EditorGUILayout.PropertyField(spMaterial);

			var sortingLayerNames = SortingLayer.layers.Select(l => l.name).ToArray();
          
            if (sortingLayerNames != null) {

                // Look up the layer name using the current layer ID
                string oldName = SortingLayer.IDToName(spSortingLayer.intValue);

                // Use the name to look up our array index into the names list
                int oldLayerIndex = Array.IndexOf(sortingLayerNames, oldName);

                // Show the popup for the names
                int newLayerIndex = EditorGUILayout.Popup(spSortingLayer.displayName, oldLayerIndex, sortingLayerNames);

                // If the index changes, look up the ID for the new index to store as the new ID
                if (newLayerIndex != oldLayerIndex) {
                    spSortingLayer.intValue = SortingLayer.NameToID(sortingLayerNames[newLayerIndex]);
                }
			}
            else {
                int newValue = EditorGUILayout.IntField(spSortingLayer.displayName ,spSortingLayer.intValue);
                if (newValue != spSortingLayer.intValue) {
                    spSortingLayer.intValue = newValue;
                }
                EditorGUI.EndProperty();
            }

			EditorGUILayout.PropertyField(spOrderInLayer);

			serializedObject.ApplyModifiedProperties();
		}
    }
}