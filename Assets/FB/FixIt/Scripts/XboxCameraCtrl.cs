using UnityEngine;
using System.Collections;
using fb.utils;

namespace fb.fixit
{
	// Moves both the camera and the objects by means of an Xbox controller.
	// While moving the camera, actions on the selected object are suspended (it just follows the camera).
	public class XboxCameraCtrl : MoveCameraWithXboxCtrl
	{
		private OculusInputController inputCtrl;

		void Awake ()
		{
			inputCtrl = Object.FindObjectOfType<OculusInputController> ();
		}

		private void manageInputCtrl (bool activate)
		{
			inputCtrl.suspendInput = activate;
		}

		protected override void OnCameraMovementStarted ()
		{
			manageInputCtrl (true);
		}

		protected override void OnCameraMovementEnded ()
		{
			manageInputCtrl (false);
		}
	}
}
