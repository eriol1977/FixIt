using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	public class MagnetMatcher
	{
		// Finds all the possibles matches between each of the moving part magnets
		// and the presently available and visible base magnets.
		public static List<MagnetMatch> findMatches (GameObject baseObj, GameObject movingObj)
		{
			List<MagnetMatch> result = new List<MagnetMatch> ();
			Magnet[] baseMagnets = baseObj.GetComponentsInChildren<Magnet> ();
			Magnet[] movingMagnets = movingObj.GetComponentsInChildren<Magnet> ();
			Magnet[] joinables;
			MagnetMatch magnetMatch = null;
			// for each magnet of the moving part
			foreach (Magnet movingMagnet in movingMagnets) {
				// which base magnets are compatible?
				joinables = movingMagnet.joinables;
				// which compatible base parts are present and visible in the actual base object?
				// (remember that the base parts are activated during the game, so the majority of the base magnets
				// won't be visible at the beginning)
				foreach (Magnet joinable in joinables) {
					foreach (Magnet baseMagnet in baseMagnets) {
						if (joinable == baseMagnet) {
							// creates a match object for the nmoving magnet when the first possible match is found
							if (magnetMatch == null) {
								magnetMatch = new MagnetMatch ();
								magnetMatch.movingMagnet = movingMagnet;
								magnetMatch.baseMagnets = new List<Magnet> ();
							}
							// adds the base magnet to the possible connections list of the moving magnet
							magnetMatch.baseMagnets.Add (baseMagnet);
						}
					}
				}
				if (magnetMatch != null) {
					result.Add (magnetMatch);
					magnetMatch = null;
				}
			}
			return result;
		}
	}
}
