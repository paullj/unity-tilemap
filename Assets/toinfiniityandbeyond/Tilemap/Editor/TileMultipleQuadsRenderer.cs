using UnityEngine;
using UnityEditor;
using System.Collections;

namespace toinfiniityandbeyond.Tilemapping
{
    [CustomEditor(typeof(TileMultipleQuadsRenderer))]
    public class TileMultipleQuadsRendererEditor : TileRendererEditor
    {
        TileMultipleQuadsRenderer renderer;

        public override void OnEnable() {
            base.OnEnable();
            
            renderer = (target as TileMultipleQuadsRenderer);
            EditorUtility.SetSelectedWireframeHidden(renderer.meshRenderer, true);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            Utillity.MyGUILayout.Splitter();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Force Refresh Atlas")) {
                renderer.RefreshAtlas();
            }
            GUILayout.Toggle(true, "Auto update atlas");
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture atlas: ");
            GUILayout.Label(renderer.Atlas);
            GUILayout.EndHorizontal();
        }
	}
}
