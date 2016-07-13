using UnityEngine;
using System;
using System.Collections;

namespace fb.utils
{
	public class CameraVisibility : MonoBehaviour
	{
		private Collider anObjCollider;
		private Camera cam;
		private Plane[] planes;

		public event Action<GameObject> ObjectInvisible;

		void Start ()
		{
			cam = Camera.main;
			planes = GeometryUtility.CalculateFrustumPlanes (cam);
			anObjCollider = GetComponent<Collider> ();
		}

		void Update ()
		{
			if (!GeometryUtility.TestPlanesAABB (planes, anObjCollider.bounds)) {
				if (ObjectInvisible != null)
					ObjectInvisible (gameObject);
			}
		}
	}
}
