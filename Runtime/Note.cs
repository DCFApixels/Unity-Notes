#pragma warning disable CS0414
using UnityEngine;

namespace DCFApixels.Notes
{
    [AddComponentMenu("Note", 30)]
    public class Note : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private float _height = 100f;
        [SerializeField]
        private string _note = "Enter text...";
        [SerializeField]
        private Color _color = new Color(1, 0.8f, 0.3f, 1f);
        [SerializeField]
        private bool _drawIcon = true;
#endif
        public bool DrawIcon
        {
            get
            {
#if UNITY_EDITOR
                return _drawIcon;
#else
                return false;
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
                return Color.clear;
#endif
            }
        }
    }
}