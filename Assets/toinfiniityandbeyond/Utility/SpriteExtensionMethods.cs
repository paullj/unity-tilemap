using UnityEngine;

public static class ExtensionMethods {

	public static Texture2D ToTexture2D(this Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width)
		{
			Texture2D texture = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height );
			texture.SetPixels(newColors);
			texture.Apply();
			return texture;
		}
		return sprite.texture;
	}
}
