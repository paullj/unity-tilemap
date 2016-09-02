using UnityEngine;
using System;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
    [Serializable]
    public class Line : ScriptableTool
    {
        private Point start;
        private Point end;

        //An empty constructor
        public Line() : base()
        {

        }
        //Sets the shortcut key to 'P'
        public override KeyCode Shortcut
        {
            get { return KeyCode.L; }
        }
        //Sets the tooltip description
        public override string Description
        {
            get { return "Draws a line"; }
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

            int x0 = start.x,
                x1 = end.x,
                y0 = start.y,
                y1 = end.y;

            int w = x1 - x0;
            int h = y1 - y0;

            int dx0 = 0, dy0 = 0, dx1 = 0, dy1 = 0;

            if (w < 0) dx0 = -1; else if (w > 0) dx0 = 1;
            if (h < 0) dy0 = -1; else if (h > 0) dy0 = 1;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            int longest = Mathf.Abs(w);
            int shortest = Mathf.Abs(h);
            if (!(longest > shortest))
            {
                longest = Mathf.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
                dx1 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                region.Add(new Point(x0, y0));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x0 += dx0;
                    y0 += dy0;
                }
                else
                {
                    x0 += dx1;
                    y0 += dy1;
                }
            }
            return region;
        }
    }
}