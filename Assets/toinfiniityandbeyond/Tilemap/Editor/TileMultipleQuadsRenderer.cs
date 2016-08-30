using UnityEngine;
using UnityEditor;
using System.Collections;

namespace toinfiniityandbeyond.Tilemapping
{
    [CustomEditor(typeof(TileMultipleQuadsRenderer))]
    public class TileMultipleQuadsRendererEditor : TileRendererEditor
    {
        public override void OnEnable() {
            base.OnEnable();
            
            TileMultipleQuadsRenderer renderer = (target as TileMultipleQuadsRenderer);
            EditorUtility.SetSelectedWireframeHidden(renderer.meshRenderer, true);
        }
	}
}
