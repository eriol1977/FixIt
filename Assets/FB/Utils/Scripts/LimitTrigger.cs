using System;
using UnityEngine;
using System.Collections;

// Sends an event with the two colliding objects, every time a trigger is activated on this object
public class LimitTrigger : MonoBehaviour
{
	public event Action<GameObject,GameObject> OnLimitTriggered;

	void OnTriggerEnter (Collider other)
	{
		if (OnLimitTriggered != null)
			OnLimitTriggered (gameObject, other.gameObject);
	}
}
