using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Data/Player Stats")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Player Base Stats")]
        public PlayerStats data;
    }
}
