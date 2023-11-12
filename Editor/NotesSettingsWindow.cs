#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    internal class NotesSettingsWindow : EditorWindow
    {
        //[MenuItem("Window/" + AUTHOR + "/" + ASSET_SHORT_NAME + "/Settings")]
        internal static void Open()
        {
            NotesSettingsWindow window = GetWindow<NotesSettingsWindow>();
            window.titleContent = new GUIContent("Notes settings");
            window.Show();
        }

        private NotesSettings Settigns => NotesSettings.Instance;

        private SerializedObject target;

        private Vector2 scrollViewPos;

        private void OnGUI()
        {
            if (target == null)
            {
                Settigns.hideFlags = ~HideFlags.HideAndDontSave;
                target = new SerializedObject(Settigns);
            }

            GUILayout.BeginScrollView(scrollViewPos);

            SerializedProperty authorsProp = target.FindProperty("_authorsSerialization");
            SerializedProperty typesProp = target.FindProperty("_typesSerialization");
            int oldAuthorsCount = authorsProp.arraySize;
            int oldTypesCount = typesProp.arraySize;
            GUI.enabled = true;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(authorsProp, new GUIContent("Authors"));
            EditorGUILayout.PropertyField(typesProp, new GUIContent("Types"));
            if (EditorGUI.EndChangeCheck())
            {
                if (authorsProp.arraySize != oldAuthorsCount)
                {
                    for (int i = oldAuthorsCount; i < authorsProp.arraySize; i++)
                        authorsProp.GetArrayElementAtIndex(i).FindPropertyRelative("_id").intValue = 0;
                }
                if (typesProp.arraySize != oldTypesCount)
                {
                    for (int i = oldTypesCount; i < typesProp.arraySize; i++)
                        typesProp.GetArrayElementAtIndex(i).FindPropertyRelative("_id").intValue = 0;
                }
                target.ApplyModifiedProperties();
                EditorUtility.SetDirty(Settigns);
                Settigns.Save();
            }

            GUILayout.EndScrollView();
        }
    }
}
#endif