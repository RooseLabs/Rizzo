using System;
using UnityEngine;

namespace RooseLabs.Models
{
    [Serializable]
    public class WeaponAttackData : ISerializationCallbackReceiver
    {
        [SerializeField] private AnimationStateData animationState;
        [SerializeField] private float baseDamage;

        [Header("Combo Timing")]
        [SerializeField] private bool fixedDuration;
        [SerializeField] private float minComboTime = 0.25f;
        [SerializeField] private float maxComboTime = 1.0f;

        private float m_computedMinComboTime;
        private float m_computedMaxComboTime;

        public AnimationStateData AnimationState => animationState;
        public float BaseDamage => baseDamage;
        public float MinComboTime => m_computedMinComboTime;
        public float MaxComboTime => m_computedMaxComboTime;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            m_computedMinComboTime = fixedDuration ? minComboTime : animationState.length * minComboTime;
            m_computedMaxComboTime = fixedDuration ? maxComboTime : animationState.length * maxComboTime;
        }
    }
}
