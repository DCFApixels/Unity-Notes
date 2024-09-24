#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    [CustomEditor(typeof(LazyNote))]
    internal class LazyNoteEditor : ExtendedEditor<LazyNote>
    {
        private Rect _fullRect = new Rect();
        private Texture2D _lineTex;

        private SerializedProperty _heightProp;
        private SerializedProperty _textProp;
        private SerializedProperty _drawIconProp;
        private SerializedProperty _colorProp;

        private static GUIStyle _textAreaStyle;

        #region Init
        protected override bool IsStaticInit => _textAreaStyle != null;
        protected override bool IsInit => _textProp != null;
        protected override void OnStaticInit()
        {
            _textAreaStyle = new GUIStyle(EditorStyles.wordWrappedLabel);

            _textAreaStyle.fontSize = 14;
            _textAreaStyle.normal.textColor = Color.black;
            _textAreaStyle.hover = _textAreaStyle.normal;
            _textAreaStyle.focused = _textAreaStyle.normal;
            _textAreaStyle.richText = true;
        }
        protected override void OnInit()
        {
            _lineTex = CreateTexture(2, 2, Color.black);

            _heightProp = FindProperty("_height");
            _textProp = FindProperty("_text");
            _drawIconProp = FindProperty("_drawIcon");
            _colorProp = FindProperty("_color");
        }
        private static Texture2D CreateTexture(int width, int height, Color32 color32)
        {
            var pixels = new Color32[width * height];
            for (var i = 0; i < pixels.Length; ++i)
                pixels[i] = color32;

            var result = new Texture2D(width, height);
            result.SetPixels32(pixels);
            result.Apply();
            return result;
        }
        #endregion

        #region Draw
        protected override void DrawCustom()
        {
            Color defaultColor = GUI.color;
            Color defaultBackgroundColor = GUI.backgroundColor;

            Color color = _colorProp.colorValue;

            Color elemcolor = NormalizeBackgroundColor(color);
            _fullRect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2 + _heightProp.floatValue + 5);

            EditorGUI.DrawRect(_fullRect, color);

            GUI.backgroundColor = elemcolor;


            EditorGUILayout.BeginHorizontal();

            _drawIconProp.boolValue = EditorGUILayout.Toggle(_drawIconProp.boolValue, GUILayout.MaxWidth(16));
            GUILayout.Label("");

            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 14;
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
            GUI.backgroundColor = Color.white;

            GUIStyle gUIStylex = new GUIStyle(EditorStyles.helpBox);
            _heightProp.floatValue = EditorGUILayout.FloatField("↕", _heightProp.floatValue, gUIStylex, GUILayout.MaxWidth(58));
            _heightProp.floatValue = Mathf.Max(MIN_NOTE_HEIGHT, _heightProp.floatValue);
            GUI.color = defaultColor;
            EditorGUIUtility.labelWidth = originalValue;

            Color newColor = EditorGUILayout.ColorField(_colorProp.colorValue, GUILayout.MaxWidth(40));
            newColor.a = 1f;
            _colorProp.colorValue = newColor;

            EditorGUILayout.EndHorizontal();

            GUILayout.Box(_lineTex, GUILayout.Height(1), GUILayout.ExpandWidth(true));

            _textProp.stringValue = EditorGUILayout.TextArea(_textProp.stringValue, _textAreaStyle, GUILayout.Height(_heightProp.floatValue));
            GUI.backgroundColor = defaultBackgroundColor;
        }

        public override void DrawPreview(Rect previewArea)
        {
            _fullRect = previewArea;
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