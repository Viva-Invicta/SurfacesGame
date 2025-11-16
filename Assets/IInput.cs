using System;

namespace SurfacesGame
{
    public interface IInput
    {
        public event Action InputDataUpdated;
        public InputData Data { get; }
        public void Refresh();
    }
}
