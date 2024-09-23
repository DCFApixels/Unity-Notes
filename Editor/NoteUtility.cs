#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    internal static class NoteUtility
    {
        private static string _gizmosPath;
        private static string _noteIconPath;
        private static string _authorNoteIconPath;
        private static string _typeNoteIconPath;
        private static GUIStyle _textStyle;

        private static bool _isInit = false;


        private static void Init()
        {
            if (_isInit && _textStyle != null) { return; }
            _noteIconPath = GetGizmosPath() + "/Runtime/Note Icon.png";
            _authorNoteIconPath = GetGizmosPath() + "/Runtime/Note Author Icon.png";
            _typeNoteIconPath = GetGizmosPath() + "/Runtime/Note Type Icon.png";
            _textStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
            _textStyle.richText = true;
            _textStyle.alignment = TextAnchor.MiddleCenter;
            _textStyle.wordWrap = true;
            _isInit = true;
        }

        private static GUIContent _label;
        public static GUIContent GetLabel(string text, string tooltip)
        {
            if (_label == null)
            {
                _label = new GUIContent();
            }
            _label.text = text;
            _label.tooltip = tooltip;
            return _label;
        }
        public static GUIContent GetLabel(string text)
        {
            if (_label == null)
            {
                _label = new GUIContent();
            }
            _label.text = text;
            return _label;
        }

        #region CreateLazyNote
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(LazyNote) + " with arrow")]
        public static void CreateLazyNoteWithArrow(MenuCommand menuCommand)
        {
            CreateLazyNoteInternal(menuCommand, true);
        }
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(LazyNote))]
        public static void CreateLazyNote(MenuCommand menuCommand)
        {
            CreateLazyNoteInternal(menuCommand, false);
        }
        private static GameObject CreateLazyNoteInternal(MenuCommand menuCommand, bool isWithArrow)
        {
            GameObject go = new GameObject(nameof(LazyNote) + (isWithArrow ? " (Arrow)" : ""));
            go.tag = EDITOR_NAME_TAG;
            go.AddComponent<LazyNote>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            if (go.transform.parent == null)
            {
                go.transform.parent = FindRootTransform().transform;
            }
            Selection.activeObject = go;
            if (isWithArrow)
            {
                go.AddComponent<NoteArrow>();
            }
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            return go;
        }
        #endregion

        #region CreateNote
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(Note) + " with arrow")]
        public static void CreateNoteWithArrow(MenuCommand menuCommand)
        {
            CreateNoteInternal(menuCommand, true);
        }
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(Note))]
        public static void CreateNote(MenuCommand menuCommand)
        {
            CreateNoteInternal(menuCommand, false);
        }
        private static GameObject CreateNoteInternal(MenuCommand menuCommand, bool isWithArrow)
        {
            GameObject go = new GameObject(nameof(Note) + (isWithArrow ? " (Arrow)" : ""));
            go.tag = EDITOR_NAME_TAG;
            go.AddComponent<Note>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            if (go.transform.parent == null)
            {
                go.transform.parent = FindRootTransform().transform;
            }
            Selection.activeObject = go;
            if (isWithArrow)
            {
                go.AddComponent<NoteArrow>();
            }
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            return go;
        }
        #endregion

        #region Draw
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawLazyNote(LazyNote note, GizmoType gizmoType)
        {
            Init();

            string sceneNote = GetSceneNote(note.Text, note.DrawIcon);
            DrawWorldLabel(note.transform.position, GetLabel(sceneNote), _textStyle);

            if (note.DrawIcon)
            {
                Gizmos.DrawIcon(note.transform.position, _noteIconPath, false, note.Color);
            }
        }
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawNote(Note note, GizmoType gizmoType)
        {
            Init();

            string sceneNote = GetSceneNote(note.Text, note.DrawIcon);
            DrawWorldLabel(note.transform.position, GetLabel(sceneNote), _textStyle);

            if (note.DrawIcon)
            {
                Gizmos.DrawIcon(note.transform.position, _authorNoteIconPath, false, note.Author.color);
                Gizmos.DrawIcon(note.transform.position, _typeNoteIconPath, false, note.Type.color);
            }
        }
        private static void DrawWorldLabel(Vector3 position, GUIContent content, GUIStyle style)
        {

            if (!(HandleUtility.WorldToGUIPointWithDepth(position).z < 0f))
            {
                Handles.BeginGUI();
                Color dc = GUI.color;
                GUI.color = Color.black;
                GUI.Label(WorldPointToSizedRect(position, content, style), content, style);
                GUI.color = dc;
                Handles.EndGUI();
            }
        }
        public static Rect WorldPointToSizedRect(Vector3 position, GUIContent content, GUIStyle style)
        {
            Vector2 center = HandleUtility.WorldToGUIPointWithDepth(position);
            Vector2 size = style.CalcSize(content);
            Rect rect = new Rect(Vector2.zero, size);
            rect.center = center;
            return style.padding.Add(rect);
        }
        internal static string GetSceneNote(string fullNote, bool isNeedSpacing)
        {
            int index = fullNote.IndexOf(NOTE_SEPARATOR);
            if (index < 0) { return string.Empty; }
            string result = fullNote.Substring(0, index);
            //return isNeedSpacing ? "\r\n" + result : result;
            return result;
        }
        #endregion

        #region Utils
        private static Transform FindRootTransform(string name = NOTES_ROOT_NAME)
        {
            GameObject root = GameObject.Find(name);
            if (root == null)
            {
                root = new GameObject(name);
                root.tag = EDITOR_NAME_TAG;
                root.transform.position = Vector3.zero;
                root.transform.rotation = Quaternion.identity;
                root.transform.localScale = Vector3.one;
            }
            return root.transform;
        }
        internal static string GetGizmosPath()
        {
            if (string.IsNullOrEmpty(_gizmosPath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                string packagePath = null;
                if (assembly != null)
                {
                    packagePath = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly)?.assetPath;
                }
                if (string.IsNullOrEmpty(packagePath))
                {
                    var guids = AssetDatabase.FindAssets($"Notes-Unity t:AssemblyDefinitionAsset");
                    for (var i = 0; i < guids.Length; i++)
                    {
                        var guid = guids[i];
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var asmdef = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
                        if (asmdef != null && asmdef.name == "Notes-Unity")
                        {
                            packagePath = path.Substring(0, path.LastIndexOf("/"));
                            break;
                        }
                    }
                }
                _gizmosPath = packagePath + "/Gizmos";
            }
            return _gizmosPath;
        }
        #endregion
    }
}
#endif