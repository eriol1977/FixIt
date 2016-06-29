using UnityEngine;
using System.Collections;

namespace fb.fixit
{
	public class Magnet : MonoBehaviour
	{
		// if this magnet belongs to the base, the array should remain empty;
		// if this magnet belongs to a moving part, the array must contain all the base magnets which can be joined to this one
		// e.g: if this magnet belongs to a fist, the array will contain both the magnets for the left and right wrists on the base
		public Magnet[] joinables;
	}
}
