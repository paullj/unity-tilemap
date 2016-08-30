using UnityEngine;
using System.Linq;

namespace toinfiniityandbeyond.Tilemapping
{
	[ExecuteInEditMode, AddComponentMenu ("Tilemapping/TilemapRenderer"), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileRenderer-Component")]
	public abstract class TileRenderer : MonoBehaviour
	{
		public Color color;
		public Material material;

		public int sortingLayer;
		public int orderInLayer;

		[SerializeField]
		protected TileMap tileMap;

		public virtual void Reset ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			color = Color.white;
			material = new Material (Shader.Find ("Sprites/Default"));

			Resize (tileMap.Width, tileMap.Height);
			UpdateTileMap ();
		}
		public virtual void OnEnable ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			tileMap.OnUpdateTileAt += UpdateTileAt;
			tileMap.OnUpdateTileMap -= UpdateTileMap;
			tileMap.OnResize += Resize;

			Resize (tileMap.Width, tileMap.Height);
			UpdateTileMap ();
		}
		public virtual void OnDisable ()
		{
			tileMap.OnUpdateTileAt -= UpdateTileAt;
			tileMap.OnUpdateTileMap -= UpdateTileMap;
			tileMap.OnResize -= Resize;
		}
		private void OnValidate()
		{
			UpdateTileMap();
		}
		public abstract void Resize(int width, int height);
		
		public abstract void UpdateTileMap();

		public abstract void UpdateTileAt(int x, int y);
	}
}