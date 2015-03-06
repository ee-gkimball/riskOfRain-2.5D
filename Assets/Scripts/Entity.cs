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
	public AudioClip sound_spawn;

	GameObject player;
	public float detection_range;
	public float attack_range;
	public float attack_rate;
	public bool is_attacking;
	public float attack_timer;

	CharacterMotor motor;
	float wander_time;
	bool been_hit = false;
	bool stunned = false;
	public bool isDead;

	ParticleSystem hitParticles;
	public Transform floatingHitPoints;
	public GameObject attackHitBox;

	bool inDeathCoroutine;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");	
		motor = GetComponent<CharacterMotor>();
		motor.movement.maxForwardSpeed = speed;
		hitParticles = GetComponent<ParticleSystem> ();
		sprite_plane = transform.FindChild("spritePlane").gameObject;
		attackHitBox = transform.FindChild("attackHitBox").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (stun_time <= 0 && !isDead && !is_attacking){
			if (Vector3.Distance(transform.position, player.transform.position) < detection_range || been_hit){
				Basic_Move();
			}
			else if (Vector3.Distance(transform.position, player.transform.position) > detection_range)
				Wander();

			if (Vector3.Distance(transform.position, player.transform.position) <= attack_range
			                          && !is_attacking && attack_timer <= 0)
	        	StartCoroutine(attack());
		}
		else{
			stun_time -= Time.deltaTime;
			motor.inputMoveDirection = Vector3.zero;
		}

		if (attack_timer > 0)
			attack_timer -= Time.deltaTime;

		if (hp <= 0 && !isDead){
			isDead = true;
			StartCoroutine(die());
		}
	}

	IEnumerator die(){
		this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		sprite_plane.GetComponent<MeshRenderer>().material.color = Color.red;
		PlaySound("die");
		yield return new WaitForSeconds(1.0f);
		Destroy(this.gameObject);
	}

	IEnumerator attack(){
		motor.inputMoveDirection = Vector3.zero;
		motor.movement.velocity = Vector3.zero;
		is_attacking = true;
		yield return new WaitForSeconds(0.4f);
		if (attackHitBox.GetComponent<MeleeAttackBox>().detected_colliders.Contains(player))
			player.GetComponent<PlayerController>().TakeHit(damage);

		PlaySound("shoot");
		yield return new WaitForSeconds(0.5f);
		attack_timer = attack_rate;
		is_attacking = false;
	}

	void Basic_Move(){
		if (Vector3.Distance(transform.position, player.transform.position) <= attack_range)
			motor.inputMoveDirection = transform.forward * 0.1f;
		else{
			transform.LookAt(player.transform.position);
			motor.inputMoveDirection = transform.forward;
		}
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
		PlaySound("hit");

		Vector3 floating_pos = Camera.main.WorldToViewportPoint(transform.position);
		SpawnFloatPoints(dam, floating_pos.x, floating_pos.y);
	}

	void SpawnFloatPoints(float points, float x, float y){
		x = Mathf.Clamp(x, 0.05f, 0.95f);
		y = Mathf.Clamp(y, 0.05f, 0.9f);
		Transform gui = (Transform)Instantiate(floatingHitPoints, new Vector3(x, y, 0), Quaternion.identity);
		gui.GetComponent<FloatingDamage>().calling_transform = this.transform;
		gui.GetComponent<GUIText>().text = points.ToString();
	}

	public void PlaySound(string name){
		if (name == "spawn")
			GetComponent<AudioSource>().PlayOneShot(sound_spawn);
		else if (name == "hit")
			GetComponent<AudioSource>().PlayOneShot(sound_hit);
		else if (name == "shoot")
			GetComponent<AudioSource>().PlayOneShot(sound_attack);
		else if (name == "die")
			GetComponent<AudioSource>().PlayOneShot(sound_death);
	}
}
