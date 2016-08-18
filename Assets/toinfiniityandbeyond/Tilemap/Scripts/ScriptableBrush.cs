using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	/// <summary>
	/// The base class for a ScriptableBrush
	/// </summary>
	[Serializable]
	public abstract class ScriptableBrush
	{
		/// <summary>
		/// The default constructor where you can set up default variable values
		/// </summary>
		public ScriptableBrush ()
		{

		}
		/// <summary>
		/// The name (which is the class name)
		/// </summary>
		public string Name { get { return this.GetType ().Name; } }
		/// <summary>
		/// Unique ID of this ScriptableBrush
		/// </summary>
		public int ID { get { return Animator.StringToHash (Name); } }
		/// <summary>
		/// Small summary on what the ScriptableBrush does
		/// </summary>
		public virtual KeyCode ShortcutKeyCode { get { return KeyCode.None; } }
		/// <summary>
		/// Small summary on what the ScriptableBrush does
		/// </summary>
		public virtual string Description { get { return string.Empty; } }

		/// <summary>
		/// Called by the tilemap to paint tiles
		/// </summary>
		/// <param name="point">Where you want to paint the tile</param>
		/// <param name="tile">The BaseTile you want to paint</param>
		/// <param name="map">What you want to paint on</param>
		public abstract bool Paint (Coordinate point, ScriptableTile tile, TileMap map);
	}
}