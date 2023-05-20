using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    using static NotesConsts;
    internal static class NoteUtility
    {
        private static string _gizmosPath;

        [MenuItem("GameObject/Create " + nameof(LazyNote))]
        public static void CreateLazyNote(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(LazyNote));
            go.AddComponent<LazyNote>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        [MenuItem("GameObject/Create " + nameof(Note))]
        public static void CreateNote(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(Note));
            go.AddComponent<Note>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

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
                if(string.IsNullOrEmpty(packagePath))
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
