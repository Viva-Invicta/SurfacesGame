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
        private SurfacesNavigator surfacesNavigator;

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

            surfacesNavigator = new SurfacesNavigator(platform, size);
            movementController = new MovementCalculator(movementSettings, size);

            surfacesNavigator.SurfaceDataUpdated += HandleSurfaceDataUpdated;
            surfacesNavigator.SurfaceProgressDataUpdated += HandleSurfaceProgressDataUpdated;

            transform.position = surfacesNavigator.GetSnapPosition();

            isInitialized = true;
        }

        public void Release()
        {
            surfacesNavigator.SurfaceDataUpdated -= HandleSurfaceDataUpdated;
            surfacesNavigator.SurfaceProgressDataUpdated -= HandleSurfaceProgressDataUpdated;

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

            surfacesNavigator.UpdateNavigation(position);

            (transform.position, transform.rotation) = movementController.CalculateMovement(position, rotation, inputData, Time.deltaTime);
        }

        private void OnDestroy()
        {
            Release();
        }

        private void HandleSurfaceDataUpdated()
        {
            movementController.SetSurfaceData(surfacesNavigator.SurfaceData);
        }

        private void HandleSurfaceProgressDataUpdated()
        {
            movementController.SetSurfaceProgressData(surfacesNavigator.SurfaceProgressData);
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
