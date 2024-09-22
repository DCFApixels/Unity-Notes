#if UNITY_EDITOR
using System.IO;
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
            GameObject go = new GameObject(nameof(LazyNote));
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
            GameObject go = new GameObject(nameof(Note));
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
        internal static string GetSceneNote(string fullNote, bool isNeedSpacing)
        {
            int index = fullNote.IndexOf(NOTE_SEPARATOR);
            if (index < 0) return string.Empty;
            string result = fullNote.Substring(0, index);
            return isNeedSpacing ? "\r\n" + result : result;
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