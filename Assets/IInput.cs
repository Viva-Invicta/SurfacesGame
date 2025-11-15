namespace SurfacesGame
{
    public interface IInput
    {
        public void Refresh();

        public float HorizontalMovement { get; }
        public bool JumpPressed { get; }
    }
}
