using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SurfacesGame
{
    public class KeyboardInput : BaseInput
    {
        private readonly Dictionary<InputButtonType, KeyCode[]> _bindings;

        public KeyboardInput(IEnumerable<KeyboardInputButtonData> buttonDatas)
        {
            _bindings = buttonDatas
                .GroupBy(b => b.ButtonType)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(b => b.KeyCode).ToArray()
                );
        }

        public override void Refresh()
        {
            leftPressed = IsPressed(InputButtonType.Left);
            rightPressed = IsPressed(InputButtonType.Right);
            jumpPressed = IsPressed(InputButtonType.Jump);

            UpdateData(); 
        }

        private bool IsPressed(InputButtonType buttonType)
        {
            if (!_bindings.TryGetValue(buttonType, out var keys))
            {
                return false;
            }

            foreach (var key in keys)
            {
                if (Input.GetKey(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
