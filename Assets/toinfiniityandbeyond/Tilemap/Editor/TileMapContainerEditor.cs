using UnityEditor;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    [CustomEditor(typeof(TileMapContainer))]
    public class TileMapContainerEditor : Editor
    {
        TileMapContainer container;

        public override void OnInspectorGUI()
        {
			container = (target as TileMapContainer);
            EditorGUILayout.HelpBox("This is just a container of tilemap data", MessageType.Info);

            Utillity.MyGUILayout.Splitter();
            if (GUILayout.Button("Import as New TileMap"))
            {

                GameObject go = new GameObject(container.name);
				TileMap map = go.AddComponent<TileMap>();
                map.Resize(container.width, container.height);
                for (int x = 0; x < container.width; x++)
                {
                    for (int y = 0; y < container.height; y++)
                    {
                        map.SetTileAt(x, y, container.map[x + y * container.width]);
                    }
                }
            }
        }
    }
}