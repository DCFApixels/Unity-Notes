using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes
{
    using static NotesConsts;
    internal class NotesSettings : Config<NotesSettings>, ISerializationCallbackReceiver
    {
        internal const int NO_INIT_ID = 0;

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

        private AuthorInfo _dummyAuthor = new AuthorInfo(0)
        {
            name = "Dummy",
            color = NEUTRAL_COLOR,
        };
        private NoteTypeInfo _dummyType = new NoteTypeInfo(0)
        {
            name = "Dummy",
            color = NEUTRAL_COLOR,
        };
        public AuthorInfo DummyAuthor => _dummyAuthor;
        public NoteTypeInfo DummyNoteType => _dummyType;

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
            return _dummyAuthor;
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
            return _dummyType;
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
                if (item._id == NO_INIT_ID) item._id = _authorIDIncrement++;
            foreach (var item in _typesSerialization)
                if (item._id == NO_INIT_ID) item._id = _typeIDIncrement++;
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
        public static bool IsDummy(this AuthorInfo self) => self == NotesSettings.Instance.DummyAuthor;
        public static bool IsDummy(this NoteTypeInfo self) => self == NotesSettings.Instance.DummyNoteType;
    }

    internal abstract class Config<TSelf> : ScriptableObject where TSelf : Config<TSelf>
    {
        private static object _lock = new object();
        private static TSelf _instance;
        public static TSelf Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        string path = typeof(TSelf).ToString();
                        _instance = Resources.Load<TSelf>(typeof(TSelf).Name);
                        if (_instance == null)
                        {
                            TSelf data = CreateInstance<TSelf>();
#if UNITY_EDITOR
                            if (AssetDatabase.IsValidFolder("Assets/Resources/") == false)
                            {
                                System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/");
                                AssetDatabase.Refresh();
                            }
                            AssetDatabase.CreateAsset(data, "Assets/Resources/" + typeof(TSelf).Name + ".asset");
                            AssetDatabase.Refresh();
#endif
                            _instance = data;
                        }
                    }
                    return _instance;
                }
            }
        }

        public static void Select()
        {
#if UNITY_EDITOR
            Selection.activeObject = _instance;
#endif
        }
    }
}