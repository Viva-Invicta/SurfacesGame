using UnityEngine;

namespace SurfacesGame
{
    public class KeyboardInput : IInput
    {
        public InputData Data
        {
            get
            {
                return new InputData 
                { 
                    HorizontalInput = Input.GetAxisRaw("Horizontal"), 
                    JumpPressed = Input.GetKey(KeyCode.Space) 
                };
            }
        }
    }
}