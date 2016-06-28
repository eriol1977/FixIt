using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	[RequireComponent (typeof(MagnetController))]
	public class MagnetGameController : MonoBehaviour
	{
		private MagnetController magnetController;

		public AbsMovementController movementController;

		public GameObject baseObject;

		public AudioClip lockSound;

		public AudioClip victorySound;

		public Text victoryText;

		public BaseObjectByMagnet[] baseObjectsByMagnet;

		void Awake ()
		{
			magnetController = GetComponent<MagnetController> ();
			magnetController.OnMagnetsJoined += HandleMagnetsJoined;
			magnetController.enabled = false;

			movementController.ObjectSelected += HandleObjectSelected;
			movementController.ObjectDeselected += HandleObjectDeselected;

			if (victoryText != null)
				victoryText.text = "";
		}

		private void HandleMagnetsJoined (GameObject joinedPart, Magnet baseMagnetJoined)
		{
			magnetController.enabled = false;

			AudioSource.PlayClipAtPoint (lockSound, Camera.main.transform.position);

			joinedPart.SetActive (false);

			GameObject basePart = null;
			foreach (BaseObjectByMagnet baseObjectByMagnet in baseObjectsByMagnet) {
				if (baseObjectByMagnet.baseMagnet == baseMagnetJoined) {
					basePart = baseObjectByMagnet.baseObject;
					break;
				}
			}

			basePart.SetActive (true);

			bool allBasePartsActive = true;
			foreach (Transform child in baseObject.transform) {
				if (!child.gameObject.activeSelf) {
					allBasePartsActive = false;
					break;
				}
			}
			if (allBasePartsActive)
				GameWon ();
		}

		private void HandleObjectSelected (GameObject selected)
		{
			List<MagnetMatch> matches = MagnetMatcher.findMatches (baseObject, selected);
			if (matches.Count > 0) {
				magnetController.matches = matches;
				magnetController.status = MagnetController.STATUS.NORMAL;
				selected.GetComponent<Collider> ().isTrigger = true;
				magnetController.enabled = true;
			}
		}

		private void HandleObjectDeselected (GameObject deselected)
		{
			deselected.GetComponent<Collider> ().isTrigger = false;
			magnetController.enabled = false;
		}

		private void GameWon ()
		{
			if (victoryText != null)
				victoryText.text = "WELL DONE!";

			AudioSource.PlayClipAtPoint (victorySound, Camera.main.transform.position);
		}
	}
}