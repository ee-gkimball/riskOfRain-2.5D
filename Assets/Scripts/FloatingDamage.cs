using UnityEngine;
using System.Collections;

public class FloatingDamage : MonoBehaviour {

	public Color color;
	public float scroll;
	public float duration;
	public float alpha;
	public Transform calling_transform;

	// Use this for initialization
	void Start () {
		guiText.material.color = color;
		alpha = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (alpha > 0){
			transform.position = new Vector3(Camera.main.WorldToViewportPoint(calling_transform.position).x,
			                                 transform.position.y + scroll*Time.deltaTime,
			                                 transform.position.z);
			alpha -= Time.deltaTime/duration;
			guiText.material.color = new Color(color.r, color.g, color.b, alpha);
		} else {
			Destroy(gameObject); // text vanished - destroy itself
		}
	}
}
