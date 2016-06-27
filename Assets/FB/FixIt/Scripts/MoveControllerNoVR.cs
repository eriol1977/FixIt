using System;
using UnityEngine;
using System.Collections;

namespace fb.fixit
{
	public class MoveControllerNoVR : AbsMovementController
	{
		public MagnetController magnetController;

		private GameObject selected;

		private float zoom;

		private float zoomStep = 0.2f;

		private float rotationSpeed = 100f;

		void OnEnable ()
		{
			magnetController.OnMagnetsJoining += HandleMagnetsJoining;
		}

		void OnDisable ()
		{
			magnetController.OnMagnetsJoining -= HandleMagnetsJoining;
		}

		// Use this for initialization
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
					selected.GetComponent<Rigidbody> ().isKinematic = true;

					OnObjectSelected (selected);
				}
			}
			if (Input.GetMouseButtonDown (1) && selected != null) {
				DeselectObject ();
			}
		}

		void DeselectObject ()
		{
			selected.GetComponent<Rigidbody> ().isKinematic = false;
			OnObjectDeselected (selected);
			selected = null;
			zoom = 3f;
		}

		private static int GetLayerMask (params string[] layerNames)
		{
			int mask = 0;
			for (int i = 0; i < layerNames.Length; i++) {
				mask = mask | 1 << LayerMask.NameToLayer (layerNames [i]);
			}
			return mask;
		}

		private void HandleMagnetsJoining ()
		{
			selected = null;
			zoom = 3f;
		}
	}
}