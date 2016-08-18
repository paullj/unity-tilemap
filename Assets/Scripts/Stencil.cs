using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	/// <summary>
	/// The base class for stencil
	/// </summary>
	[Serializable]
	public abstract class Stencil
	{
		/// <summary>
		/// The default constructor where you can set up default variable values
		/// </summary>
		public Stencil ()
		{

		}
		/// <summary>
		/// The name (which is the class name)
		/// </summary>
		public string Name { get { return this.GetType ().Name; } }
		/// <summary>
		/// Unique ID of this stencil
		/// </summary>
		public int ID { get { return Animator.StringToHash (Name); } }
		/// <summary>
		/// Small summary on what the stencil does
		/// </summary>
		public virtual KeyCode ShortcutKeyCode { get { return KeyCode.None; } }
		/// <summary>
		/// Small summary on what the stencil does
		/// </summary>
		public virtual string Description { get { return string.Empty; } }

		/// <summary>
		/// Called by the tilemap to paint tiles
		/// </summary>
		/// <param name="point">Where you want to paint the tile</param>
		/// <param name="tile">The BaseTile you want to paint</param>
		/// <param name="map">What you want to paint on</param>
		public abstract bool Paint (Coordinate point, BaseTile tile, TileMap map);
	}
}