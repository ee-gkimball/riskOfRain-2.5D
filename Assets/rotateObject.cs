using UnityEngine;
using System.Collections;

public class rotateObject : MonoBehaviour {

	public float speed;
	public bool enabled;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (enabled)
			transform.Rotate(0, Time.deltaTime * speed, 0);
	}
}
