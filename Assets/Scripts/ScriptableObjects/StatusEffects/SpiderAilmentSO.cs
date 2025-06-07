using RooseLabs.Enums;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    [CreateAssetMenu(fileName = "SpiderAilment", menuName = "Status/Spider Ailment")]
    public class SpiderAilmentSO : StatusEffectSO
    {
        [Header("About (Additional Info)")]
        [SerializeField][TextArea] private string description;

        public override StatusEffectType Type => StatusEffectType.Debuff;
    }
}
