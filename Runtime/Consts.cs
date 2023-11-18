using UnityEngine;

namespace DCFApixels.Notes
{
    internal class NotesConsts
    {
        public const string EDITOR_NAME_TAG = "EditorOnly";
        public const string NOTES_ROOT_NAME = "NOTES (" + EDITOR_NAME_TAG + ")";

        public const string ASSET_SHORT_NAME = "Notes";
        public const string AUTHOR = "DCFApixels";
        public const string NOTE_SEPARATOR = ">-<";

        public const float DEFAULT_NOTE_HEIGHT = 100f;
        public const float MIN_NOTE_HEIGHT = 20f;

        public const string STICKER_NAME = "Sticker";
        public const string ANONYMOUS_NAME = "Anonymous";
        public static readonly Color STICKER_COLOR = new Color(1f, 0.8f, 0.3f, 1f);
        public static readonly Color NEUTRAL_COLOR = new Color(0.68f, 0.73f, 0.77f, 1f);

        public static readonly AuthorInfo DUMMY_AUTHOR = new AuthorInfo(0)
        {
            name = "Dummy",
            color = NEUTRAL_COLOR,
        };
        public static readonly NoteTypeInfo DUMMY_NOTE_TYPE = new NoteTypeInfo(0)
        {
            name = "Dummy",
            color = NEUTRAL_COLOR,
        };
    }
}
