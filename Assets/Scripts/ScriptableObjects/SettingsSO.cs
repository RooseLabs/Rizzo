using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
    public class SettingsSO : ScriptableObject
    {
        [Header("Audio Settings")]
        [SerializeField][Range(0f, 1f)] private float _masterVolume = 0.5f;
        [SerializeField][Range(0f, 1f)] private float _musicVolume = 1f;
        [SerializeField][Range(0f, 1f)] private float _sfxVolume = 1f;

        [Header("Screen Settings")]
        [SerializeField][Range(0, 2)] private int _windowModeIndex = 0;
        [SerializeField] private int _resolutionIndex = 0;
        [SerializeField][Range(0.1f, 2f)] private float _renderScale = 1f;
        [SerializeField] private bool _vSync = true;
        [Tooltip("The game's target framerate.\nSet to -1 for unlimited.\nIt has no effect if VSync is enabled.")]
        [SerializeField] private int _framerateLimit = -1;

        public void SetAudioSettings(float master, float music, float sfx)
        {
            _masterVolume = master;
            _musicVolume = music;
            _sfxVolume = sfx;
        }

        public void SetScreenSettings(int windowMode, int resolution, float renderScale, bool vsync, int framerate)
        {
            _windowModeIndex = windowMode;
            _resolutionIndex = resolution;
            _renderScale = renderScale;
            _vSync = vsync;
            _framerateLimit = framerate;
        }
    }
}
