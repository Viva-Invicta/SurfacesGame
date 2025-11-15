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
                var side = new PlatformSide(start, end, direction, normal);
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
        public PlatformSide(Vector2 start, Vector2 end, Vector2 direction, Vector2 normal) : this()
        {
            this.start = start;
            this.end = end;
            this.direction = direction;
            this.normal = normal;

            length = Vector2.Distance(start, end);
        }

        private readonly Vector2 start;
        private readonly Vector2 end;
        private readonly Vector2 direction;
        private readonly Vector2 normal;
        private readonly float length;

        private int nextIndex;
        private int prevIndex;

        public Vector2 Start => start;
        public Vector2 End => end;
        public Vector2 Direction => direction;
        public Vector2 Normal => normal;
        public float Length => length;

        public int NextIndex => nextIndex;
        public int PrevIndex => prevIndex;

        public void SetIndices(int prevIndex, int nextIndex)
        {
            this.nextIndex = nextIndex;
            this.prevIndex = prevIndex;
        }
    }

}