using System;
using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	[CreateAssetMenu (fileName = "New SimpleTile", menuName = "Tilemap/Tiles/SimpleTile")]
	public class SimpleTile : ScriptableTile
	{
		public Sprite sprite;

		private Texture2D texture;
		private Color [] colors;

		public override bool IsValid
		{
			get
			{
				if (sprite == null)
					return false;

				try
				{
					sprite.texture.GetPixel (0, 0);
				}
				catch(UnityException e)
				{
					return false;
				}
				return true;
			}
		}

		public override Sprite GetSprite (TileMap tilemap = null, Point position = default (Point))
		{
			return sprite;
		}
		public override Texture2D GetTexture (TileMap tilemap = null, Point position = default (Point))
		{
			//	if (texture == null)
			//		RebuildTexture ();
			return texture;
		}
		public override Color [] GetColors (TileMap tilemap = null, Point position = default (Point))
		{
			if (colors.Length == 0)
				RebuildTexture ();
			return colors;
		}

		private void OnValidate ()
		{
			RebuildTexture ();
		}

		public void RebuildTexture ()
		{
			if (!IsValid)
				return;

			texture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
			colors = sprite.texture.GetPixels ((int)sprite.textureRect.x,
													(int)sprite.textureRect.y,
													(int)sprite.textureRect.width,
													(int)sprite.textureRect.height);
			texture.SetPixels (colors);
			texture.filterMode = sprite.texture.filterMode;
			texture.wrapMode = sprite.texture.wrapMode;
			texture.Apply ();
		}
	}
}