using UnityEngine;
using System.Collections;
using fb.utils;

namespace fb.fixit
{
	public class XboxCameraCtrl : MoveCameraWithXboxCtrl
	{
		private OculusInputController inputCtrl;

		void Awake ()
		{
			inputCtrl = Object.FindObjectOfType<OculusInputController> ();
		}

		private void manageInputCtrl (bool activate)
		{
			inputCtrl.enabled = activate;
		}
	}
}
