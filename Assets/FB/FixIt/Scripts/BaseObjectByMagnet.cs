using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fb.fixit
{
	// Structure used to store the compatibility between a specific base magnet and a moving part.
	// If a base magnet is compatible with more than one moving part, a structure must be created for each combination.
	[System.Serializable]
	public class BaseObjectByMagnet
	{
		public Magnet baseMagnet;

		public GameObject baseObject;
	}
}
