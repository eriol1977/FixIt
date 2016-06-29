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
		public MagnetController magnetController;

		private GameObject selected;

		private float zoom;

		private float zoomStep = 0.2f;

		private float rotationSpeed = 100f;

		void OnEnable ()
		{
			// FIXME
			magnetController.OnMagnetsJoining += HandleMagnetsJoining;
		}

		void OnDisable ()
		{
			// FIXME
			magnetController.OnMagnetsJoining -= HandleMagnetsJoining;
		}

		void Start ()
		{
			zoom = 3f;
		}

		void Update ()
		{
			SelectObject ();

			if (selected != null) {
				Vector3 mp = Input.mousePosition;
				mp.z = zoom;
				mp = Camera.main.ScreenToWorldPoint (mp);


				var wheel = Input.GetAxis ("Mouse ScrollWheel");
				if (wheel > 0f) {
					zoom += zoomStep;
				} else if (wheel < 0f) {
					zoom -= zoomStep;
				}
				
				selected.transform.position = mp;

				selected.transform.Rotate (new Vector3 (Input.GetAxis ("Vertical"), 0, -Input.GetAxis ("Horizontal")) * Time.deltaTime * rotationSpeed, Space.World);
			}

		}

		void SelectObject ()
		{
			if (Input.GetMouseButtonDown (0) && selected == null) {
				Ray rayOrigin = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (rayOrigin, out hit, 200f, GetLayerMask ("Selectable"))) {
					selected = hit.collider.gameObject;
					
					// the selected object turns kinematic so that we can move it around
					selected.GetComponent<Rigidbody> ().isKinematic = true;

					OnObjectSelected (selected);
				}
			}
			if (Input.GetMouseButtonDown (1) && selected != null) {
				DeselectObject ();
			}
		}

		public void DeselectObject ()
		{
			if (selected != null) {
				// the deselected object turns not kinematic so that it falls and responds to physical forces
				selected.GetComponent<Rigidbody> ().isKinematic = false;
				OnObjectDeselected (selected);
				selected = null;
				zoom = 3f;
			}
		}

		private static int GetLayerMask (params string[] layerNames)
		{
			int mask = 0;
			for (int i = 0; i < layerNames.Length; i++) {
				mask = mask | 1 << LayerMask.NameToLayer (layerNames [i]);
			}
			return mask;
		}

		// FIXME
		private void HandleMagnetsJoining ()
		{
			selected = null;
			zoom = 3f;
		}
	}
}
