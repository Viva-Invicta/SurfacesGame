using System;
using UnityEngine;

namespace SurfacesGame
{
    [Serializable]
    public class MovementSettings
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float gravityForce;
        [SerializeField] private float rotationSpeed;

        public float MoveSpeed => moveSpeed;
        public float JumpForce => jumpForce;
        public float GravityForce => gravityForce;
        public float RotationSpeed => rotationSpeed;
    }
}
