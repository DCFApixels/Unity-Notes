using System;
using UnityEngine;

namespace DCFApixels.Notes
{
    [Serializable]
    public abstract class NotePropertyInfo
    {
        [SerializeField]
        [HideInInspector]
        internal int _id;
        public string name = "Name";

        protected NotePropertyInfo(int id)
        {
            _id = id;
        }

        public override int GetHashCode()
        {
            return _id;
        }
    }
}
