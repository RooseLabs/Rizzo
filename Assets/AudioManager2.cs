using UnityEngine;

namespace RooseLabs
{
    public class AudioManager2 : MonoBehaviour
    {
        [Header("-----Audio Source-----")]
        [SerializeField] AudioSource musicSource;

        [Header("-----Audio Clips-----")]
        public AudioClip lvl2;
    
        private void Start()
        {
            musicSource.clip = lvl2;
            musicSource.Play();
        }
    }
}