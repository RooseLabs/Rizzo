using UnityEngine;
using UnityEngine.SceneManagement;

namespace RooseLabs.UI
{
    public class UIMenuManager : MonoBehaviour
    {
        public void OnPlay()
        {
            SceneManager.LoadScene("Backalley_1");
        }

        public void OnExit()
        {
            Application.Quit();
        }
    }
}
