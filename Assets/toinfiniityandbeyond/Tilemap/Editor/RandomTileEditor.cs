using UnityEngine;
using UnityEditor;
using toinfiniityandbeyond.Utillity;

namespace toinfiniityandbeyond.Tilemapping
{
    [CustomEditor(typeof(RandomTile))]
    public class RandomTileEditor : ScriptableTileEditor
    {
        private RandomTile tile;

        private void OnEnable()
        {
            tile = (RandomTile)target;
        }
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
			EditorGUILayout.HelpBox("This tile randomly chooses a sprite from the list below to render. It is based on position and the seed.", MessageType.Info);
          	tile.globalSeed = EditorGUILayout.IntField("Global Seed:", tile.globalSeed);
			GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Sprites:", MyStyles.leftBoldLabel);
           	GUILayout.FlexibleSpace();
			GUI.color = new Color(0.5f, 1, 0.5f);
            if (GUILayout.Button("Add New"))
            {
				tile.sprites.Add(tile.sprites[tile.sprites.Count-1]);
            }
			GUI.color = Color.white;
            GUILayout.EndHorizontal();
			GUILayout.Space(10);
            float width = EditorGUIUtility.labelWidth / 3;
            for (int i = 0; i < tile.sprites.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (!tile.IsValid)
                {
                    GUI.color = new Color(1, 0.5f, 0.5f);
                }
                if (GUILayout.Button(GUIContent.none, MyStyles.centerWhiteBoldLabel, GUILayout.Width(width), GUILayout.Height(width)))
                {
                    EditorGUIUtility.ShowObjectPicker<Sprite>(tile.sprites[i], false, "", i);
                }
                Rect r = GUILayoutUtility.GetLastRect();

                Texture2D texture = tile.IsValid ? tile.sprites[i].ToTexture2D() : new Texture2D(16, 16);
                GUI.DrawTexture(r, texture);
                GUI.color = Color.white;

                GUIStyle labelStyle = new GUIStyle(MyStyles.centerWhiteBoldLabel);
                if (!tile.sprites[i])
                    GUI.Label(r, "Tile not valid!\nSprite cannot be left empty", labelStyle);
                else if (!tile.IsValid)
                    GUI.Label(r, "Tile not valid!\nEnable Read/Write in import settings", labelStyle);

                GUILayout.FlexibleSpace();
				GUI.color = new Color(1, 0.5f, 0.5f);
                if (GUILayout.Button("Delete"))
                {
					tile.sprites.RemoveAt(i);
                }
				GUI.color = Color.white;
                GUILayout.EndHorizontal();
                MyGUILayout.Splitter();
            }

            /*
			if (!tile.sprite) 
				EditorGUILayout.HelpBox ("This tile is not valid, main sprite (15) cannot be left empty.", MessageType.Error);
			else if (!tile.IsValid)
				EditorGUILayout.HelpBox ("This tile is not valid, please check that Read/Write is enabled in the main sprite (15)'s import settings", MessageType.Error);
			*/
            if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                int index = EditorGUIUtility.GetObjectPickerControlID();
                tile.sprites[index] = EditorGUIUtility.GetObjectPickerObject() as Sprite;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }
    }
}