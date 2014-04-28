using UnityEngine;
using System.Collections;

public class rotateObject : MonoBehaviour {

	public float speed;
	public bool rotateEnabled;

	public bool orbitEnabled;
	public float orbitSpeed;
	public Transform orbitCenter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (rotateEnabled)
			transform.Rotate(0, Time.deltaTime * speed, 0);


		if (orbitEnabled)
			transform.RotateAround(orbitCenter.position, Vector3.forward, orbitSpeed * Time.deltaTime);
	}
}
