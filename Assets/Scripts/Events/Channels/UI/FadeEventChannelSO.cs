using System;
using RooseLabs.SceneManagement.UI;
using UnityEngine;

namespace RooseLabs.Events.Channels.UI
{
    [CreateAssetMenu(menuName = "Events/UI/Fade Event Channel")]
    public class FadeEventChannelSO : ScriptableObject
    {
        public Action<float, Color, Color?> OnEventRaised;

        /// <summary>
        /// Fade helper function to simplify usage. Fades the screen in to gameplay.
        /// </summary>
        /// <param name="duration">How long it takes to the image to fade in.</param>
        public void FadeIn(float duration)
        {
            Fade(duration, Color.clear, Color.black);
        }

        /// <summary>
        /// Fade helper function to simplify usage. Fades the screen out to black.
        /// </summary>
        /// <param name="duration">How long it takes to the image to fade out.</param>
        public void FadeOut(float duration)
        {
            Fade(duration, Color.black, Color.clear);
        }

        /// <summary>
        /// Generic fade function. Communicates with <see cref="FadeController"/>.
        /// </summary>
        /// <param name="duration">How long it takes to the image to fade in/out.</param>
        /// <param name="color">Target color for the image to reach.</param>
        /// <param name="initialColor">If provided, sets the color of the image to this color before fading.</param>
        private void Fade(float duration, Color color, Color? initialColor = null)
        {
            OnEventRaised?.Invoke(duration, color, initialColor);
        }
    }
}
