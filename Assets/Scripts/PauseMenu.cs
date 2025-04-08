using UnityEngine;

namespace RooseLabs
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        public void Pause()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Continue()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}
