using UnityEngine;

namespace SurfacesGame
{
    public class Platform : MonoBehaviour
    {
        [SerializeField]
        private Vector2[] vertices;

        private PlatformSide[] sides;

        public PlatformSide[] Sides => sides;
        public Vector2[] Vertices => vertices;

        public void Initialize()
        {
            if (vertices == null || vertices.Length < 3)
            {
                Debug.LogError($"{nameof(Platform)} needs at least 3 vertices to create a platform");
                return;
            }

            CreateSides();
        }

        private void CreateSides()
        {
            var count = vertices.Length;
            sides = new PlatformSide[count];

            for (var i = 0; i < count; i++)
            {
                var (start, end, direction, normal, prevIndex, nextIndex) = GetSideData(i);

                var data = new SurfaceData
                {
                    Start = start,
                    End = end,
                    Direction = direction,
                    Normal = normal,
                    Length = Vector2.Distance(start, end)
                };

                var side = new PlatformSide(data);
                side.SetIndices(prevIndex, nextIndex);

                sides[i] = side;
            }
        }

        private (Vector2 start, Vector2 end, Vector2 direction, Vector2 normal, int prevIndex, int nextIndex) GetSideData(int index)
        {
            var count = vertices.Length;
            var current = vertices[index];
            var nextIndex = (index + 1) % count;
            var prevIndex = (index - 1 + count) % count;
            var next = vertices[nextIndex];

            var direction = (next - current).normalized;
            var normal = new Vector2(-direction.y, direction.x); // counter-clockwise normal

            return (current, next, direction, normal, prevIndex, nextIndex);
        }

        private void OnDrawGizmos()
        {
            if (vertices == null || vertices.Length < 2)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            for (var i = 0; i < vertices.Length; i++)
            {
                var (start, end, _, _, _, _) = GetSideData(i);
                Gizmos.DrawSphere(start, 0.1f);
                Gizmos.DrawLine(start, end);
            }
        }
    }

    [System.Serializable]
    public struct PlatformSide
    {
        public SurfaceData Data;

        public int NextIndex;
        public int PrevIndex;

        public PlatformSide(SurfaceData data)
        {
            Data = data;
            NextIndex = -1;
            PrevIndex = -1;
        }

        public void SetIndices(int prevIndex, int nextIndex)
        {
            PrevIndex = prevIndex;
            NextIndex = nextIndex;
        }
    }

    public struct SurfaceData
    {
        public Vector2 Start;
        public Vector2 End;
        public Vector2 Direction;
        public Vector2 Normal;
        public float Length;
    }
}