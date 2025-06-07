using System;
using UnityEngine;

namespace RooseLabs.Events.Channels
{
	/// <summary>
	///   <para>This class is used for Events that have one (1) bool argument.</para>
	///   <para>Example: An event to toggle a UI interface.</para>
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
	public class BoolEventChannelSO : ScriptableObject
	{
		public event Action<bool> OnEventRaised;

		public void RaiseEvent(bool value)
		{
			OnEventRaised?.Invoke(value);
		}
	}
}
