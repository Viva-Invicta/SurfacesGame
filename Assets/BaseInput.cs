using System;

namespace SurfacesGame
{
    public abstract class BaseInput : IInput
    {
        public event Action InputDataUpdated;

        public InputData Data { get; protected set; } = new();

        protected bool leftPressed;
        protected bool rightPressed;
        protected bool jumpPressed;

        public virtual void Refresh() { }

        protected void UpdateData()
        {
            var horizontal = CalculateHorizontal(leftPressed, rightPressed);

            var jump = jumpPressed;

            if (Data.HorizontalInput != horizontal || Data.JumpPressed != jump)
            {
                Data = new InputData
                {
                    HorizontalInput = horizontal,
                    JumpPressed = jump
                };
                InputDataUpdated?.Invoke();
            }
        }

        protected static float CalculateHorizontal(bool left, bool right)
        {
            if (left == right)
            {
                return 0;
            }

            return right ? 1 : -1;
        }
    }
}