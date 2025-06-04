using RooseLabs.Enums;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    [CreateAssetMenu(fileName = "SpiderAilment", menuName = "Status/Spider Ailment")]
    public class SpiderAilmentSO : StatusEffectSO
    {
        private void Reset()
        {
            type = StatusEffectType.Debuff;
        }
    }
}
