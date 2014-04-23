using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecalManager : MonoBehaviour {

	public GameObject[] decals;
	private int decalIndex;

	public GameObject basicBulletDecal;

	// Use this for initialization
	void Start () {
	
		decals = new GameObject[200];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddDecal(RaycastHit hit){
		Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		
		GameObject newDecal = (GameObject)Instantiate(basicBulletDecal, 
		                                              new Vector3(hit.point.x + (hit.normal.x * 0.001f), 
											           			  hit.point.y + (hit.normal.y * 0.001f),
										           				  hit.point.z + (hit.normal.z * 0.001f)), 
		                                              hitRotation);
		newDecal.transform.parent = this.transform;

		decalIndex++;
		if (decalIndex >= decals.Length)
			decalIndex = 0;
		if (decals[decalIndex] != null)
			Destroy(decals[decalIndex]);

		decals[decalIndex] = newDecal;
	}
}
