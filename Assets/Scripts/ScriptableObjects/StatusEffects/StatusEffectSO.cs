using System;
using RooseLabs.Enums;
using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffect", menuName = "Status/Status Effect")]
    public class StatusEffectSO : ScriptableObject
    {
        [SerializeField] protected string effectName;
        [SerializeField][TextArea] protected string effectDescription;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool isStackable;
        [Tooltip("If true, the status effect will not be displayed in the UI.")]
        [SerializeField] protected bool isHidden = false;
        [Tooltip("If true, the status effect can be removed by cleansing effects.")]
        [SerializeField] protected bool canBeCleansed = true;
        [Tooltip("The duration of the status effect in seconds. Set to 0 for permanent effects.")]
        [SerializeField] protected float duration;
        [Tooltip("When stackable, this determines how the duration stacks.")]
        [SerializeField] protected StatusEffectDurationStackingType durationStackingType;

        public virtual StatusEffectType Type => StatusEffectType.Neutral;
        public string Name => effectName;
        public virtual string Description => $"{effectName}: {effectDescription}";
        public Sprite Icon => icon;
        public float Duration => duration;
        public bool IsStackable => isStackable;
        public StatusEffectDurationStackingType DurationStackingType => durationStackingType;
        public bool IsHidden => isHidden;
        public bool CanBeCleansed => canBeCleansed;

        private int m_currentStackCount = 1;
        public int StackCount
        {
            get => isStackable ? m_currentStackCount : 1;
            set => m_currentStackCount = Math.Clamp(value, 1, int.MaxValue);
        }

        private float m_currentDuration;
        public float CurrentDuration
        {
            get => m_currentDuration;
            set => m_currentDuration = Mathf.Max(value, 0);
        }

        public bool IsPermanent { get; private set; }

        private void OnEnable()
        {
            IsPermanent = duration == 0.0f;
        }

        public virtual void Apply(PlayerStats stats)
        {
            if (IsPermanent) return;
            switch (durationStackingType)
            {
                case StatusEffectDurationStackingType.ResetDuration:
                    CurrentDuration = duration;
                    break;
                case StatusEffectDurationStackingType.AddDuration:
                    CurrentDuration += duration;
                    break;
            }
        }

        public virtual void OnUpdate(PlayerStats stats) {}
        public virtual void OnFixedUpdate(PlayerStats stats) {}
        public virtual void OnCollision(PlayerStats stats, Collision2D collision) {}
        public virtual void OnTriggerCollision(PlayerStats stats, Collider2D collider) {}

        protected float GetFinalStatEffect(float baseStatValue, StatEffect effect)
        {
            if (effect.isFlat) return GetStackedStatEffect(effect, StackCount);
            return baseStatValue * GetStackedStatEffect(effect, StackCount);
        }

        protected static float GetStackedStatEffect(StatEffect statEffect, int stackCount = 1)
        {
            switch (statEffect.stackingType)
            {
                case StatusEffectStatStackingType.Linear:
                    return statEffect.baseValue * stackCount;
                case StatusEffectStatStackingType.Hyperbolic:
                    return 1 - 1 / (1 + statEffect.baseValue * stackCount);
                case StatusEffectStatStackingType.Exponential:
                    return Mathf.Pow(statEffect.baseValue, stackCount);
                case StatusEffectStatStackingType.None:
                default:
                    return statEffect.baseValue;
            }
        }
    }
}
