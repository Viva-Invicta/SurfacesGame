using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SurfacesGame
{
    public class UIInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPressed;
        public event Action OnReleased;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
        }

        public void Release()
        {
            OnPressed = null;
            OnReleased = null;
        }
    }
}