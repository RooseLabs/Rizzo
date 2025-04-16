using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RooseLabs.EditorTools
{
	/// <summary>
	/// Allows a "cold start" in the editor, when pressing Play and not passing from the Initialisation scene.
	/// </summary>
	public class EditorColdStartup : MonoBehaviour
	{
#if UNITY_EDITOR
		[SerializeField] private SceneReference thisScene;
		[SerializeField] private SceneReference persistentManagersScene;
		[SerializeField] private LoadEventChannelSO notifyColdStartupChannel;

		private bool m_isColdStart = false;

		private void Awake()
		{
			m_isColdStart = !persistentManagersScene.LoadedScene.IsValid();
		}

		private void Start()
		{
			if (!m_isColdStart) return;
			SceneManager.LoadSceneAsync(persistentManagersScene.Path, LoadSceneMode.Additive)!.completed += NotifyColdStartup;
		}

		private void NotifyColdStartup(AsyncOperation op)
		{
			if (thisScene != null)
			{
				notifyColdStartupChannel.RaiseEvent(thisScene);
			}
		}
#endif
	}
}
