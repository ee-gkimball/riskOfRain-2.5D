using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackBox : MonoBehaviour {

	public List<GameObject> detected_colliders = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		detected_colliders.Add(col.gameObject);
		Debug.Log(col.name);
	}

	void OnTriggerExit(Collider col){
		detected_colliders.Remove(col.gameObject);
	}
}
