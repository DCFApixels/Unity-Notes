using System;
using UnityEngine;

namespace DCFApixels.Notes
{
    [Serializable]
    public class AuthorInfo : NotePropertyInfo
    {
        public Color32 color = new Color32(255, 255, 255, 255);
        internal AuthorInfo(int id) : base(id) { }
    }
}
