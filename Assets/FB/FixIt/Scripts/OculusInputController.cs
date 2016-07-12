using System;
using UnityEngine;
using System.Collections;

namespace fb.fixit
{
	// A basic movement controller:
	// - an A press on any "gazed at" object belonging to the "Selectable" layer selects it
	// - a B press deselects the presently selected object
	// - moving the head with an object selected moves the object around
	// - pressing the triggers with an object selected moves the object along the Z axis
	// - the primary thumbstick rotates the selected object around two of its axes
	public class OculusInputController : AbsMovementController
	{
		protected override void ManageSelection ()
		{
			if (OVRInput.GetDown (OVRInput.Button.One)) {
				SelectFromInputDevice (); 
			} else if (OVRInput.GetDown (OVRInput.Button.Two)) {
				DeselectFromInputDevice ();
			}
		}

		protected override Vector3 GetScreenInputPosition ()
		{
			// TODO inform here the gaze screen position, based on Oculus tracking!
			// Could this be ok, if we consider the center of the screen as the "looked at" point
			// of the camera, which in turn moves along with the head?
			// test by moving the camera with a mouse script
			return new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane);
		}

		protected override void CalculateZoom ()
		{
			if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger) == 1.0f) {
				zoom += zoomStep;
			} else if (OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger) == 1.0f) {
				zoom -= zoomStep;
			}
		}

		protected override Vector3 CalculateRotationAngle ()
		{
			Vector2 primaryThumbstick = OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick);
			return new Vector3 (primaryThumbstick.y, 0, -primaryThumbstick.x);
		}
	}
}
