using System;
using UnityEngine;

namespace RooseLabs.UI.Gameplay
{
    public class UIGameOverScreen : MonoBehaviour
    {
        public Action OnGameOverAnimationComplete;

        private void OnAnimationComplete()
        {
            OnGameOverAnimationComplete?.Invoke();
        }
    }
}
