using UnityEngine;

namespace SurfacesGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private PlayerBehaviourController playerController;

        [SerializeField]
        private Platform platform;

        private void OnEnable()
        {
            platform.Initialize();
            playerController.Initialize(new KeyboardInput(), platform);
        }
    }
}