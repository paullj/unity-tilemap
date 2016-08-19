using UnityEngine;
using System;

namespace toinfiniityandbeyond.Tilemapping
{
	/// <summary>
	/// The base class for a ScriptableTool
	/// </summary>
	[Serializable]
	public abstract class ScriptableTool
	{
		/// <summary>
		/// The default constructor where you can set up default variable values
		/// </summary>
		public ScriptableTool ()
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
		/// Small summary on what the ScriptableTool does
		/// </summary>
		public virtual KeyCode Shortcut { get { return KeyCode.None; } }
		/// <summary>
		/// Small summary on what the ScriptableTool does
		/// </summary>
		public virtual string Description { get { return string.Empty; } }

		/// <summary>
		/// Called by the tilemap to paint tiles
		/// </summary>
		/// <param name="point">Where you want to use the tool</param>
		/// <param name="tile">The ScriptableTile you want to use</param>
		/// <param name="map">What you want to use the tool on</param>
		public abstract bool Use (Coordinate point, ScriptableTile tile, TileMap map);
	}
}