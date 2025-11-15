using UnityEngine;

namespace SurfacesGame
{
    public class MovementController
    {
        private readonly MovementSettings settings;
        private readonly Transform owner;
        private readonly PlatformNavigator platformNavigator;
        private readonly Vector2 ownerSize;

        private float verticalVelocity;
        private float addedVerticalDistance;

        private bool isGrounded;
        private SurfaceData surfaceData;

        public MovementController(MovementSettings settings, Transform owner, PlatformNavigator navigator, Vector2 ownerSize)
        {
            this.settings = settings;
            this.owner = owner;
            this.platformNavigator = navigator;
            this.ownerSize = ownerSize;

            SnapToSide();
        }

        public void SetSurfaceData(SurfaceData surfaceData)
        {
            this.surfaceData = surfaceData;
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
                return oldPosition + deltaTime * verticalVelocity * surfaceData.Normal;
            }

            if (jumpPressed)
            {
                verticalVelocity = settings.JumpForce;
                return oldPosition + deltaTime * verticalVelocity * surfaceData.Normal;
            }

            verticalVelocity = 0;
            return oldPosition;
        }

        private Vector2 CalculateAndApplyHorizontalMomement(Vector2 oldPosition, float horizontalInput, float deltaTime)
        {
            return oldPosition + deltaTime * horizontalInput * settings.MoveSpeed * surfaceData.Direction;
        }

        private bool CheckIsGrounded()
        {
            var dist = platformNavigator.DistanceToSide(owner.position);
            var ownerSizeFraction = (ownerSize.x * 0.5f) / surfaceData.Length;

            if (platformNavigator.SideProgress > 1f + ownerSizeFraction ||
                platformNavigator.SideProgress < 0f - ownerSizeFraction)
            {
                return false;
            }

            return dist >= 0;
        }

        private Quaternion CalculateAndApplyRotation(Quaternion oldRotation, float deltaTime)
        {
            var activeSideUp = surfaceData.Normal;
            var targetZAngle = Mathf.Atan2(activeSideUp.y, activeSideUp.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.Euler(0, 0, targetZAngle);
            return Quaternion.Lerp(oldRotation, targetRotation, settings.RotationSpeed * deltaTime);
        }

        private void SnapToSide()
        {
            owner.position = platformNavigator.GetSnapPosition(addedVerticalDistance);
        }
    }
}
