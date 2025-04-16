using System;
using UnityEngine;

namespace RooseLabs.Events.Channels
{
	/// <summary>
	///   <para>This class is used for Events that have one (1) float argument.</para>
	///   <para>Example: An event to report the progress of something.</para>
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Float Event Channel")]
	public class FloatEventChannelSO : ScriptableObject
	{
		public event Action<float> OnEventRaised;

		public void RaiseEvent(float value)
		{
			OnEventRaised?.Invoke(value);
		}
	}
}
