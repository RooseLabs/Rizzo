using System;
using UnityEngine;

namespace RooseLabs.Events.Channels
{
	/// <summary>
	///   <para>This class is used for Events that take no arguments.</para>
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Void Event Channel")]
	public class VoidEventChannelSO : ScriptableObject
	{
		public event Action OnEventRaised;

		public void RaiseEvent()
		{
			OnEventRaised?.Invoke();
		}
	}
}
