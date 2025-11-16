using System;

namespace SurfacesGame
{
    public class MobileInput : BaseInput
    {
        private MobileInputView view;

        public void SetView(MobileInputView mobileInputView)
        {
            if (view)
            {
                view.ButtonPressed -= OnButtonPressed;
                view.ButtonReleased -= OnButtonReleased;
            }

            view = mobileInputView;

            view.ButtonPressed += OnButtonPressed;
            view.ButtonReleased += OnButtonReleased;
        }

        private void OnButtonPressed(InputButtonType type)
        {
            if (type == InputButtonType.Left)
            {
                leftPressed = true;
            }

            if (type == InputButtonType.Right)
            {
                rightPressed = true;
            }

            if (type == InputButtonType.Jump)
            {
                jumpPressed = true;
            }

            UpdateData();
        }

        private void OnButtonReleased(InputButtonType type)
        {
            if (type == InputButtonType.Left)
            {
                leftPressed = false;
            }

            if (type == InputButtonType.Right)
            {
                rightPressed = false;
            }

            if (type == InputButtonType.Jump)
            {
                jumpPressed = false;
            }

            UpdateData();
        }
    }

}