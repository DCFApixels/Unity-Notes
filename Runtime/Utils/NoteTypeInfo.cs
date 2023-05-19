using System;
using UnityEngine;

namespace DCFApixels.Notes
{
    [Serializable]
    public class NoteTypeInfo : NotePropertyInfo
    {
        public Color32 color = new Color32(255, 255, 255, 255);
        internal NoteTypeInfo(int id) : base(id) { }
    }
}
