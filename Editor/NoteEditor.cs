using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    public static class NoteUtils
    {
        [MenuItem("GameObject/Create Note")]
        public static void CreateNote(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Note");
            go.AddComponent<Note>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        public static void DrawNote(Note note, GizmoType gizmoType)
        {
            if (!note.DrawIcon) return;

            var assembly = Assembly.GetExecutingAssembly();
            var packagePath = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly).assetPath;
            Gizmos.DrawIcon(note.transform.position, packagePath + "/Gizmos/Runtime/Note Icon.png", false, note.Color);
        }
    }
    [CustomEditor(typeof(Note))]
    public class NoteEditor : Editor
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
            Init();
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