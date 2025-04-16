using UnityEngine;

namespace RooseLabs.SceneManagement.UI
{
    public class LoadingInterfaceController : MonoBehaviour
    {
        [SerializeField] private GameObject[] loadingInterfaces;

        public void ToggleLoadingScreen(bool state)
        {
            foreach (var loadingInterface in loadingInterfaces)
            {
                loadingInterface.SetActive(state);
            }
        }
    }
}
