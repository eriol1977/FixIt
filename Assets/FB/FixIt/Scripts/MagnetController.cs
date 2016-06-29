using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	public class MagnetController : MonoBehaviour
	{
		public List<MagnetMatch> matches;

		public Magnet baseMagnetToJoin;

		public Magnet movingMagnetToJoin;

		private float distanceThreshold = 0.5f;

		private Vector3 rotationThreshold = new Vector3 (5, 5, 5);

		public event Action OnMagnetsJoining;

		public event Action<GameObject, Magnet> OnMagnetsJoined;

		public enum STATUS
		{
			NORMAL,
			JOINING,
			JOINED
		}

		public STATUS status;

		void Start ()
		{
			status = STATUS.NORMAL;
			baseMagnetToJoin = null;
			movingMagnetToJoin = null;


		}

		public void OnDisable ()
		{
			matches = new List<MagnetMatch> ();
			baseMagnetToJoin = null;
			movingMagnetToJoin = null;
		}

		void Update ()
		{
			if (status == STATUS.NORMAL) {
				foreach (MagnetMatch match in matches) {
					foreach (Magnet baseMagnet in match.baseMagnets) {
						if (checkDistance (baseMagnet, match.movingMagnet) && checkRotation (baseMagnet, match.movingMagnet)) {
							match.movingMagnet.transform.parent.GetComponent<Collider> ().enabled = false;

							if (OnMagnetsJoining != null)
								OnMagnetsJoining ();

							baseMagnetToJoin = baseMagnet;
							movingMagnetToJoin = match.movingMagnet;
							status = STATUS.JOINING;
							break;
						}
					}
				}
			} else if (status == STATUS.JOINING) {
				joinMagnets ();
			}
		}

		private bool checkDistance (Magnet baseMagnet, Magnet movingMagnet)
		{
			float distance = Vector3.Distance (baseMagnet.transform.position, movingMagnet.transform.position);
			return Mathf.Abs (distance) <= distanceThreshold;
		}

		private bool checkRotation (Magnet baseMagnet, Magnet movingMagnet)
		{
			Vector3 difference = baseMagnet.transform.eulerAngles - movingMagnet.transform.eulerAngles;

			float absX = Mathf.Abs (difference.x);
			bool xOK = false;
			if ((absX - 0) < rotationThreshold.x || (360 - absX) < rotationThreshold.x)
				xOK = true;
			
			float absZ = Mathf.Abs (difference.z);
			bool zOK = false;
			if ((absZ - 0) < rotationThreshold.z || (360 - absZ) < rotationThreshold.z)
				zOK = true;
			
			return xOK && zOK;
		}

		private void joinMagnets ()
		{
			status = STATUS.JOINED;
			if (OnMagnetsJoined != null)
				OnMagnetsJoined (movingMagnetToJoin.transform.parent.gameObject, baseMagnetToJoin);
		}
	
	}
}