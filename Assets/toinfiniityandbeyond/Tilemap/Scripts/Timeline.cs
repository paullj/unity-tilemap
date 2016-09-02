using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
    public class Timeline
    {
        #region Variables
        private List<List<ChangeElement>> changes;
        private int current = -1;
        #endregion
        #region Methods
        public bool CanUndo
        {
            get { return !(current == -1); }
        }
        public bool CanRedo
        {
            get { return !(current + 1 == changes.Count); }
        }
        #endregion
        #region Constructor
        public Timeline()
        {
            changes = new List<List<ChangeElement>>();
        }
        #endregion
        #region Functions
        public List<ChangeElement> Undo()
        {
            if (CanUndo)
            {
                //current--;
                return changes[current--];
            }
            //At oldest revision, nothing to Undo
            //Return empty list
            return new List<ChangeElement>();
        }

        public List<ChangeElement> Redo()
        {
            if (CanRedo)
            {
                //current++;
                return changes[++current];
            }
            //At newest revision, nothing to redo
            //Return empty list
            return new List<ChangeElement>();
        }

        public void PushChanges(List<ChangeElement> change)
        {
            //Any tiles changed?
            if (change == null || change.Count == 0)
                return;

            //Are we at the newest revision?
            //If not, all future changes must be erased
            if (current - 1 != changes.Count)
                changes.RemoveRange(current + 1, changes.Count - current - 1);

            changes.Add(change);
            current++;
        }
        #endregion
    }

    public class ChangeElement
    {
        public int x;
        public int y;

        public ScriptableTile from;
        public ScriptableTile to;

        public ChangeElement(int _x, int _y, ScriptableTile _from, ScriptableTile _to)
        {
            x = _x;
            y = _y;
            from = _from;
            to = _to;
        }
    }
}