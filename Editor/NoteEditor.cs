#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;

    [CustomEditor(typeof(Note))]
    [CanEditMultipleObjects]
    internal class NoteEditor : ExtendedEditor<Note>
    {
        private const float HEADER_HEIGHT = 27;

        private Rect fullRect = new Rect();
        private Texture2D _lineTex;

        private GUIStyle _settingsButtonStyle;

        private GenericMenu _authorsGenericMenu;
        private int _authorsGenericMenuCount;
        private GenericMenu _typesGenericMenu;
        private int _typesGenericMenuCount;

        private SerializedProperty _heightProp;
        private SerializedProperty _textProp;
        private SerializedProperty _drawIconProp;
        private SerializedProperty _authorProp;
        private SerializedProperty _typeProp;

        #region Init
        protected override void OnInit()
        {
            _lineTex = CreateTexture(2, 2, Color.black);

            _settingsButtonStyle = new GUIStyle(EditorStyles.miniButton);
            _settingsButtonStyle.padding = new RectOffset(0, 0, 0, 0);

            _heightProp = FindProperty("_height");
            _textProp = FindProperty("_text");
            _drawIconProp = FindProperty("_drawIcon");
            _authorProp = FindProperty("_authorID");
            _typeProp = FindProperty("_typeID");
        }
        private static Texture2D CreateTexture(int width, int height, Color32 color32)
        {
            var pixels = new Color32[width * height];
            for (var i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = color32;
            }

            var result = new Texture2D(width, height);
            result.SetPixels32(pixels);
            result.Apply();
            return result;
        }
        public GenericMenu GetAuthorsGenericMenu()
        {
            if (_authorsGenericMenu == null || _authorsGenericMenuCount != Settings.AuthorsCount)
            {
                _authorsGenericMenuCount = Settings.AuthorsCount;
                _authorsGenericMenu = new GenericMenu();
                foreach (var author in Settings.Authors)
                {
                    _authorsGenericMenu.AddItem(new GUIContent(author.name), false, OnAuthorSelected, author);
                }
            }
            return _authorsGenericMenu;
        }
        private void OnAuthorSelected(object obj)
        {
            AuthorInfo author = (AuthorInfo)obj;
            serializedObject.FindProperty("_authorID").intValue = author._id;
            serializedObject.ApplyModifiedProperties();
            foreach (Note note in Targets)
            {
                note.UpdateRefs();
            }
        }
        public GenericMenu GetTypesGenericMenu()
        {
            if (_typesGenericMenu == null || _typesGenericMenuCount != Settings.TypesCount)
            {
                _typesGenericMenuCount = Settings.TypesCount;
                _typesGenericMenu = new GenericMenu();
                foreach (var type in Settings.Types)
                {
                    _typesGenericMenu.AddItem(new GUIContent(type.name), false, OnTypeSelected, type);
                }
            }
            return _typesGenericMenu;
        }
        private void OnTypeSelected(object obj)
        {
            NoteTypeInfo type = (NoteTypeInfo)obj;
            serializedObject.FindProperty("_typeID").intValue = type._id;
            serializedObject.ApplyModifiedProperties();
            foreach (Note note in Targets)
            {
                note.UpdateRefs();
            }
        }
        #endregion

        #region Draw
        protected override void DrawCustom()
        {
            Color defaultColor = GUI.color;
            Color defaultBackgroundColor = GUI.backgroundColor;

            AuthorInfo author = Settings.GetAuthorInfoOrDummy(_authorProp.hasMultipleDifferentValues ? 0 : _authorProp.intValue);
            NoteTypeInfo noteType = Settings.GetNoteTypeInfoOrDummy(_typeProp.hasMultipleDifferentValues ? 0 : _typeProp.intValue);
            Color headerColor = author.color;
            Color bodyColor = noteType.color;


            Color headerBackColor = NormalizeBackgroundColor(headerColor);

            fullRect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2 + _heightProp.floatValue + 5);
            Rect headerRect = fullRect;
            headerRect.height = HEADER_HEIGHT;
            Rect bodyRect = fullRect;
            bodyRect.yMin += HEADER_HEIGHT;

            EditorGUI.DrawRect(headerRect, headerColor);
            EditorGUI.DrawRect(bodyRect, bodyColor);

            GUI.backgroundColor = headerBackColor;

            GUIStyle areastyle = new GUIStyle(EditorStyles.wordWrappedLabel);

            areastyle.fontSize = 14;
            areastyle.normal.textColor = Color.black;
            areastyle.hover = areastyle.normal;
            areastyle.focused = areastyle.normal;

            EditorGUILayout.BeginHorizontal();
            GUIStyle gUIStyle = new GUIStyle(EditorStyles.label);
            gUIStyle.normal.textColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);

            _drawIconProp.boolValue = EditorGUILayout.Toggle(_drawIconProp.boolValue, GUILayout.MaxWidth(16));


            GUI.backgroundColor = Color.white;
            GUI.color = Color.black;
            GUILayout.Label("Author:", GUILayout.Width(44));
            GUI.color = headerColor;
            if (GUILayout.Button(author.IsDummy() ? "-" : author.name, EditorStyles.popup))
            {
                GetAuthorsGenericMenu().ShowAsContext();
            }
            GUI.color = Color.black;
            GUILayout.Label("Type:", GUILayout.Width(36));
            GUI.color = headerColor;
            if (GUILayout.Button(noteType.IsDummy() ? "-" : noteType.name, EditorStyles.popup))
            {
                GetTypesGenericMenu().ShowAsContext();
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Popup"), _settingsButtonStyle, GUILayout.Width(18f)))
            {
                NotesSettingsWindow.Open();
            }
            GUILayout.Label("", gUIStyle);

            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 14;
            GUI.color = new Color(0.2f, 0.2f, 0.2f);

            GUIStyle gUIStylex = new GUIStyle(EditorStyles.helpBox);
            EditorGUI.BeginChangeCheck();
            float newHeight = EditorGUILayout.FloatField("↕", _heightProp.hasMultipleDifferentValues ? DEFAULT_NOTE_HEIGHT : _heightProp.floatValue, gUIStylex, GUILayout.MaxWidth(58));
            if (EditorGUI.EndChangeCheck())
            {
                _heightProp.floatValue = Mathf.Max(newHeight, MIN_NOTE_HEIGHT);
            }
            EditorGUIUtility.labelWidth = originalValue;


            GUI.color = defaultColor;

            //Color newColor = EditorGUILayout.ColorField(colorProp.colorValue, GUILayout.MaxWidth(40));
            //newColor.a = 1f;
            //colorProp.colorValue = newColor;

            EditorGUILayout.EndHorizontal();

            GUILayout.Box(_lineTex, GUILayout.Height(1), GUILayout.ExpandWidth(true));

            EditorGUI.BeginChangeCheck();
            string newValue = EditorGUILayout.TextArea(_textProp.hasMultipleDifferentValues ? "-" : _textProp.stringValue, areastyle, GUILayout.Height(_heightProp.floatValue));
            if (EditorGUI.EndChangeCheck())
            {
                _textProp.stringValue = newValue;
            }

            GUI.backgroundColor = defaultBackgroundColor;

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNote(Note target, SerializedObject serializedObject)
        {

        }
        public override void DrawPreview(Rect previewArea)
        {
            fullRect = previewArea;
            base.DrawPreview(previewArea);
        }
        private static Color NormalizeBackgroundColor(Color color)
        {
            Color.RGBToHSV(color, out float H, out float S, out float V);
            S -= S * 0.62f;
            return Color.HSVToRGB(H, S, V) * 3f;
        }
        #endregion
    }
}
#endif