#pragma warning disable CS0414
using System;
using UnityEditor;
using UnityEngine;

namespace DCFApixels
{
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

        private void OnDrawGizmos()
        {
            if (!_drawIcon) return;
            Gizmos.DrawIcon(transform.position, "Note Icon.png", false, _color);
        }
#endif
    }

#if UNITY_EDITOR
    namespace Editors
    {
        [CustomEditor(typeof(Note))]
        public class NoteDraw : Editor
        {
            private Rect rect = new Rect();
            private Texture2D _lineTex;
            private bool _IsInit = false;

            private void Init()
            {
                if (_IsInit) return;
                _lineTex = CreateTexture(2, 2, Color.black);
                _IsInit = true;
            }

            public override void OnInspectorGUI()
            {
                Color defaultColor = GUI.color;
                EditorGUI.BeginChangeCheck();
                SerializedProperty heightProp = serializedObject.FindProperty("_height");
                SerializedProperty noteProp = serializedObject.FindProperty("_note");
                SerializedProperty colorProp = serializedObject.FindProperty("_color");
                SerializedProperty drawIconProp = serializedObject.FindProperty("_drawIcon");

                Color defaultcolor = GUI.backgroundColor;
                Color.RGBToHSV(colorProp.colorValue, out float H, out float S, out float V);
                S -= S * 0.62f;
                Color elemcolor = Color.HSVToRGB(H, S, V) * 3f;
                rect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2 + heightProp.floatValue + 5);

                EditorGUI.DrawRect(rect, colorProp.colorValue);

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
                heightProp.floatValue = Mathf.Max(20f, heightProp.floatValue);
                GUI.color = defaultColor;
                EditorGUIUtility.labelWidth = originalValue;

                Color newColor = EditorGUILayout.ColorField(colorProp.colorValue, GUILayout.MaxWidth(40));
                newColor.a = 1f;
                colorProp.colorValue = newColor;

                EditorGUILayout.EndHorizontal();

                GUILayout.Box(_lineTex, GUILayout.Height(1), GUILayout.ExpandWidth(true));

                noteProp.stringValue = EditorGUILayout.TextArea(noteProp.stringValue, areastyle, GUILayout.Height(heightProp.floatValue));
                GUI.backgroundColor = defaultcolor;

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }


            public override void DrawPreview(Rect previewArea)
            {
                rect = previewArea;
                base.DrawPreview(previewArea);
            }

            private static GUIStyle CreateStyle(Color32 color32)
            {
                GUIStyle result = new GUIStyle(GUI.skin.box);
                Color componentColor = color32;
                result.normal.background = CreateTexture(2, 2, componentColor);
                return result;
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
}