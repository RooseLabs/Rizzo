using System.Threading.Tasks;
using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using RooseLabs.Events.Channels.UI;
using RooseLabs.Input;
using RooseLabs.SceneManagement.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RooseLabs.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneReference gameplayScene;
        [SerializeField] private LoadingInterfaceController loadingInterfaceController;

        [Header("Listening to")]
        [SerializeField] private LoadEventChannelSO loadMenuChannel;

        [SerializeField] private LoadEventChannelSO loadLevelChannel;
        [SerializeField] private LoadEventChannelSO coldStartupChannel;

        [Header("Broadcasting on")]
        [SerializeField] private VoidEventChannelSO onSceneReady;
        [SerializeField] private FloatEventChannelSO onLoadingProgress;
        [SerializeField] private FadeEventChannelSO fadeEventChannel;

        private SceneReference m_sceneToLoad;
        private SceneReference m_currentlyLoadedScene;
        private bool m_showLoadingScreen;
        private bool m_doFadeScreen;

        private const float FadeDuration = .5f;
        private bool m_isLoading = false;
        private Scene m_gameplayManagerScene;

        private void OnEnable()
        {
            loadMenuChannel.OnLoadingRequested += LoadMenu;
            loadLevelChannel.OnLoadingRequested += LoadLevel;
#if UNITY_EDITOR
            coldStartupChannel.OnLoadingRequested += LevelColdStartup;
#endif
        }

        private void OnDisable()
        {
            loadMenuChannel.OnLoadingRequested -= LoadMenu;
            loadLevelChannel.OnLoadingRequested -= LoadLevel;
#if UNITY_EDITOR
            coldStartupChannel.OnLoadingRequested -= LevelColdStartup;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// This special loading function is only used in the editor, when the developer presses Play in a Level scene, without passing by Initialisation.
        /// </summary>
        private void LevelColdStartup(SceneReference loadedScene, bool showLoadingScreen, bool fadeScreen)
        {
            m_currentlyLoadedScene = loadedScene;

            SceneManager.LoadScene(gameplayScene.Path, LoadSceneMode.Additive);
            m_gameplayManagerScene = gameplayScene.LoadedScene;

            StartGameplay();
        }
#endif

        /// <summary>
        /// Prepares to load the Main Menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
        /// </summary>
        private void LoadMenu(SceneReference menuToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            if (m_isLoading) return;
            m_sceneToLoad = menuToLoad;
            m_showLoadingScreen = showLoadingScreen;
            m_doFadeScreen = fadeScreen;
            m_isLoading = true;

            // In case we are coming from a Level back to the Main Menu, we need to get rid of the persistent Gameplay manager scene
            if (m_gameplayManagerScene is { isLoaded: true })
            {
                SceneManager.UnloadSceneAsync(m_gameplayManagerScene);
            }

            _ = ChangeScene();
        }

        private void LoadLevel(SceneReference levelToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            if (m_isLoading) return;
            m_sceneToLoad = levelToLoad;
            m_showLoadingScreen = showLoadingScreen;
            m_doFadeScreen = fadeScreen;
            m_isLoading = true;

            // In case we are coming from the Main Menu, we need to load the Gameplay manager scene first
            if (m_gameplayManagerScene is { isLoaded: false })
            {
                SceneManager.LoadScene(gameplayScene.Path, LoadSceneMode.Additive);
                m_gameplayManagerScene = gameplayScene.LoadedScene;
            }

            _ = ChangeScene();
        }

        private async Task ChangeScene()
        {
            InputManager.Instance.DisableAllInput();

            if (m_doFadeScreen)
            {
                fadeEventChannel.FadeOut(FadeDuration);
                await Task.Delay((int)(FadeDuration * 1000));
            }

            if (m_currentlyLoadedScene != null)
            {
                _ = SceneManager.UnloadSceneAsync(m_currentlyLoadedScene.LoadedScene);
            }

            if (m_showLoadingScreen)
            {
                loadingInterfaceController.ToggleLoadingScreen(true);

                float artificialProgress = 0f;
                var op = SceneManager.LoadSceneAsync(m_sceneToLoad.Path, LoadSceneMode.Additive);
                op!.allowSceneActivation = false;
                while (op.progress < 0.9f || artificialProgress < 1f)
                {
                    onLoadingProgress.RaiseEvent((op.progress + artificialProgress) * 0.5f);
                    await Task.Delay(100);
                    artificialProgress += 100 / 5000f; // 5 seconds of artificial loading time TODO: Remove (probably)
                }

                onLoadingProgress.RaiseEvent(1f);
                await Task.Delay(1000); // Loading animation takes roughly 1 second to transition
                op.allowSceneActivation = true;
                await op;
            }
            else
            {
                await SceneManager.LoadSceneAsync(m_sceneToLoad.Path, LoadSceneMode.Additive);
            }

            m_currentlyLoadedScene = m_sceneToLoad;
            SceneManager.SetActiveScene(m_sceneToLoad.LoadedScene);
            m_isLoading = false;

            if (m_showLoadingScreen) loadingInterfaceController.ToggleLoadingScreen(false);
            if (m_doFadeScreen) fadeEventChannel.FadeIn(FadeDuration);

            StartGameplay();
        }

        private void StartGameplay()
        {
            onSceneReady.RaiseEvent();
        }
    }
}
