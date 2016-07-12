using System;
using UnityEngine;
using System.Collections;

namespace fb.fixit
{
	/**
	 * Interacts with the Oculus Utilities to manage controller inputs.
	 * OBS: the OVRManager component must be placed on the main camera of the scene, otherwise this class won't work at all!
	 */
	public class OculusInputController : AbsMovementController
	{
		protected override void ManageSelection ()
		{
			if (OVRInput.GetDown (OVRInput.Button.One)) {
				// SelectFromInputDevice (); TODO inform here the gaze position!
			} else if (OVRInput.GetDown (OVRInput.Button.Two)) {
				DeselectFromInputDevice ();
			}
		}

		protected override void CalculateZoom ()
		{
			// TODO
		}

		protected override Vector3 CalculateRotationAngle ()
		{
			return Vector3.zero; // TODO
		}

		//		public event Action OnButtonOnePressed;
		//
		//		public event Action OnButtonTwoPressed;
		//
		//		public event Action OnPrimaryIndexTriggerPressed;
		//
		//		public event Action OnSecondaryIndexTriggerPressed;
		//
		//		public event Action<Vector2> OnPrimaryThumbstickMoved;
		//
		//		void Update ()
		//		{
		//			CheckInput ();
		//		}
		//
		//		private void CheckInput ()
		//		{
		//			if (OVRInput.GetDown (OVRInput.Button.One)) {
		//				if (OnButtonOnePressed != null)
		//					OnButtonOnePressed ();
		//			} else if (OVRInput.GetDown (OVRInput.Button.Two)) {
		//				if (OnButtonTwoPressed != null)
		//					OnButtonTwoPressed ();
		//			} else if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger) == 1.0f) {
		//				if (OnPrimaryIndexTriggerPressed != null)
		//					OnPrimaryIndexTriggerPressed ();
		//			} else if (OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger) == 1.0f) {
		//				if (OnSecondaryIndexTriggerPressed != null)
		//					OnSecondaryIndexTriggerPressed ();
		//			}
		//
		//			Vector2 primaryThumbstick = OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick);
		//			if (OnPrimaryThumbstickMoved != null && !primaryThumbstick.isZeroVector ())
		//				OnPrimaryThumbstickMoved.Invoke (primaryThumbstick);
		//		}
		//
		//		void OnDestroy ()
		//		{
		//			OnButtonOnePressed = null;
		//			OnButtonTwoPressed = null;
		//			OnPrimaryIndexTriggerPressed = null;
		//			OnSecondaryIndexTriggerPressed = null;
		//			OnPrimaryThumbstickMoved = null;
		//		}
	}
}