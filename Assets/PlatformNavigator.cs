using System;
using UnityEngine;

namespace SurfacesGame
{
    public class PlatformNavigator
    {
        public event Action SurfaceDataUpdated;

        private readonly Platform platform;
        private readonly Vector2 ownerSize;

        public PlatformSide CurrentSide { get; private set; }
        public float SideProgress { get; private set; }
        public SurfaceData SurfaceData { get; private set; }

        public Vector2 ActiveSideNormal => CurrentSide.Data.Normal;
        public Vector2 ActiveSideDirection => CurrentSide.Data.Direction;

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
            var position = Vector2.Lerp(CurrentSide.Data.Start, CurrentSide.Data.End, SideProgress);
            return position + CurrentSide.Data.Normal * (ownerSize.y * 0.5f + addedVerticalDistance);
        }

        public float DistanceToSide(Vector2 ownerPosition)
        {
            var invertedNormal = -CurrentSide.Data.Normal;

            var pointOnSide = Vector2.Lerp(CurrentSide.Data.Start, CurrentSide.Data.End, SideProgress);
            var ownerBottom = ownerPosition + invertedNormal * (ownerSize.y * 0.5f);

            var ownerToPoint = ownerBottom - pointOnSide;

            return Vector2.Dot(ownerToPoint, invertedNormal);
        }

        //[0..1]
        private float CalculateSideProgress(PlatformSide side, Vector2 ownerPosition)
        {
            var toPlayer = ownerPosition - side.Data.Start;
            var projection = Vector2.Dot(toPlayer, side.Data.Direction);
            var length = Vector2.Distance(side.Data.Start, side.Data.End);

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
            SurfaceData = CurrentSide.Data;

            SurfaceDataUpdated?.Invoke();
        }

        private float DistanceToOwner(PlatformSide side, float progress, Vector2 ownerPosition)
        {
            var p = Vector2.Lerp(side.Data.Start, side.Data.End, Mathf.Clamp01(progress));
            return Vector2.Distance(p, ownerPosition);
        }
    }
}
