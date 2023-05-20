﻿#pragma warning disable CS0414
using DCFApixels.Notes.Editors;
using UnityEngine;

namespace DCFApixels.Notes
{
    using static NotesConsts;
    [AddComponentMenu("Notes/" + nameof(Note), 30)]
    internal class Note : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private string _text = "Enter text...";
        [SerializeField]
        private float _height = DEFAULT_NOTE_HEIGHT;
        [SerializeField]
        private bool _drawIcon = true;

        [SerializeField, HideInInspector]
        private int _authorID;
        [SerializeField, HideInInspector]
        private int _typeID;

        private AuthorInfo _author;
        private NoteTypeInfo _type;
#endif
        internal void UpdateRefs()
        {
#if UNITY_EDITOR
            _author = NotesSettings.Instance.GetAuthorInfoOrDummy(_authorID);
            _type = NotesSettings.Instance.GetNoteTypeInfoOrDummy(_authorID);
#endif
        }

        #region Properties
        public float Height
        {
            get
            {
#if UNITY_EDITOR
                return _height;
#else
                return default;
#endif
            }
        }
        public string Text
        {
            get
            {
#if UNITY_EDITOR
                return _text;
#else
                return string.Empty;
#endif
            }
        }
        public AuthorInfo Author
        {
            get
            {
#if UNITY_EDITOR
                if (_author == null) UpdateRefs();
                return _author;
#else
                return null;
#endif
            }
            set
            {
#if UNITY_EDITOR
                _author = value;
                _authorID = value._id;
#endif
            }
        }
        public NoteTypeInfo Type
        {
            get
            {
#if UNITY_EDITOR
                if (_type == null) UpdateRefs();
                return _type;
#else
                return null;
#endif
            }
            set
            {
#if UNITY_EDITOR
                _type = value;
                _typeID = value._id;
#endif
            }
        }
        public bool DrawIcon
        {
            get
            {
#if UNITY_EDITOR
                return _drawIcon;
#else
                return default;
#endif
            }
        }
        #endregion
    }
}
