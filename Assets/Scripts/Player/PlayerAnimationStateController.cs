using UnityEngine;

namespace RooseLabs.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationStateController : MonoBehaviour
    {
        private Animator m_animator;
        private GameObject[] m_weapons;

        // Float Parameters
        public readonly int F_Velocity = Animator.StringToHash("Velocity");

        // Trigger Parameters
        public readonly int T_IdleGroom = Animator.StringToHash("IdleGroom");

        // Animations
        public readonly int A_Dodge = Animator.StringToHash("Dodge");

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_weapons = GameObject.FindGameObjectsWithTag("Weapon");
        }

        public void Play(int animStateHash) => m_animator.Play(animStateHash);

        public void SetFloat(int parameterHash, float value) => m_animator.SetFloat(parameterHash, value);

        public void SetTrigger(int parameterHash) => m_animator.SetTrigger(parameterHash);

        public void HideWeapons()
        {
            foreach (GameObject weapon in m_weapons)
            {
                weapon.SetActive(false);
            }
        }
    }
}