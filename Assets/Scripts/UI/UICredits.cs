using System;
using UnityEngine;

namespace RooseLabs.UI
{
    public class UICreditsManager : MonoBehaviour
    {
        public Action BackButtonAction;

        public void BackButton()
        {
            BackButtonAction.Invoke();
        }
    }
}
