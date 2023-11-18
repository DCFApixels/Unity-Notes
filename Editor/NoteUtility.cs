#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    internal static class NoteUtility
    {
        private static string _gizmosPath;
        private static GameObject FindRoot(string name)
        {
            GameObject root = GameObject.Find(name);
            if (root == null)
            {
                root = new GameObject(name);
                root.tag = EDITOR_NAME_TAG;
            }
            return root;
        }

        #region CreateLazyNote
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(LazyNote) + " with arrow")]
        public static void CreateLazyNoteWithArrow(MenuCommand menuCommand)
        {
            GameObject go = CreateLazyNoteInternal(menuCommand);
            go.AddComponent<NoteArrow>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(LazyNote))]
        public static void CreateLazyNote(MenuCommand menuCommand)
        {
            GameObject go = CreateLazyNoteInternal(menuCommand);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
        private static GameObject CreateLazyNoteInternal(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(LazyNote));
            go.tag = EDITOR_NAME_TAG;
            go.AddComponent<LazyNote>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            if (go.transform.parent == null)
                go.transform.parent = FindRoot(NOTES_ROOT_NAME).transform;
            Selection.activeObject = go;
            return go;
        }
        #endregion

        #region CreateNote
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(Note) + " with arrow")]
        public static void CreateNoteWithArrow(MenuCommand menuCommand)
        {
            GameObject go = CreateNoteInternal(menuCommand);
            go.AddComponent<NoteArrow>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
        [MenuItem("GameObject/" + ASSET_SHORT_NAME + "/Create " + nameof(Note))]
        public static void CreateNote(MenuCommand menuCommand)
        {
            GameObject go = CreateNoteInternal(menuCommand);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
        private static GameObject CreateNoteInternal(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(Note));
            go.tag = EDITOR_NAME_TAG;
            go.AddComponent<Note>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            if (go.transform.parent == null)
                go.transform.parent = FindRoot(NOTES_ROOT_NAME).transform;
            Selection.activeObject = go;
            return go;
        }
        #endregion

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawLazyNote(LazyNote note, GizmoType gizmoType)
        {
            if (note.DrawIcon)
            {
                Gizmos.DrawIcon(note.transform.position, GetGizmosPath() + "/Runtime/Note Icon.png", false, note.Color);
            }

            string sceneNote = GetSceneNote(note.Text, note.DrawIcon);
            Handles.Label(note.transform.position, sceneNote, EditorStyles.whiteBoldLabel);
        }
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawNote(Note note, GizmoType gizmoType)
        {
            if (note.DrawIcon)
            {
                Gizmos.DrawIcon(note.transform.position, GetGizmosPath() + "/Runtime/Note Author Icon.png", false, note.Author.color);
                Gizmos.DrawIcon(note.transform.position, GetGizmosPath() + "/Runtime/Note Type Icon.png", false, note.Type.color);
            }

            string sceneNote = GetSceneNote(note.Text, note.DrawIcon);
            Handles.Label(note.transform.position, sceneNote, EditorStyles.whiteBoldLabel);
        }

        internal static string GetGizmosPath()
        {
            if (string.IsNullOrEmpty(_gizmosPath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                string packagePath = null;
                if (assembly != null)
                    packagePath = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly)?.assetPath;
                if (string.IsNullOrEmpty(packagePath))
                    packagePath = "Assets";
                _gizmosPath = packagePath + "/Gizmos";

            }
            return _gizmosPath;
        }
        internal static string GetSceneNote(string fullNote, bool isNeedSpacing)
        {
            int index = fullNote.IndexOf(NOTE_SEPARATOR);
            if (index < 0) return string.Empty;
            string result = fullNote.Substring(0, index);
            return isNeedSpacing ? "\r\n" + result : result;
        }
    }
}
#endif