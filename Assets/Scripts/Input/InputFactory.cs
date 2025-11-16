using UnityEngine;

namespace SurfacesGame
{
    public class InputFactory
    {
        private readonly InputInitializationData data;

        public InputFactory(InputInitializationData data)
        {
            this.data = data;
        }

        public IInput Create()
        {
            IInput input;

            if (data.ForceMobileInput || Application.isMobilePlatform)
            {
                var mobileInput = new MobileInput();
                var mobileInputView = Object.Instantiate(data.MobileInputView);
                mobileInputView.transform.SetParent(data.Canvas.transform, false);
                mobileInput.SetView(mobileInputView);

                input = mobileInput;
            }
            else
            {
                input = new KeyboardInput(data.KeyboardInputData);
            }

            return input;
        }
    }
}