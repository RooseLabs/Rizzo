using RooseLabs.Enums;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "Spider", menuName = "Enemies/Spider")]
    public class SpiderSO : BaseEnemySO
    {
        [SerializeField] private SpiderAilment ailment;

        public SpiderAilment Ailment => ailment;
    }
}
