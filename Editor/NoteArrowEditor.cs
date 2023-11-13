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
        private struct Segment
        {
            public float halfHeight;
            public float lerpT;
            public Segment(float halfHeight, float lerpT)
            {
                this.halfHeight = halfHeight;
                this.lerpT = lerpT;
            }
        }
        private static float _heightMultiplier = 0.0215f;
        private static Segment[] _segments = new Segment[] { 
            new Segment(0.00f / 2f, 0.00f * 0.99f),
            new Segment(0.66f / 2f, 0.01f * 0.97f),
            new Segment(1.00f / 2f, 0.04f * 0.94f),
            new Segment(0.55f / 2f, 0.20f * 0.90f),
            new Segment(0.15f / 2f, 0.55f * 0.90f),
            new Segment(0.00f / 2f, 1.00f * 0.85f),
            //new Segment(0.00f / 2f, 1.00f),
        };

        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
        static void DrawGizmo(NoteArrow obj, GizmoType type)
        {
            if (obj.Target == null)
                return;

            if(!_arrows.Contains(obj))
                _arrows.Add(obj);
        }
        private static HashSet<NoteArrow> _arrows = new HashSet<NoteArrow>();
        private static List<NoteArrow> _removedArrows = new List<NoteArrow>();
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

                    float startOffset = 0.02f * toCameraDirection.magnitude;

                    Vector3 direction = endPoint - startPoint;
                    Quaternion rotation = Quaternion.LookRotation(toCameraDirection, direction);
                    startPoint = startPoint + rotation * (Vector3.up * startOffset);
                    direction = endPoint - startPoint;

                    //TODO заменить отрисовку иконок с Gizmos.DrawIcon на GUI.DrawTexture, для большей гибкости
                    //Handles.BeginGUI();
                    //GUI.DrawTexture(new Rect(0, 0, 100, 100), tex, ScaleMode.StretchToFill, true, 10.0F);
                    //Handles.EndGUI();

                    Handles.DrawLine(startPoint, endPoint);
                    Vector3 startPoint1 = Vector3.zero;
                    Vector3 startPoint2 = Vector3.zero;
                    for (int i = 0; i < _segments.Length; i++)
                    {
                        Segment segment = _segments[i];
                        Vector3 lerpPoint = startPoint + direction.normalized * direction.magnitude * segment.lerpT;
                        float height = segment.halfHeight * toCameraDirection.magnitude / 2f * _heightMultiplier;
                        Vector3 endPoint1 = lerpPoint + rotation * (Vector3.left * height);
                        Vector3 endPoint2 = lerpPoint + rotation * (Vector3.right * height);
                        if (i > 0)
                        {
                            Handles.DrawLine(startPoint1, endPoint1);
                            Handles.DrawLine(startPoint2, endPoint2);
                        }
                        startPoint1 = endPoint1;
                        startPoint2 = endPoint2;
                    }
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

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetProp);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            GUI.backgroundColor = defaultBackgroundColor;
        }
    }
}
#endif