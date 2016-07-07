using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	// Takes care of the game flow by managing the interactions between the movement and magnet controllers, and by updating the UI.
	[RequireComponent (typeof(MagnetController))]
	public class MagnetGameController : MonoBehaviour
	{
		private MagnetController magnetController;

		// FIXME should abstract here
		public MoveControllerNoVR movementController;

		// the complete base object, with its parts disabled at the start
		public GameObject baseObject;

		public AudioClip lockSound;

		public AudioClip victorySound;

		public Text victoryText;

		// here goes a complete list of all the possible combinations between base magnets and base parts
		// e.g.: the right wrist magnet on the base is connected to the right fist base part
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

		// Whenever two magnets have been joined, the moving part is hidden and a corresponding static base part takes
		// its place, so as to avoid displaying imperfect connections with space in between, wrong angles, etc.
		private void HandleMagnetsJoined (GameObject joinedPart, Magnet baseMagnetJoined)
		{
			magnetController.enabled = false;

			AudioSource.PlayClipAtPoint (lockSound, Camera.main.transform.position);

			// the moving part is immediately hidden and removed from the game
			joinedPart.SetActive (false);

			// searches for a correspondance between the base magnet which has been connected and the array of
			// possible matches, so that it can reach the relevant disabled base part and enable it 
			GameObject basePart = null;
			foreach (BaseObjectByMagnet baseObjectByMagnet in baseObjectsByMagnet) {
				if (baseObjectByMagnet.baseMagnet == baseMagnetJoined) {
					basePart = baseObjectByMagnet.baseObject;
					break;
				}
			}
			basePart.SetActive (true);

			// if all the base parts are now active, the game is over
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
			selected.AddComponent<CameraVisibility> ().ObjectInvisible += HandleObjectInvisible;

			// the controller now listens to collisions between the moving kinematic part and the other objects
			selected.GetComponent<LimitTrigger> ().OnLimitTriggered += HandleLimitTriggered;
			// the moving part collider should trigger collisions with the other objects (since the part itself is kinematic)
			selected.GetComponent<Collider> ().isTrigger = true;
			// finds all the possible matches between the moving part magnets and the base magnets
			// (which are then passed on to the MagnetController)
			List<MagnetMatch> matches = MagnetMatcher.findMatches (baseObject, selected);
			if (matches.Count > 0) {
				magnetController.matches = matches;
				magnetController.status = MagnetController.STATUS.NORMAL;
				magnetController.enabled = true;
			}
		}

		private void HandleObjectInvisible (GameObject invisible)
		{
			Debug.Log (invisible.name);

			// FIXME
			if (invisible.Equals (movementController.selected))
				movementController.DeselectObject ();

			invisible.GetComponent<Rigidbody> ().position = new Vector3 (0, 8, 4);
		}

		private void HandleObjectDeselected (GameObject deselected)
		{
			// the controller doesn't listen anymore to collisions between the ex moving part and the other objects
			deselected.GetComponent<LimitTrigger> ().OnLimitTriggered -= HandleLimitTriggered;
			// the ex moving part must react to physics collisions normally
			deselected.GetComponent<Collider> ().isTrigger = false;
			// this is to avoid violent reactions from other objects around
			deselected.AddComponent<MassDecreaser> ();

			magnetController.enabled = false;
		}

		private void GameWon ()
		{
			if (victoryText != null)
				victoryText.text = "WELL DONE!";

			AudioSource.PlayClipAtPoint (victorySound, Camera.main.transform.position);
		}

		// Whenever the moving part hits the core of another object (not the object itself, because there must be
		// some margin to allow a connection), it must be deselected
		private void HandleLimitTriggered (GameObject movingObject, GameObject staticObject)
		{
			if (staticObject.name.Equals ("Core"))
				movementController.DeselectObject ();
		}
	}
}
