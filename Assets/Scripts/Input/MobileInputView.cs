using System;
using UnityEngine;
using UnityEngine.UI;

namespace SurfacesGame
{
    public class MobileInputView : MonoBehaviour
    {
        public event Action<InputButtonType> ButtonPressed;
        public event Action<InputButtonType> ButtonReleased;

        [SerializeField]
        private UIInputButtonData[] buttonDatas;

        private void OnEnable()
        {
            foreach (var data in buttonDatas)
            {
                data.Button.OnPressed += () => ButtonPressed?.Invoke(data.ButtonType);
                data.Button.OnReleased += () => ButtonReleased?.Invoke(data.ButtonType);
            }
        }

        private void OnDisable()
        {
            foreach (var data in buttonDatas)
            {
                data.Button.Release();
            }
        }
    }
}