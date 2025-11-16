using System;
using UnityEngine;

namespace SurfacesGame
{
    public class PlatformNavigator
    {
        public event Action SurfaceDataUpdated;
        public event Action SurfaceProgressDataUpdated;

        private readonly Platform platform;
        private readonly Vector2 ownerSize;

        private PlatformSurface currentSurface;

        public SurfaceData SurfaceData { get; private set; }
        public SurfaceProgressData SurfaceProgressData { get; private set; }

        public PlatformNavigator(Platform platform, Vector2 ownerSize)
        {
            this.platform = platform;
            this.ownerSize = ownerSize;

            currentSurface = platform.Surfaces[0];

            SurfaceData = currentSurface.Data;
            SurfaceProgressData = new SurfaceProgressData { Progress = 0.5f };
        }

        public void UpdateNavigation(Vector2 ownerPosition)
        {
            ChangeActiveSurfaceIfNecessary(ownerPosition);
            RecalculateActiveSurfaceProgress(ownerPosition);
        }

        public Vector2 GetSnapPosition()
        {
            var position = Vector2.Lerp(currentSurface.Data.Start, currentSurface.Data.End, SurfaceProgressData.Progress);
            return position + currentSurface.Data.Normal * (ownerSize.y * 0.5f);
        }

        public float DistanceToSide(Vector2 ownerPosition)
        {
            var invertedNormal = -currentSurface.Data.Normal;
            var pointOnSide = Vector2.Lerp(currentSurface.Data.Start, currentSurface.Data.End, SurfaceProgressData.Progress);
            var ownerBottom = ownerPosition + invertedNormal * (ownerSize.y * 0.5f);
            var ownerToPoint = ownerBottom - pointOnSide;
            return Vector2.Dot(ownerToPoint, invertedNormal);
        }

        public void RecalculateActiveSurfaceProgress(Vector2 ownerPosition)
        {
            SurfaceProgressData = new SurfaceProgressData
            {
                Progress = CalculateSurfaceProgress(currentSurface, ownerPosition),
                Distance = DistanceToSide(ownerPosition)
            };

            SurfaceProgressDataUpdated?.Invoke();
        }

        private float CalculateSurfaceProgress(PlatformSurface surface, Vector2 ownerPosition)
        {
            var toPlayer = ownerPosition - surface.Data.Start;
            var projection = Vector2.Dot(toPlayer, surface.Data.Direction);
            var length = Vector2.Distance(surface.Data.Start, surface.Data.End);

            return projection / length;
        }

        private void ChangeActiveSurfaceIfNecessary(Vector2 ownerPosition)
        {
            var bestSurface = currentSurface;
            var bestProgress = SurfaceProgressData.Progress;
            var bestDistance = DistanceToOwner(bestSurface, bestProgress, ownerPosition);

            int[] neighbors = { currentSurface.NextIndex, currentSurface.PrevIndex };

            foreach (var i in neighbors)
            {
                var surface = platform.Surfaces[i];
                var progress = CalculateSurfaceProgress(surface, ownerPosition);

                if (progress < 0f || progress > 1f)
                {
                    continue;
                }

                var dist = DistanceToOwner(surface, progress, ownerPosition);

                if (dist < bestDistance)
                {
                    bestSurface = surface;
                    bestProgress = progress;
                    bestDistance = dist;
                }
            }

            currentSurface = bestSurface;

            RecalculateActiveSurfaceProgress(ownerPosition);

            SurfaceData = currentSurface.Data;
            SurfaceDataUpdated?.Invoke();
        }

        private float DistanceToOwner(PlatformSurface surface, float progress, Vector2 ownerPosition)
        {
            var p = Vector2.Lerp(surface.Data.Start, surface.Data.End, Mathf.Clamp01(progress));
            return Vector2.Distance(p, ownerPosition);
        }
    }

}
