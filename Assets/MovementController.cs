using UnityEngine;
using UnityEngine.UI;

namespace SurfacesGame
{
    public class MovementController
    {
        private readonly MovementSettings settings;
        private readonly Transform owner;
        private readonly Vector2 ownerSize;

        private float verticalVelocity;
        private bool isGrounded;

        private SurfaceData surfaceData;
        private SurfaceProgressData surfaceProgressData;

        public MovementController(
            MovementSettings settings,
            Transform owner,
            Vector2 ownerSize)
        {
            this.settings = settings;
            this.owner = owner;
            this.ownerSize = ownerSize;
        }

        public void SetSurfaceData(SurfaceData surfaceData)
        {
            this.surfaceData = surfaceData;
        }

        public void SetSurfaceProgressData(SurfaceProgressData surfaceProgressData)
        {
            this.surfaceProgressData = surfaceProgressData;
        }

        public void UpdateMovement(float horizontalInput, bool jumpPressed, float deltaTime)
        {
            isGrounded = CheckIsGrounded();

            owner.position = CalculateAndApplyVelocity(owner.position, jumpPressed, deltaTime);
            owner.position = CalculateAndApplyHorizontalMomement(owner.position, horizontalInput, deltaTime);
            owner.rotation = CalculateAndApplyRotation(owner.rotation, deltaTime);
        }

        private Vector2 CalculateAndApplyVelocity(Vector2 oldPosition, bool jumpPressed, float deltaTime)
        {
            if (!isGrounded)
            {
                verticalVelocity += settings.GravityForce * deltaTime;
                return oldPosition + verticalVelocity * deltaTime * surfaceData.Normal;
            }

            if (jumpPressed)
            {
                verticalVelocity = settings.JumpForce;
                return oldPosition + verticalVelocity * deltaTime * surfaceData.Normal;
            }

            verticalVelocity = 0;
            return oldPosition;
        }

        private Vector2 CalculateAndApplyHorizontalMomement(Vector2 oldPosition, float horizontalInput, float deltaTime)
        {
            return oldPosition + horizontalInput * deltaTime * settings.MoveSpeed * surfaceData.Direction;
        }

        private bool CheckIsGrounded()
        {
            if (!IsWithinSurfaceBounds())
            {
                return false;
            }

            return surfaceProgressData.Distance >= 0;
        }

        private Quaternion CalculateAndApplyRotation(Quaternion oldRotation, float deltaTime)
        {
            var up = surfaceData.Normal;
            var targetZ = Mathf.Atan2(up.y, up.x) * Mathf.Rad2Deg - 90f;

            var targetRotation = Quaternion.Euler(0, 0, targetZ);
            return Quaternion.Lerp(oldRotation, targetRotation, settings.RotationSpeed * deltaTime);
        }

        private bool IsWithinSurfaceBounds()
        {
            var progress = surfaceProgressData.Progress;
            var tolerance = (ownerSize.x * 0.5f) / surfaceData.Length;

            return progress >= -tolerance && progress <= 1f + tolerance;
        }
    }

}
