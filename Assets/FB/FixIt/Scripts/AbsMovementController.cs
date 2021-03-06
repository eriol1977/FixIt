﻿using System;
using UnityEngine;

namespace fb.fixit
{
	// Abstract movement controller
	public abstract class AbsMovementController : MonoBehaviour
	{
		public MagnetController magnetController;

		protected GameObject selected;

		protected float zoom = 3f;

		protected float zoomStep = 0.2f;

		protected float rotationSpeed = 100f;

		public event Action<GameObject> ObjectSelected;

		public event Action<GameObject> ObjectDeselected;

		// if set to true, a selected object keeps following the input controller, but no rotation or zooming is applied
		public bool suspendInput = false;

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

		protected void OnEnable ()
		{
			magnetController.OnMagnetsJoined += HandleMagnetsJoined;
		}

		protected void OnDisable ()
		{
			magnetController.OnMagnetsJoined -= HandleMagnetsJoined;
		}

		protected void Update ()
		{
			ManageSelection ();

			if (selected != null) {
				ManageMovement ();
			}
		}

		protected abstract void ManageSelection ();

		private void ManageMovement ()
		{
			Vector3 pos = GetScreenInputPosition ();
			pos.z = zoom;
			pos = Camera.main.ScreenToWorldPoint (pos);
			selected.transform.position = pos;

			if (!suspendInput) {
				selected.transform.Rotate (CalculateRotationAngle () * Time.deltaTime * rotationSpeed, Space.World);
				CalculateZoom ();
			}
		}

		protected abstract Vector3 GetScreenInputPosition ();

		protected abstract void CalculateZoom ();

		protected abstract Vector3 CalculateRotationAngle ();

		private void SelectObject (GameObject obj)
		{
			selected = obj;
			// the selected object turns kinematic so that we can move it around
			selected.GetComponent<Rigidbody> ().isKinematic = true;
			OnObjectSelected (selected);
		}

		public void DeselectObject (GameObject obj)
		{
			if (selected != null && selected == obj) {
				// the deselected object turns not kinematic so that it falls and responds to physical forces
				selected.GetComponent<Rigidbody> ().isKinematic = false;
				OnObjectDeselected (selected);
				selected = null;
				zoom = 3f;
			}
		}

		private void HandleMagnetsJoined (GameObject joinedPart, Magnet baseMagnetJoined)
		{
			selected = null;
			zoom = 3f;
		}

		protected void SelectFromInputDevice ()
		{
			if (selected == null) {
				Ray rayOrigin = Camera.main.ScreenPointToRay (GetScreenInputPosition ());
				RaycastHit hit;
				if (Physics.Raycast (rayOrigin, out hit, 200f, GetLayerMask ("Selectable"))) {
					SelectObject (hit.collider.gameObject);
				}
			}
		}

		protected void DeselectFromInputDevice ()
		{
			if (selected != null) {
				DeselectObject (selected);
			}
		}

		private int GetLayerMask (params string[] layerNames)
		{
			int mask = 0;
			for (int i = 0; i < layerNames.Length; i++) {
				mask = mask | 1 << LayerMask.NameToLayer (layerNames [i]);
			}
			return mask;
		}
	}
}
