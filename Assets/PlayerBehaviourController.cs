using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SurfacesGame
{
    public class PlayerBehaviourController : MonoBehaviour
    {
        [SerializeField] private MovementSettings movementSettings;
        [SerializeField] private Vector2 size;

        private IInput input;

        private MovementCalculator movementController;
        private PlatformNavigator platformNavigator;

        private bool isInitialized;

        public void Initialize(IInput input, Platform platform)
        {
            if (input == default)
            {
                Debug.LogError($"{nameof(PlayerBehaviourController)} : Input is null");
            }

            if (!platform)
            {
                Debug.LogError($"{nameof(PlayerBehaviourController)} : Platform is null");
            }

            this.input = input;

            platformNavigator = new PlatformNavigator(platform, size);
            movementController = new MovementCalculator(movementSettings, size);

            platformNavigator.SurfaceDataUpdated += HandleSurfaceDataUpdated;
            platformNavigator.SurfaceProgressDataUpdated += HandleSurfaceProgressDataUpdated;

            transform.position = platformNavigator.GetSnapPosition();

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            var position = transform.position;
            var rotation = transform.rotation;

            platformNavigator.UpdateNavigation(position);

            var (newPosition, newRotation) = movementController.CalculateMovement(position, rotation, input.Data, Time.deltaTime);

            transform.position = newPosition;
            transform.rotation = newRotation;
        }

        private void OnDestroy()
        {
            platformNavigator.SurfaceDataUpdated -= HandleSurfaceDataUpdated;
            platformNavigator.SurfaceProgressDataUpdated -= HandleSurfaceProgressDataUpdated;
        }

        private void HandleSurfaceDataUpdated()
        {
            movementController.SetSurfaceData(platformNavigator.SurfaceData);
        }

        private void HandleSurfaceProgressDataUpdated()
        {
            movementController.SetSurfaceProgressData(platformNavigator.SurfaceProgressData);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
        }
    }
}
