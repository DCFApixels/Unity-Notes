#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    [FilePath(AUTHOR + "/NotesSettings", FilePathAttribute.Location.ProjectFolder)]
    internal class NotesSettings : ScriptableSingleton<NotesSettings>, ISerializationCallbackReceiver
    {
        internal const int NO_INIT_ID = 0;
        public static NotesSettings Instance => instance;

        [SerializeField]
        private int _authorIDIncrement = NO_INIT_ID + 1;
        [SerializeField]
        private int _typeIDIncrement = NO_INIT_ID + 1;

        private Dictionary<int, AuthorInfo> _authorsDict = new Dictionary<int, AuthorInfo>();
        private Dictionary<int, NoteTypeInfo> _typesDict = new Dictionary<int, NoteTypeInfo>();

        public IEnumerable<AuthorInfo> Authors => _authorsDict.Values;
        public IEnumerable<NoteTypeInfo> Types => _typesDict.Values;
        public int AuthorsCount => _authorsDict.Count;
        public int TypesCount => _typesDict.Count;

        [SerializeField, HideInInspector]
        private int _anonymousAuthorInfoID;
        [SerializeField, HideInInspector]
        private int _stickerAuthorInfoID;



        public AuthorInfo NewAuthorInfo()
        {
            var result = new AuthorInfo(_authorIDIncrement);
            _authorsSerialization.Add(result);
            _authorsDict.Add(_authorIDIncrement++, result);
            return result;
        }
        public bool TryGetAuthorInfo(int id, out AuthorInfo info)
        {
            return _authorsDict.TryGetValue(id, out info);
        }
        public AuthorInfo GetAuthorInfoOrDummy(int id)
        {
            if (TryGetAuthorInfo(id, out var result))
                return result;
            return DUMMY_AUTHOR;
        }

        public NoteTypeInfo NewTypeInfo()
        {
            var result = new NoteTypeInfo(_typeIDIncrement);
            _typesSerialization.Add(result);
            _typesDict.Add(_typeIDIncrement++, result);
            return result;
        }
        public bool TryGetTypeInfo(int id, out NoteTypeInfo info)
        {
            return _typesDict.TryGetValue(id, out info);
        }
        public NoteTypeInfo GetNoteTypeInfoOrDummy(int id)
        {
            if (TryGetTypeInfo(id, out var result))
                return result;
            return DUMMY_NOTE_TYPE;
        }

        public void Save()
        {
            foreach (var item in _authorsSerialization)
            {
                if (item._id == NO_INIT_ID) item._id = _authorIDIncrement++;
                item.color.a = 255;
            }
            foreach (var item in _typesSerialization)
            {
                if (item._id == NO_INIT_ID) item._id = _typeIDIncrement++;
                item.color.a = 255;
            }
            Save(false);
        }
        //public void SetNewAuthors(IEnumerable<AuthorInfo> authors)
        //{
        //    _authorsSerialization = authors.ToArray();
        //}
        //public void SetNewNoteTypes(IEnumerable<NoteTypeInfo> types)
        //{
        //    _typesSerialization = types.ToArray();
        //}

        #region ISerializationCallbackReceiver
        [SerializeField]
        private List<AuthorInfo> _authorsSerialization = new List<AuthorInfo>();
        [SerializeField]
        private List<NoteTypeInfo> _typesSerialization = new List<NoteTypeInfo>();
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            foreach (var item in _authorsSerialization)
            {
                if (item._id == NO_INIT_ID) item._id = _authorIDIncrement++;
                item.color.a = 255;
            }
            foreach (var item in _typesSerialization)
            {
                if (item._id == NO_INIT_ID) item._id = _typeIDIncrement++;
                item.color.a = 255;
            }
            _authorsDict = _authorsSerialization.ToDictionary(o => o._id);
            _typesDict = _typesSerialization.ToDictionary(o => o._id);
        }
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
        #endregion
    }
    public static class NotesSettingsExtensions
    {
        public static bool IsDummy(this AuthorInfo self) => self == DUMMY_AUTHOR;
        public static bool IsDummy(this NoteTypeInfo self) => self == DUMMY_NOTE_TYPE;
    }
}
#endif