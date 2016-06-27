using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	public class MagnetMatcher
	{
		public static List<MagnetMatch> findMatches (GameObject baseObj, GameObject movingObj)
		{
			List<MagnetMatch> result = new List<MagnetMatch> ();
			Magnet[] baseMagnets = baseObj.GetComponentsInChildren<Magnet> ();
			Magnet[] movingMagnets = movingObj.GetComponentsInChildren<Magnet> ();
			Magnet[] joinables;
			MagnetMatch magnetMatch = null;
			foreach (Magnet movingMagnet in movingMagnets) {
				joinables = movingMagnet.joinables;
				foreach (Magnet joinable in joinables) {
					foreach (Magnet baseMagnet in baseMagnets) {
						if (joinable == baseMagnet) {
							if (magnetMatch == null) {
								magnetMatch = new MagnetMatch ();
								magnetMatch.movingMagnet = movingMagnet;
								magnetMatch.baseMagnets = new List<Magnet> ();
							}
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