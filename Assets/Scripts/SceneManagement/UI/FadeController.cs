using System.Collections;
using RooseLabs.Events.Channels.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.SceneManagement.UI
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image imageComponent;

        [Header("Listening to")]
        [SerializeField] private FadeEventChannelSO fadeEventChannel;

        private void OnEnable()
        {
            fadeEventChannel.OnEventRaised += InitiateFade;
        }

        private void OnDisable()
        {
            fadeEventChannel.OnEventRaised -= InitiateFade;
        }

        /// <summary>
        /// Controls the fade-in and fade-out.
        /// </summary>
        /// <param name="duration">How long it takes to the image to fade in/out.</param>
        /// <param name="color">Target color for the image to reach.</param>
        /// <param name="initialColor">If provided, sets the color of the image to this color before fading.</param>
        private void InitiateFade(float duration, Color color, Color? initialColor = null)
        {
            if (duration == 0f)
            {
                imageComponent.color = color;
                return;
            }
            if (initialColor.HasValue)
            {
                imageComponent.color = initialColor.Value;
            }
            StartCoroutine(BlendColor(color, duration));
        }

        private IEnumerator BlendColor(Color targetColor, float duration)
        {
            Color initialColor = imageComponent.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                imageComponent.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            imageComponent.color = targetColor;
        }
    }
}
