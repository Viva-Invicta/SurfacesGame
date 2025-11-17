using UnityEditor;
using UnityEngine;

namespace SurfacesGame
{
    [CustomEditor(typeof(Platform))]
    public class PlatformEditor : Editor
    {
        private void OnSceneGUI()
        {
            var platform = (Platform)target;

            if (platform.Vertices == null || platform.Vertices.Length == 0)
            {
                return;
            }

            Undo.RecordObject(platform, "Move Platform Vertex");

            var vertices = platform.Vertices;

            for (var i = 0; i < vertices.Length; i++)
            {
                var worldPos = platform.transform.TransformPoint(vertices[i]);

                var newWorldPos = Handles.FreeMoveHandle(
                    worldPos,
                    0.1f,
                    Vector3.zero,
                    Handles.SphereHandleCap
                );

                if (newWorldPos != worldPos)
                {
                    vertices[i] = platform.transform.InverseTransformPoint(newWorldPos);
                    EditorUtility.SetDirty(platform);
                }
            }

            Handles.color = Color.yellow;
            for (var i = 0; i < vertices.Length; i++)
            {
                var a = platform.transform.TransformPoint(vertices[i]);
                var b = platform.transform.TransformPoint(vertices[(i + 1) % vertices.Length]);
                Handles.DrawLine(a, b);
            }
        }
    }
}