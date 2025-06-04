using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    public class DescriptionBaseSO : ScriptableObject
    {
        [SerializeField][TextArea] protected string description;

        public string Description => description;
    }
}
