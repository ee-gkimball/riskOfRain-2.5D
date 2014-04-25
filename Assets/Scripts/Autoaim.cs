using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Autoaim : MonoBehaviour {

	public GameObject autoAimObject;
	public GameObject[] targets = new GameObject[2]; //max of 2 targets at once;
	public List<GameObject> detected_entities = new List<GameObject>();
	public float assist_angle;

	// Use this for initialization
	void Start () {
		autoAimObject = GameObject.Find("autoAim");
	}
	
	// Update is called once per frame
	void Update () {
		GameObject closest_entity;

		if (detected_entities.Count > 0){
			foreach(GameObject entity in detected_entities)
				if (entity.GetComponent<Entity>().isDead){
					detected_entities.Remove(entity);
					return;
				}


			closest_entity = detected_entities[0].gameObject;

			for (int i = 0; i < detected_entities.Count; i++){
				if (Vector3.Distance(this.transform.position, detected_entities[i].transform.position)
				    <
				    Vector3.Distance(this.transform.position, closest_entity.transform.position))
					closest_entity = detected_entities[i];
			}
			Vector3 target = closest_entity.transform.position;
			target.y = closest_entity.transform.position.y;

			autoAimObject.transform.LookAt(target);
			float angleDif = Quaternion.Angle(autoAimObject.transform.localRotation, Quaternion.Euler(new Vector3(0,0,0)));
			if (angleDif > assist_angle)
				autoAimObject.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));

		}
		else{
			closest_entity = null;
			autoAimObject.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		}

	}

	void OnTriggerEnter(Collider entity){
		if (entity.tag == "enemy" && !entity.GetComponent<Entity>().isDead)
			detected_entities.Add(entity.gameObject);
	}

	void OnTriggerExit(Collider entity){
		if (entity.tag == "enemy")
			detected_entities.Remove(entity.gameObject);
	}
}
