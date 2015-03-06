using UnityEngine;
using System.Collections;

public class WalkAroundPlanet : MonoBehaviour {

	public Transform myGravity;
	
	float gravityVal = 0.001f;
	float wander_time;
	float direction;
	float velocity;

	void Start(){
		Random.seed = System.Environment.TickCount;
	}
	
	void Update()
	{
		if (direction == 2 || direction == -1){
			direction = -1;
			this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1,1);
		}
		else
			this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1,1);

		transform.RotateAround(myGravity.position, Vector3.forward, velocity * direction * Time.deltaTime);

		if (wander_time <= 0){
			direction = Random.Range(1,3);
			wander_time = Random.Range(1, 5);
		}
		else if (wander_time <= 1){
			wander_time -= Time.deltaTime;
			velocity = 0;
		}
		else{
			wander_time -= Time.deltaTime;
			velocity = 3.0f;
		}
	}

}
