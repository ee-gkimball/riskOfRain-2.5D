using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
	public bool lock_x = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (lock_x){
			Vector3 v = Camera.main.transform.position - transform.position;
			
			v.x = v.z = 0.0f;
			
			transform.LookAt(Camera.main.transform.position - v); 
			transform.Rotate(new Vector3(90, 0 ,0));
		}
		else
			transform.LookAt(Camera.main.transform.position); 
	}
}
