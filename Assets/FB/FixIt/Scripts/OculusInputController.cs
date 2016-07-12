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

		protected override Vector2 GetScreenInputPosition ()
		{
			// TODO inform here the gaze screen position, based on Oculus tracking!
		       return Input.mousePosition;
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
