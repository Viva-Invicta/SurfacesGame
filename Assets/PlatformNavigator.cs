using UnityEngine;

namespace SurfacesGame
{
    public class PlatformNavigator
    {
        private readonly Platform platform;
        private readonly Vector2 ownerSize;

        public PlatformSide CurrentSide { get; private set; }
        public float SideProgress { get; private set; }



        public Vector2 ActiveSideNormal => CurrentSide.Normal;
        public Vector2 ActiveSideDirection => CurrentSide.Direction;

        public PlatformNavigator(Platform platform, Vector2 ownerSize)
        {
            this.platform = platform;
            this.ownerSize = ownerSize;

            CurrentSide = platform.Sides[0];
            SideProgress = 0.5f;
        }

        public void UpdateNavigation(Vector2 ownerPosition)
        {
            SideProgress = CalculateSideProgress(CurrentSide, ownerPosition);
            ChangeSideIfNecessary(ownerPosition);
        }

        public Vector2 GetSnapPosition(float addedVerticalDistance)
        {
            var position = Vector2.Lerp(CurrentSide.Start, CurrentSide.End, SideProgress);
            return position + CurrentSide.Normal * (ownerSize.y * 0.5f + addedVerticalDistance);
        }

        public float DistanceToSide(Vector2 ownerPosition)
        {
            var invertedNormal = -CurrentSide.Normal;

            var pointOnSide = Vector2.Lerp(CurrentSide.Start, CurrentSide.End, SideProgress);
            var ownerBottom = ownerPosition + invertedNormal * (ownerSize.y * 0.5f);

            var ownerToPoint = ownerBottom - pointOnSide;

            return Vector2.Dot(ownerToPoint, invertedNormal);
        }

        //[0..1]
        private float CalculateSideProgress(PlatformSide side, Vector2 ownerPosition)
        {
            var toPlayer = ownerPosition - side.Start;
            var projection = Vector2.Dot(toPlayer, side.Direction);
            var length = Vector2.Distance(side.Start, side.End);

            return projection / length;
        }

        private void ChangeSideIfNecessary(Vector2 ownerPosition)
        {
            var bestSide = CurrentSide;
            var bestProgress = SideProgress;
            var bestDistance = DistanceToOwner(bestSide, bestProgress, ownerPosition);

            int[] neighbors = { CurrentSide.NextIndex, CurrentSide.PrevIndex };

            foreach (var i in neighbors)
            {
                var side = platform.Sides[i];
                var progress = CalculateSideProgress(side, ownerPosition);

                if (progress < 0f || progress > 1f)
                {
                    continue;
                }

                var dist = DistanceToOwner(side, progress, ownerPosition);

                if (dist < bestDistance)
                {
                    bestSide = side;
                    bestProgress = progress;
                    bestDistance = dist;
                }
            }

            CurrentSide = bestSide;
            SideProgress = bestProgress;
        }

        private float DistanceToOwner(PlatformSide side, float progress, Vector2 ownerPosition)
        {
            var p = Vector2.Lerp(side.Start, side.End, Mathf.Clamp01(progress));
            return Vector2.Distance(p, ownerPosition);
        }
    }
}
