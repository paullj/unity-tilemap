using UnityEngine;
using UnityEditor;
using toinfiniityandbeyond.UI;

namespace toinfiniityandbeyond.Tilemapping
{
	[CustomEditor (typeof (SimpleTile))]
	public class TileCustomEditor : ScriptableTileEditor
	{
		private SimpleTile tile;

		private void OnEnable ()
		{
			tile = (SimpleTile)target;
		}
		public override void OnInspectorGUI ()
		{
			GUILayout.Space (10);

			GUILayout.Label ("Sprite:", CustomStyles.leftBoldLabel);

			float width = EditorGUIUtility.labelWidth;
			if (!tile.IsValid)
			{
				GUI.color = new Color (1, 0.5f, 0.5f);
			}
			if (GUILayout.Button (GUIContent.none, CustomStyles.centerWhiteBoldLabel, GUILayout.Width (width), GUILayout.Height (width)))
			{
				EditorGUIUtility.ShowObjectPicker<Sprite> (tile.sprite, false, "", 0);
			}
			Rect r = GUILayoutUtility.GetLastRect ();

			Texture2D texture = tile.IsValid ? TextureFromSprite (tile.sprite) : new Texture2D (16, 16);
			GUI.DrawTexture (r, texture);
			GUI.color = Color.white;

			GUIStyle labelStyle = new GUIStyle (CustomStyles.centerWhiteBoldLabel);
			if (!tile.sprite)
				GUI.Label (r, "Tile not valid!\nSprite cannot be left empty", labelStyle);
			else if (!tile.IsValid)
				GUI.Label (r, "Tile not valid!\nEnable Read/Write in import settings", labelStyle);


			/*
			if (!tile.sprite) 
				EditorGUILayout.HelpBox ("This tile is not valid, main sprite (15) cannot be left empty.", MessageType.Error);
			else if (!tile.IsValid)
				EditorGUILayout.HelpBox ("This tile is not valid, please check that Read/Write is enabled in the main sprite (15)'s import settings", MessageType.Error);
			*/
			if (Event.current.commandName == "ObjectSelectorUpdated")
			{
				tile.sprite = EditorGUIUtility.GetObjectPickerObject () as Sprite;
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty (this);
			}
		}
		private Texture2D TextureFromSprite (Sprite sprite)
		{
			var texture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
			var pixels = sprite.texture.GetPixels ((int)sprite.textureRect.x,
													(int)sprite.textureRect.y,
													(int)sprite.textureRect.width,
													(int)sprite.textureRect.height);
			texture.SetPixels (pixels);
			texture.filterMode = sprite.texture.filterMode;
			texture.wrapMode = sprite.texture.wrapMode;
			texture.Apply ();
			return texture;
		}
	}
}