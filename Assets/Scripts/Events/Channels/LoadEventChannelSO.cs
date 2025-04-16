using System;
using Eflatun.SceneReference;
using UnityEngine;

namespace RooseLabs.Events.Channels
{
    [CreateAssetMenu(menuName = "Events/Load Event Channel")]
    public class LoadEventChannelSO : ScriptableObject
    {
        public Action<SceneReference, bool, bool> OnLoadingRequested;

        public void RaiseEvent(SceneReference sceneToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (OnLoadingRequested != null)
            {
                OnLoadingRequested.Invoke(sceneToLoad, showLoadingScreen, fadeScreen);
            }
            else
            {
                Debug.LogWarning($"[{name}] A Scene loading was requested, but nobody picked it up. " +
                                 "Check why there is no SceneLoader already present, " +
                                 "and make sure it's listening to this Load Event channel.");
            }
        }
    }
}
