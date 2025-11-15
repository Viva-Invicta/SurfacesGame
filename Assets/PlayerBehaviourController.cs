using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SurfacesGame
{
    public class PlayerBehaviourController : MonoBehaviour
    {
        [SerializeField] private MovementSettings movementSettings;
        [SerializeField] private Vector2 size;

        private IInput input;

        private MovementController movementController;
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
            movementController = new MovementController(movementSettings, transform, platformNavigator, size);

            platformNavigator.SurfaceDataUpdated += HandleSurfaceDataUpdated;

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            platformNavigator.UpdateNavigation(transform.position);
            movementController.UpdateMovement(input.HorizontalMovement, input.JumpPressed, Time.deltaTime);
        }

        private void OnDestroy()
        {
            platformNavigator.SurfaceDataUpdated -= HandleSurfaceDataUpdated;
        }

        private void HandleSurfaceDataUpdated()
        {
            movementController.SetSurfaceData(platformNavigator.SurfaceData);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
        }
    }


    public interface IInput
    {
        public void Refresh();

        public float HorizontalMovement { get; }
        public bool JumpPressed { get; }
    }
}
