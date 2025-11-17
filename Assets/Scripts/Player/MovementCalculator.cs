using UnityEngine;
using UnityEngine.UI;

namespace SurfacesGame
{
    public class MovementCalculator
    {
        private readonly MovementSettings settings;
        private readonly Vector2 ownerSize;

        private float verticalVelocity;
        private bool isGrounded;

        private SurfaceData surfaceData;
        private SurfaceProgressData surfaceProgressData;

        public MovementCalculator(
            MovementSettings settings,
            Vector2 ownerSize)
        {
            this.settings = settings;
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

        public (Vector2 newPosition, Quaternion newRotation) CalculateMovement(Vector2 oldPosition, Quaternion oldRotation, InputData inputData, float deltaTime)
        {
            isGrounded = CheckIsGrounded();

            Vector2 newPosition;
            Quaternion newRotation;

            newPosition = CalculateAndApplyVelocity(oldPosition, inputData.JumpPressed, deltaTime);
            newPosition = CalculateAndApplyHorizontalMomement(newPosition, inputData.HorizontalInput, deltaTime);
            newRotation = CalculateAndApplyRotation(oldRotation, deltaTime);

            return (newPosition, newRotation);
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
