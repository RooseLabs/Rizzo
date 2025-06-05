using UnityEngine;

namespace RooseLabs
{
    public class AudioManager3 : MonoBehaviour
    {
        [Header("-----Audio Source-----")]
        [SerializeField] AudioSource musicSource;

        [Header("-----Audio Clips-----")]
        public AudioClip menu;
    
        private void Start()
        {
            if (musicSource.clip != menu || !musicSource.isPlaying)
            {
                musicSource.clip = menu;
                musicSource.Play();
            }
        }
    }
}