using UnityEngine;
using System.Collections;

namespace fb.utils
{
	// NB: needs OVRManager component in scene (eg: on Main Camera) to work!
	public class MoveCameraWithXboxCtrl : MonoBehaviour
	{
		public float turnSpeed = 0.5f;
		public float panSpeed = 0.2f;
		public float zoomSpeed = 0.5f;

		private bool isMoving = false;

		void Update ()
		{
			if (OVRInput.GetDown (OVRInput.Button.PrimaryShoulder)) {
				isMoving = true;
				OnCameraMovementStarted ();
			}

			if (OVRInput.GetUp (OVRInput.Button.PrimaryShoulder)) {
				isMoving = false;
				OnCameraMovementEnded ();
			}


			if (isMoving) {
				// rotating
				Vector2 primaryThumbstick = OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick);
				transform.Rotate (new Vector3 (-primaryThumbstick.y * turnSpeed, 0, primaryThumbstick.x * turnSpeed));

				// panning
				Vector2 secThumbstick = OVRInput.Get (OVRInput.Axis2D.SecondaryThumbstick);
				transform.Translate (new Vector3 (secThumbstick.x * panSpeed, secThumbstick.y * panSpeed, 0));

				// zooming
				if (OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger) == 1.0f) {
					transform.Translate (new Vector3 (0, 0, zoomSpeed));
				} else if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger) == 1.0f) {
					transform.Translate (new Vector3 (0, 0, -zoomSpeed));
				}
			}

		}

		protected virtual void OnCameraMovementStarted ()
		{
			// do nothing: child classes can implement it if useful
		}

		protected virtual void OnCameraMovementEnded ()
		{
			// do nothing: child classes can implement it if useful
		}
	}
}