using System;
using UnityEngine;

namespace RooseLabs.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationStateController : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public event Action OnHideWeaponsRequested;

        #region Float Parameters
        public readonly int F_Velocity = Animator.StringToHash("Velocity");
        #endregion

        #region Trigger Parameters
        public readonly int T_IdleGroom = Animator.StringToHash("IdleGroom");
        #endregion

        #region Animation States
        public readonly int A_Dodge = Animator.StringToHash("Dodge");
        #endregion

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void RequestHideWeapons() => OnHideWeaponsRequested?.Invoke();
    }
}
