using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    public class DescriptionBaseSO : ScriptableObject
    {
        [SerializeField][TextArea] private string description;
    }
}
