using UnityEngine;
using System;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
    [Serializable]
    public class Rectangle : ScriptableTool
    {
        public bool filled;
        private Point start;
        private Point end;

        //An empty constructor
        public Rectangle() : base()
        {
            filled = true; 
        }
        //Sets the shortcut key to 'P'
        public override KeyCode Shortcut
        {
            get { return KeyCode.W; }
        }
        //Sets the tooltip description
        public override string Description
        {
            get { return "Draws a rectangle"; }
        }
        //Called when the left mouse button is held down
        public override void OnClick(Point point, ScriptableTile tile, TileMap map)
        {
            //Return if the tilemap is null/empty
            if (map == null)
                return;

            //If we haven't already started an operation, start one now
            //This is for undo/ redo support
            if (!map.OperationInProgress())
                map.BeginOperation();

            end = point;
            //Set the tile at the specified point to the specified tile
        }
        //Called when the left mouse button is initially held down
        public override void OnClickDown(Point point, ScriptableTile tile, TileMap map)
        {
            base.OnClickDown(point, tile, map);
            start = end = point;
        }
        public override void OnClickUp(Point point, ScriptableTile tile, TileMap map)
        {
            base.OnClickUp(point, tile, map);
            for (int i = 0; i < region.Count; i++)
            {
                map.SetTileAt(region[i], tile);
            }
            start = end = point;
            region = new List<Point>();
        }
        public override List<Point> GetRegion(Point point, ScriptableTile tile, TileMap map)
        {
            region = new List<Point>();
            if (end == start)
                return base.GetRegion(point, tile, map);

            int x0 = Mathf.Min(start.x, end.x),
                x1 = Mathf.Max(start.x, end.x),
                y0 = Mathf.Min(start.y, end.y),
                y1 = Mathf.Max(start.y, end.y);

            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    if(filled || (x == x0 || x == x1 || y == y0 || y == y1))
                        region.Add(new Point(x, y));
                }
            }

            return region;
        }
    }
}