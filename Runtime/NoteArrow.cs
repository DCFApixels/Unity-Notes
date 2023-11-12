using UnityEngine;

namespace DCFApixels.Notes
{
    using static NotesConsts;

    [AddComponentMenu(ASSET_SHORT_NAME + "/" + nameof(NoteArrow), 30)]
    public class NoteArrow : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private Transform _target;
#endif
        public Transform Target
        {
            get
            {
#if UNITY_EDITOR
                return _target;
#else
                return null;
#endif
            }
        }
    }
}
