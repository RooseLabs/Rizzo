using RooseLabs.Events.Channels;
using UnityEngine;

namespace RooseLabs.SceneManagement.UI
{
    [RequireComponent(typeof(Animator))]
    public class LoadingCoinController : MonoBehaviour
    {
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private float smoothTime = 0.85f;

        [Header("Listening to")]
        [SerializeField] private FloatEventChannelSO onLoadingProgress;

        private float loadingProgress = 0f;
        private float displayedProgress = 0f;
        private float velocity = 0f;

        private bool m_isFlipped;
        private readonly Quaternion m_defaultRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion m_flippedRotation = Quaternion.Euler(0, 180, 0);

        private void OnEnable()
        {
            onLoadingProgress.OnEventRaised += UpdateLoadingProgress;
            animationClip.SampleAnimation(gameObject, 0f);
            Rotate180();
        }

        private void OnDisable()
        {
            onLoadingProgress.OnEventRaised -= UpdateLoadingProgress;
        }

        private void Update()
        {
            // Smoothly approach loadingProgress
            displayedProgress = Mathf.SmoothDamp(displayedProgress, loadingProgress, ref velocity, smoothTime);

            // Convert to time and sample the animation
            float time = displayedProgress * animationClip.length;
            animationClip.SampleAnimation(gameObject, time);
        }

        private void UpdateLoadingProgress(float progress) => loadingProgress = progress;

        public void Rotate180()
        {
            transform.parent.localRotation = m_isFlipped ? m_defaultRotation : m_flippedRotation;
            m_isFlipped = !m_isFlipped;
        }
    }
}
