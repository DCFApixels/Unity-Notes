#if UNITY_EDITOR
namespace DCFApixels.Notes.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Unity.Collections.LowLevel.Unsafe;
    using UnityEditor;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    internal abstract class ExtendedEditor : Editor
    {
        private bool _isStaticInit = false;
        private bool _isInit = false;

        protected bool IsMultipleTargets { get { return targets.Length > 1; } }
        protected float OneLineHeight { get => EditorGUIUtility.singleLineHeight; }
        protected float Spacing { get => EditorGUIUtility.standardVerticalSpacing; }
        protected virtual bool IsStaticInit { get { return _isStaticInit; } }
        protected virtual bool IsInit { get { return _isInit; } }
        protected NotesSettings Settings { get { return NotesSettings.Instance; } }

        protected void StaticInit()
        {
            if (IsStaticInit) { return; }
            _isStaticInit = true;
            OnStaticInit();
        }
        protected void Init()
        {
            if (IsInit) { return; }
            _isInit = true;
            OnInit();
        }
        protected virtual void OnStaticInit() { }
        protected virtual void OnInit() { }

        public sealed override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            StaticInit();
            Init();
            DrawCustom();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected abstract void DrawCustom();
        protected void DrawDefault()
        {
            base.OnInspectorGUI();
        }

        protected SerializedProperty FindProperty(string name)
        {
            return serializedObject.FindProperty(name);
        }
    }
    internal abstract class ExtendedEditor<T> : ExtendedEditor
    {
        public T Target
        {
            get
            {
                var obj = target;
                return UnsafeUtility.As<UnityObject, T>(ref obj);
            }
        }
        public T[] Targets
        {
            get
            {
                var obj = targets;
                return UnsafeUtility.As<UnityObject[], T[]>(ref obj);
            }
        }
    }
    internal abstract class ExtendedPropertyDrawer : PropertyDrawer
    {
        private bool _isStaticInit = false;
        private bool _isInit = false;

        private IEnumerable<Attribute> _attributes = null;

        protected IEnumerable<Attribute> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = fieldInfo.GetCustomAttributes();
                }
                return _attributes;
            }
        }
        protected float OneLineHeight { get => EditorGUIUtility.singleLineHeight; }
        protected float Spacing { get => EditorGUIUtility.standardVerticalSpacing; }
        protected virtual bool IsStaticInit { get { return _isStaticInit; } }
        protected virtual bool IsInit { get { return _isInit; } }
        protected NotesSettings Settings { get { return NotesSettings.Instance; } }


        protected void StaticInit()
        {
            if (IsStaticInit) { return; }
            _isStaticInit = true;
            OnStaticInit();
        }
        protected void Init()
        {
            if (IsInit) { return; }
            _isInit = true;
            OnInit();
        }
        protected virtual void OnStaticInit() { }
        protected virtual void OnInit() { }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            StaticInit();
            Init();
            DrawCustom(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        protected abstract void DrawCustom(Rect position, SerializedProperty property, GUIContent label);
    }
    internal abstract class ExtendedPropertyDrawer<TAttribute> : ExtendedPropertyDrawer
    {
        protected TAttribute Attribute
        {
            get
            {
                var obj = attribute;
                return UnsafeUtility.As<PropertyAttribute, TAttribute>(ref obj);
            }
        }
    }
}
#endif