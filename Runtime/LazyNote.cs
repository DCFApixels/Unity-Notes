#pragma warning disable CS0414
using UnityEngine;

namespace DCFApixels.Notes
{
    using static NotesConsts;
    [AddComponentMenu(ASSET_SHORT_NAME + "/" + nameof(LazyNote), 30)]
    internal class LazyNote : MonoBehaviour, INote
    {
#if UNITY_EDITOR
        [SerializeField]
        private string _text = "Enter text...";
        [SerializeField]
        private float _height = DEFAULT_NOTE_HEIGHT;
        [SerializeField]
        private Color _color = STICKER_COLOR;
        [SerializeField]
        private bool _drawIcon = true;
#endif

        #region Readonly properties
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
        public Color Color
        {
            get
            {
#if UNITY_EDITOR
                return _color;
#else
                return default;
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