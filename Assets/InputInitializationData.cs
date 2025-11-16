using System;
using UnityEngine;

namespace SurfacesGame
{
    [Serializable]
    public struct InputInitializationData
    {
        public bool ForceMobileInput;
        public MobileInputView MobileInputView;
        public Canvas Canvas;
        public KeyboardInputButtonData[] KeyboardInputData;
    }
}