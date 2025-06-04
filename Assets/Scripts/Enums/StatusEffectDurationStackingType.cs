using System;

namespace RooseLabs.Enums
{
    [Flags]
    public enum StatusEffectDurationStackingType
    {
        None = 0,
        ResetDuration = 1,
        AddDuration = 2
    }
}
