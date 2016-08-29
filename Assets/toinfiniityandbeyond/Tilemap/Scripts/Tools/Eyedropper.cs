using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
    [Serializable]
    public class Eyedropper : ScriptableTool
    {
        public Eyedropper() : base()
        {

        }
        public override KeyCode Shortcut { get { return KeyCode.I; } }
        public override string Description { get { return "Sets the primary tile to whatever you click"; } }

        public override void OnClickDown(Point point, ScriptableTile tile, TileMap map)
        {
        }
        public override void OnClick(Point point, ScriptableTile tile, TileMap map)
		{

		}	
        public override void OnClickUp(Point point, ScriptableTile tile, TileMap map)
        {
            map.primaryTile = map.GetTileAt(point);
        }	
    }
}
