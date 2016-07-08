using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MassDecreaser : MonoBehaviour
{

	private Rigidbody rb;

	private float originalMass;

	private float timeToWait = 2f;

	private float endTime;

	private float decreaseFactor = 1000f;

	private float maxSpeedAfterCollision = 1f;

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

	void OnCollisionEnter (Collision other)
	{
		rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeedAfterCollision);
	}
}
