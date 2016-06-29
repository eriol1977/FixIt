using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	// Manages the connection between compatible magnets
	public class MagnetController : MonoBehaviour
	{
		// every time a moving part is selected, here go all the possible matches between its magnets and the base magnets
		public List<MagnetMatch> matches;

		// once a connection has been estabilished, it contains the base magnet
		public Magnet baseMagnetToJoin;

		// once a connection has been estabilished, it contains the moving part magnet
		public Magnet movingMagnetToJoin;

		// minimum distance between magnets to estabilish a connection
		private float distanceThreshold = 0.5f;

		// minimum rotation difference between magnets to estabilish a connection
		private Vector3 rotationThreshold = new Vector3 (5, 5, 5);

		// event sent when two magnets are joining (FIXME: still used?)
		public event Action OnMagnetsJoining;

		// event sent when two magnets have been connected
		public event Action<GameObject, Magnet> OnMagnetsJoined;

		public enum STATUS
		{
			NORMAL, // when trying to find a connection
			JOINING, // when joining two magnets (FIXME: still used?)
			JOINED // when two magnets have been connected
		}

		// present status
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
				// for each possible match between one magnet on the moving part and those on the base
				foreach (MagnetMatch match in matches) {
					// for each of the possible base magnets
					foreach (Magnet baseMagnet in match.baseMagnets) {
						// keeps monitoring position and rotation difference until the connection threshold is reached
						if (checkDistance (baseMagnet, match.movingMagnet) && checkRotation (baseMagnet, match.movingMagnet)) {
							// disables the moving part collider to avoid collisions while joining magnets
							match.movingMagnet.transform.parent.GetComponent<Collider> ().enabled = false;
							
							// FIXME: still useful?
							if (OnMagnetsJoining != null)
								OnMagnetsJoining ();

							baseMagnetToJoin = baseMagnet;
							movingMagnetToJoin = match.movingMagnet;
							status = STATUS.JOINING;
							break;
						}
					}
				}
			// FIXME: still useful?
			} else if (status == STATUS.JOINING) {
				joinMagnets ();
			}
		}

		// Checks distance threshold between magnets
		private bool checkDistance (Magnet baseMagnet, Magnet movingMagnet)
		{
			float distance = Vector3.Distance (baseMagnet.transform.position, movingMagnet.transform.position);
			return Mathf.Abs (distance) <= distanceThreshold;
		}

		// Checks rotation difference threshold between magnets
		// FIXME: with a different controller, the Y axis should be considered too...
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
