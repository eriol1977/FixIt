using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	// stores all the possible connections between a single magnet from a moving part and the base magnets
	[System.Serializable]
	public class MagnetMatch
	{
		public Magnet movingMagnet;

		public List<Magnet> baseMagnets;
	}
}
