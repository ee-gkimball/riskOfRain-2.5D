using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public string name;
	public string team;
	public float hp;
	public float max_hp;
	public float xp_worth;
	public float speed;
	public float damage;
	public float critical_chance;
	public float defense;
	public float stun_time;
	public GameObject sprite_plane;
	public Texture2D sprite_idle;
	public Texture2D sprite_walk;
	public Texture2D sprite_jump;
	public Texture2D sprite_shoot;
	public Texture2D sprite_death;
	bool can_jump;

	public AudioClip sound_hit;
	public AudioClip sound_death;
	public AudioClip sound_attack;

	GameObject player;
	public float detection_range;
	public float attack_range;

	CharacterMotor motor;
	float wander_time;
	bool been_hit = false;
	bool stunned = false;
	public bool isDead;

	ParticleSystem hitParticles;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");	
		motor = GetComponent<CharacterMotor>();
		motor.movement.maxForwardSpeed = speed;
		hitParticles = GetComponent<ParticleSystem> ();
		sprite_plane = transform.FindChild("spritePlane").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (stun_time <= 0){
			if (Vector3.Distance(transform.position, player.transform.position) < detection_range || been_hit){
				Basic_Move();
			}
			else if (Vector3.Distance(transform.position, player.transform.position) > detection_range)
				Wander();
		}
		else{
			stun_time -= Time.deltaTime;
			motor.inputMoveDirection = Vector3.zero;
		}

		if (hp <= 0){
			isDead = true;
			StartCoroutine(die());
		}
	}

	IEnumerator die(){
		this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		sprite_plane.GetComponent<MeshRenderer>().material.color = Color.red;
		yield return new WaitForSeconds(1.0f);
		Destroy(this.gameObject);
	}

	void Basic_Move(){
		transform.LookAt(player.transform.position);
		motor.inputMoveDirection = transform.forward;
	}

	void Wander(){
		if (wander_time <= 0){
			transform.Rotate(new Vector3(0, Random.Range(0, 180), 0));
			wander_time = Random.Range(2, 3);
		}
		else if (wander_time <= 1){
			wander_time -= Time.deltaTime;
			motor.inputMoveDirection = Vector3.zero;
		}
		else{
			wander_time -= Time.deltaTime;
			motor.inputMoveDirection = transform.forward;
		}
	}

	void TakeHit(object[] args){
		float dam =  (float)args[0];
		float knockback = (float)args[1];
		stun_time = (float)args[2];
		hitParticles.Emit (1);
		hp -= dam;
		been_hit = true;
		transform.LookAt(player.transform.position);
		motor.movement.velocity = -transform.forward * knockback;
	}
}
