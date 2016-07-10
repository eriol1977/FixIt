using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

	public GameObject target;

	public float rotationSpeed = 100f;

	public float translationSpeed = 0.1f;

	private float zoomInLimit = 3.0f;

	private float zoomOutLimit = 15.0f;

	private Vector3 point;
	//the coord to the point where the camera looks at

	// Use this for initialization
	void Start ()
	{
		point = target.transform.position;//get target's coords
		transform.LookAt (point);//makes the camera look to it 
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.H))
			transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), Time.deltaTime * rotationSpeed * -1);
		else if (Input.GetKey (KeyCode.K))
			transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), Time.deltaTime * rotationSpeed);
		else if (Input.GetKey (KeyCode.U) && Vector3.Distance (transform.position, point) > zoomInLimit)
			transform.Translate (Vector3.forward * translationSpeed);
		else if (Input.GetKey (KeyCode.J) && Vector3.Distance (transform.position, point) < zoomOutLimit)
			transform.Translate (Vector3.forward * translationSpeed * -1);
	}
}
