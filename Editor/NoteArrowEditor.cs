#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Notes.Editors
{
    [CustomEditor(typeof(NoteArrow))]
    [InitializeOnLoad]
    public class NoteArrowEditor : Editor
    {
        private const float arrowHeight = 0.0085f;
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
        static void DrawGizmo(NoteArrow obj, GizmoType type)
        {
            if (obj.Target == null)
                return;

            if(!_arrows.Contains(obj))
                _arrows.Add(obj);
        }
        private static HashSet<NoteArrow> _arrows = new HashSet<NoteArrow>();
        private static HashSet<NoteArrow> _removedArrows = new HashSet<NoteArrow>();
        static NoteArrowEditor()
        {
            SceneView.duringSceneGui += SceneView_duringSceneGui;
        }
        private static void SceneView_duringSceneGui(SceneView scene)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Camera camera = scene.camera;
                bool isOrthographic = camera.orthographic;
                _removedArrows.Clear();
                foreach (var item in _arrows)
                {
                    if (item == null || !item.gameObject.activeInHierarchy)
                        _removedArrows.Add(item);
                }
                _arrows.SymmetricExceptWith(_removedArrows);
                foreach (var obj in _arrows)
                {
                    if (obj.Target == null)
                        return;

                    Color color = Color.white;
                    if (obj.TryGetComponent(out INote inote))
                        color = inote.Color;

                    Vector3 startPoint = obj.transform.position;
                    Vector3 endPoint = obj.Target.position;

                    Color defaultColor = Handles.color;

                    Handles.color = color;

                    Vector3 toCameraDirection;
                    if (isOrthographic)
                    {
                        Plane plane = new Plane(Vector3.up, startPoint);
                        plane.SetNormalAndPosition(camera.transform.forward, camera.transform.position);
                        var cp = plane.ClosestPointOnPlane(startPoint);
                        float distacne = Vector3.Distance(cp, startPoint);
                        toCameraDirection = -camera.transform.forward * distacne * 2f;
                    }
                    else
                    {
                        toCameraDirection = camera.transform.position - startPoint;
                    }

                    float height = arrowHeight * toCameraDirection.magnitude;

                    float startOffset = 0.02f * toCameraDirection.magnitude;
                    float endOffset = 0.2f;

                    Vector3 direction = endPoint - startPoint;
                    Quaternion q = Quaternion.LookRotation(toCameraDirection, direction);

                    endPoint -= direction.normalized * endOffset;
                    startPoint = startPoint + q * (Vector3.up * startOffset);

                    Vector3 startPoint1 = startPoint + q * (Vector3.left * height / 2f);
                    Vector3 startPoint2 = startPoint + q * (Vector3.right * height / 2f);

                    Handles.DrawLine(startPoint, endPoint);
                    Handles.DrawLine(startPoint1, endPoint);
                    Handles.DrawLine(startPoint2, endPoint);

                    Handles.color = defaultColor;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            var targetProp = serializedObject.FindProperty("_target");

            NoteArrow target = this.target as NoteArrow;
            Color color = Color.white;
            if (target.TryGetComponent(out INote inote))
                color = inote.Color;

            Color defaultBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            Rect rect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2 + 9);
            EditorGUI.DrawRect(rect, color);

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetProp);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            GUI.backgroundColor = defaultBackgroundColor;
        }
    }
}
#endif