using UnityEngine;

namespace SurfacesGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private PlayerBehaviourController playerController;

        [SerializeField]
        private Platform platform;

        [SerializeField]
        private InputInitializationData inputInitializationData;

        private IInput input;

        private void OnEnable()
        {
            input = new InputFactory(inputInitializationData).Create();

            platform.Initialize();
            playerController.Initialize(input, platform);
        }

        private void Update()
        {
            if (input != default)
            {
                input.Refresh();
            }
        }
    }
}