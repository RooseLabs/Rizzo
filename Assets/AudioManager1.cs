using UnityEngine;

namespace RooseLabs
{
    public class AudioManager1 : MonoBehaviour
    {
        [Header("-----Audio Source-----")]
        [SerializeField] AudioSource musicSource;

        [Header("-----Audio Clips-----")]
        public AudioClip lvl1;
    
        private void Start()
        {
            musicSource.clip = lvl1;
            musicSource.Play();
        }
    }
}