using System;
using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
	public class Timeline
	{

		private List<List<ChangeElement>> changes;
		private int current = -1;

		public List<ChangeElement> undo ()
		{
			//At oldest revision, nothing to undo
			//Return empty list
			if (current == -1)
				return new List<ChangeElement> ();

			//current--;
			return changes [current--];
		}

		public List<ChangeElement> redo ()
		{
			//At newest revision, nothing to redo
			//Return empty list
			if (current + 1 == changes.Count)
				return new List<ChangeElement> ();

			//current++;
			return changes [++current];
		}


		public void pushChanges (List<ChangeElement> change)
		{
			//Any tiles changed?
			if (change.Count == 0)
				return;

			//Are we at the newest revision?
			//If not, all future changes must be erased
			if (current - 1 != changes.Count)
				changes.RemoveRange (current + 1, changes.Count - current - 1);

			changes.Add (change);
			current++;
		}

		public Timeline ()
		{
			changes = new List<List<ChangeElement>> ();
		}
	}

	public class ChangeElement
	{
		public int x;
		public int y;

		public ScriptableTile from;
		public ScriptableTile to;

		public ChangeElement (int _x, int _y, ScriptableTile _from, ScriptableTile _to)
		{
			x = _x;
			y = _y;
			from = _from;
			to = _to;
		}


	}

}