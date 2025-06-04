using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item")]
    public class ItemSO : DescriptionBaseSO
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [Tooltip("Maximum number of this item that can be stacked in the inventory. Set to 0 for unlimited stacks.")]
        [SerializeField] private int maxStackSize = 1;
        [SerializeField] private StatusEffectSO buffEffect;
        [SerializeField] private StatusEffectSO debuffEffect;

        public string Name => itemName;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
        public StatusEffectSO BuffEffect => buffEffect;
        public StatusEffectSO DebuffEffect => debuffEffect;
    }
}
