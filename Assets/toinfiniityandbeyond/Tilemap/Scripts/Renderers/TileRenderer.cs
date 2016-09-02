using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	[ExecuteInEditMode, DisallowMultipleComponent, RequireComponent(typeof(TileMap)), HelpURL("https://github.com/toinfiniityandbeyond/Tilemap/wiki/TileRenderer-Component")]
	public abstract class TileRenderer : MonoBehaviour
	{
		public Color color;
		public Material material;

		public int sortingLayer;
		public int orderInLayer;

		[SerializeField]
		protected TileMap tileMap;
		[SerializeField]
		protected Transform parent;
		
		public virtual void Reset ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			color = Color.white;
			material = new Material (Shader.Find ("Sprites/Default"));

			parent = transform.FindChild("_TILERENDERER");
			if(parent == null) {
				parent = new GameObject("_TILERENDERER").GetComponent<Transform>();
				parent.SetParent(transform);
				parent.localPosition = Vector2.zero;
				parent.localScale = Vector2.one;
		       	parent.gameObject.hideFlags = HideFlags.HideInHierarchy;
			}

			Resize (tileMap.Width, tileMap.Height);
			UpdateTileMap ();
		}
		public virtual void OnEnable ()
		{
			if(!tileMap)
				tileMap = GetComponent<TileMap> ();

			tileMap.OnUpdateTileAt += UpdateTileAt;
			tileMap.OnUpdateTileMap += UpdateTileMap;
			tileMap.OnResize += Resize;
		}
		public virtual void OnDisable ()
		{
			tileMap.OnUpdateTileAt -= UpdateTileAt;
			tileMap.OnUpdateTileMap -= UpdateTileMap;
			tileMap.OnResize -= Resize;
		}
		public virtual void OnValidate()
		{
			UpdateTileMap();
		}
		public void ClearChildren() {
			if(parent.childCount > 0) {
				if (!Application.isPlaying)
				{
					while (parent.childCount > 0)
					{
						Object.DestroyImmediate(parent.GetChild(0).gameObject);
					}
				}
				else
				{
					foreach (Transform child in parent)
					{
						Object.Destroy(child.gameObject);
					}
				}   
			}
		}
		public void OnDestroy() 
		{
        	ClearChildren();
        }
		public abstract void Resize(int width, int height);
		
		public virtual void UpdateTileMap() {
			for (int x = 0; x < tileMap.Width; x++)
            {
                for (int y = 0; y < tileMap.Height; y++)
                {
                    UpdateTileAt(x, y);
                }
            }
		}

		public abstract void UpdateTileAt(int x, int y);
	}
}