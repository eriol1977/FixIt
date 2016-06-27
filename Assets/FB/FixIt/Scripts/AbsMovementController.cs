using System;
using UnityEngine;

namespace fb.fixit
{
	public class AbsMovementController : MonoBehaviour
	{
		public event Action<GameObject> ObjectSelected;

		public event Action<GameObject> ObjectDeselected;

		protected virtual void OnObjectSelected (GameObject selected)
		{
			if (ObjectSelected != null)
				ObjectSelected (selected);
		}

		protected virtual void OnObjectDeselected (GameObject deselected)
		{
			if (ObjectDeselected != null)
				ObjectDeselected (deselected);
		}
	}
}