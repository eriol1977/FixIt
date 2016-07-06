using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MassDecreaser : MonoBehaviour
{

	private Rigidbody rb;

	private float originalMass;

	public float timeToWait = 2f;

	private float endTime;

	private float decreaseFactor = 1000f;

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		originalMass = rb.mass;
		rb.mass = originalMass / decreaseFactor;
	}

	void Update ()
	{
		timeToWait -= Time.deltaTime;
		if (timeToWait < 0) {
			rb.mass = originalMass;
			Destroy (this);
		}
	}
}
