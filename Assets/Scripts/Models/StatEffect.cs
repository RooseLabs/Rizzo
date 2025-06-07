using System;
using RooseLabs.Enums;
using UnityEngine;

namespace RooseLabs.Models
{
    [Serializable]
    public struct StatEffect
    {
        public float baseValue;

        [Tooltip("If true, this stat effect is applied as a flat value. Otherwise, it is applied as a percentage.")]
        public bool isFlat;

        [Tooltip("When stackable, this determines how this stat effect stacks.")]
        public StatusEffectStatStackingType stackingType;
    }
}
