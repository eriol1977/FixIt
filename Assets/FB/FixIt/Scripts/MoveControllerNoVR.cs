using System;
using UnityEngine;
using System.Collections;

namespace fb.fixit
{
	// A basic movement controller:
	// - a left click on any object belonging to the "Selectable" layer selects it
	// - a right click deselects the presently selected object
	// - moving the mouse with an object selected moves the object around on the X and Y axes
	// - scrolling the mouse wheel with an object selected moves the object along the Z axes
	// - the arrow keys rotate the selected object around two of its axes
	public class MoveControllerNoVR : AbsMovementController
	{
		protected override void ManageSelection ()
		{
			if (Input.GetMouseButtonDown (0)) {
				SelectFromInputDevice (Input.mousePosition);
			} else if (Input.GetMouseButtonDown (1)) {
				DeselectFromInputDevice ();
			}
		}

		protected override Vector3 GetWorldInputPosition ()
		{
		        Vector3 pos = Input.mousePosition;
			pos.z = zoom;
			pos = Camera.main.ScreenToWorldPoint (pos);
		}
	
		protected override void CalculateZoom ()
		{
			var wheel = Input.GetAxis ("Mouse ScrollWheel");
			if (wheel > 0f) {
				zoom += zoomStep;
			} else if (wheel < 0f) {
				zoom -= zoomStep;
			}
		}

		protected override Vector3 CalculateRotationAngle ()
		{
			return new Vector3 (Input.GetAxis ("Vertical"), 0, -Input.GetAxis ("Horizontal"));
		}
	}
}
