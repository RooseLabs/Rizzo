using RooseLabs.Enums;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffect", menuName = "Status/Status Effect")]
    public class StatusEffectSO : DescriptionBaseSO
    {
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected string effectName;
        [SerializeField] protected Sprite icon;
        [Tooltip("The duration of the status effect in seconds. Set to 0 for permanent effects.")]
        [SerializeField] protected float duration;
        [SerializeField] protected bool isStackable;
        [Tooltip("When stackable, this determines how the duration stacks.")]
        [SerializeField] protected StatusEffectDurationStackingType durationStackingType;
        [Tooltip("When stackable, this determines how the stat modifications this effect applies stack.")]
        [SerializeField] protected StatusEffectStatStackingType statStackingType;
        [Tooltip("If true, the status effect will not be displayed in the UI.")]
        [SerializeField] protected bool isHidden = false;
        [Tooltip("If true, the status effect can be removed by cleansing effects.")]
        [SerializeField] protected bool canBeCleansed = true;

        public StatusEffectType Type => type;
        public string Name => effectName;
        public Sprite Icon => icon;
        public float Duration => duration;
        public bool IsStackable => isStackable;
        public StatusEffectDurationStackingType DurationStackingType => durationStackingType;
        public StatusEffectStatStackingType StatStackingType => statStackingType;
        public bool IsHidden => isHidden;
        public bool CanBeCleansed => canBeCleansed;
    }
}
