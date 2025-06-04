using RooseLabs.Enemies.Spider;
using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "Spider", menuName = "Enemies/Spider")]
    public class SpiderSO : BaseEnemySO
    {
        [SerializeField] private SpiderAilmentSO ailment;

        [Header("Attack Base Damage")]
        [SerializeField] private float jumpAttackDamage = 10f;
        [SerializeField] private float spitAttackDamage = 5f;
        [SerializeField] private float biteAttackDamage = 2f;

        public SpiderAilmentSO Ailment => ailment;
        public float JumpAttackBaseDamage => jumpAttackDamage;
        public float SpitAttackBaseDamage => spitAttackDamage;
        public float BiteAttackBaseDamage => biteAttackDamage;

        public override void Spawn(Vector3 position)
        {
            Spider.Create(this, position);
        }
    }
}
