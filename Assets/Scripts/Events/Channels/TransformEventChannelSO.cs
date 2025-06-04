using System;
using UnityEngine;

namespace RooseLabs.Events.Channels
{
	/// <summary>
	///   <para>This class is used for Events that have one (1) Transform argument.</para>
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Transform Event Channel")]
	public class TransformEventChannelSO : ScriptableObject
	{
		public event Action<Transform> OnEventRaised;

		public void RaiseEvent(Transform value)
		{
			OnEventRaised?.Invoke(value);
		}
	}
}
