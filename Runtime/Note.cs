#pragma warning disable CS0414
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

namespace DCFApixels.Notes
{
    [AddComponentMenu("Note", 30)]
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

            var assembly = Assembly.GetExecutingAssembly();
            var packagePath = PackageInfo.FindForAssembly(assembly).assetPath;
            Gizmos.DrawIcon(transform.position, packagePath + "/Runtime/Note Icon.png", false, _color);
        }
#endif
    }
}