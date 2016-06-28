using System;
using UnityEngine;
using System.Collections;

public class LimitTrigger : MonoBehaviour
{
	public event Action<GameObject,GameObject> OnLimitTriggered;

	void OnTriggerEnter (Collider other)
	{
		if (OnLimitTriggered != null)
			OnLimitTriggered (gameObject, other.gameObject);
	}

}
