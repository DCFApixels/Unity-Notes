#pragma warning disable CS0414
using UnityEngine;

namespace DCFApixels.Notes
{
    [AddComponentMenu("Note", 30)]
    internal class Note : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private string _text = "Enter text...";
        [SerializeField]
        private float _height = 100f;
        [SerializeField]
        private Color _color = new Color(1, 0.8f, 0.3f, 1f);
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