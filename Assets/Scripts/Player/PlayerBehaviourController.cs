using UnityEngine;

namespace SurfacesGame
{
    public class PlayerBehaviourController : MonoBehaviour
    {
        [SerializeField] private MovementSettings movementSettings;
        [SerializeField] private Vector2 size;

        private IInput input;
        private InputData inputData;

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

            input.InputDataUpdated += HandleInputDataUpdated;

            platformNavigator = new PlatformNavigator(platform, size);
            movementController = new MovementCalculator(movementSettings, size);

            platformNavigator.SurfaceDataUpdated += HandleSurfaceDataUpdated;
            platformNavigator.SurfaceProgressDataUpdated += HandleSurfaceProgressDataUpdated;

            transform.position = platformNavigator.GetSnapPosition();

            isInitialized = true;
        }

        public void Release()
        {
            platformNavigator.SurfaceDataUpdated -= HandleSurfaceDataUpdated;
            platformNavigator.SurfaceProgressDataUpdated -= HandleSurfaceProgressDataUpdated;

            input.InputDataUpdated -= HandleInputDataUpdated;

            isInitialized = false;
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

            (transform.position, transform.rotation) = movementController.CalculateMovement(position, rotation, inputData, Time.deltaTime);
        }

        private void OnDestroy()
        {
            Release();
        }

        private void HandleSurfaceDataUpdated()
        {
            movementController.SetSurfaceData(platformNavigator.SurfaceData);
        }

        private void HandleSurfaceProgressDataUpdated()
        {
            movementController.SetSurfaceProgressData(platformNavigator.SurfaceProgressData);
        }

        private void HandleInputDataUpdated()
        {
            inputData = input.Data;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
        }
    }
}
