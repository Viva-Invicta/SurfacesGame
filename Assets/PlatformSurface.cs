namespace SurfacesGame
{
    [System.Serializable]
    public struct PlatformSurface
    {
        public SurfaceData Data;

        public int NextIndex;
        public int PrevIndex;

        public PlatformSurface(SurfaceData data)
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
}