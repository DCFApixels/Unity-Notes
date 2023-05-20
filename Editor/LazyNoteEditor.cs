#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    [CustomEditor(typeof(LazyNote))]
    internal class LazyNoteEditor : Editor
    {
        private Rect rect = new Rect();
        private Texture2D _lineTex;
        private bool _IsInit = false;

        private LazyNote Target => target as LazyNote;

        private void Init()
        {
            if (_IsInit) return;
            _lineTex = CreateTexture(2, 2, Color.black);
            _IsInit = true;
        }

        public override void OnInspectorGUI()
        {
            Init();
            Color defaultColor = GUI.color;
            Color defaultBackgroundColor = GUI.backgroundColor;

            EditorGUI.BeginChangeCheck();
            SerializedProperty heightProp = serializedObject.FindProperty("_height");
            SerializedProperty textProp = serializedObject.FindProperty("_text");
            SerializedProperty colorProp = serializedObject.FindProperty("_color");
            SerializedProperty drawIconProp = serializedObject.FindProperty("_drawIcon");

            Color color = colorProp.colorValue;

            Color elemcolor = NormalizeBackgroundColor(color);
            rect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2 + heightProp.floatValue + 5);

            EditorGUI.DrawRect(rect, color);

            GUI.backgroundColor = elemcolor;

            GUIStyle areastyle = new GUIStyle(EditorStyles.wordWrappedLabel);

            areastyle.fontSize = 14;
            areastyle.normal.textColor = Color.black;
            areastyle.hover = areastyle.normal;
            areastyle.focused = areastyle.normal;

            EditorGUILayout.BeginHorizontal();
            GUIStyle gUIStyle = new GUIStyle(EditorStyles.label);
            gUIStyle.normal.textColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);

            drawIconProp.boolValue = EditorGUILayout.Toggle(drawIconProp.boolValue, GUILayout.MaxWidth(16));
            GUILayout.Label("", gUIStyle);

            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 14;
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
            GUI.backgroundColor = Color.white;

            GUIStyle gUIStylex = new GUIStyle(EditorStyles.helpBox);
            heightProp.floatValue = EditorGUILayout.FloatField("↕", heightProp.floatValue, gUIStylex, GUILayout.MaxWidth(58));
            heightProp.floatValue = Mathf.Max(MIN_NOTE_HEIGHT, heightProp.floatValue);
            GUI.color = defaultColor;
            EditorGUIUtility.labelWidth = originalValue;

            Color newColor = EditorGUILayout.ColorField(colorProp.colorValue, GUILayout.MaxWidth(40));
            newColor.a = 1f;
            colorProp.colorValue = newColor;

            EditorGUILayout.EndHorizontal();

            GUILayout.Box(_lineTex, GUILayout.Height(1), GUILayout.ExpandWidth(true));

            textProp.stringValue = EditorGUILayout.TextArea(textProp.stringValue, areastyle, GUILayout.Height(heightProp.floatValue));
            GUI.backgroundColor = defaultBackgroundColor;

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
        public override void DrawPreview(Rect previewArea)
        {
            rect = previewArea;
            base.DrawPreview(previewArea);
        }
        private static Color NormalizeBackgroundColor(Color color)
        {
            Color.RGBToHSV(color, out float H, out float S, out float V);
            S -= S * 0.62f;
            return Color.HSVToRGB(H, S, V) * 3f;
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
    }
}
#endif