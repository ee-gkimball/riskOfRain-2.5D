using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Texture2D weaponSprite;
	public Transform autoAimPosition;
	public Vector3 weaponPosition = new Vector3(0,0,0);
	public GameObject weaponObject;
	public AudioClip basicFireSound;
	public AudioClip piercingFireSound;
	public AudioClip suppressiveFireSound;
	bool canShoot;
	bool isShooting;
	public bool isFiring;
	public float fireRate;
	public int range;
	public float basic_damage;
	public float basic_knockback;
	public float basic_stun;
	public float specialZ_damage;
	public float specialZ_knockback;
	public float specialZ_stun;
	public float specialC_damage;
	public float specialC_knockback;
	public float specialC_stun;
	float shootTime;
	public GameObject bulletDecal;
	public DecalManager decalManager;
	public float spreadFactor = 0.02f;
	public Light fireLight;

	public float piercingShotCooldown;
	private float piercingShotTimer;
	public float suppressiveFireCooldown;
	private float suppressiveFireTimer;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		decalManager = GameObject.Find("DecalHolder").GetComponent<DecalManager>();
		weaponObject = GameObject.Find("WeaponPlane");
		fireLight = GameObject.Find("fireLight").GetComponent<Light>();
		autoAimPosition = GameObject.Find("autoAim").transform;
		shootTime = 60.0f / fireRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
			isShooting = true;
		if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetMouseButtonUp(0))
			isShooting = false;

		if (piercingShotTimer <= 0 && (Input.GetKeyDown(KeyCode.Z)) || Input.GetMouseButtonDown(1)){
			StartCoroutine(PiercingShot());
			piercingShotTimer = piercingShotCooldown;
		}

		if (piercingShotTimer > 0)
			piercingShotTimer -= Time.deltaTime;

		if (suppressiveFireTimer <= 0 && (Input.GetKeyDown(KeyCode.C)) || Input.GetMouseButtonDown(2)){
			StartCoroutine(SuppressiveFire());
			suppressiveFireTimer = suppressiveFireCooldown;
		}
		
		if (suppressiveFireTimer > 0)
			suppressiveFireTimer -= Time.deltaTime;
	}

	void FixedUpdate(){
		weaponPosition = new Vector3(transform.position.x,
		                             transform.position.y + 0.7f,
		                             transform.position.z);

		if (isShooting){
			if (canShoot){
				StartCoroutine(BasicShot());
				shootTime = 60.0f / fireRate;
				canShoot = false;
			}
		}

		if (shootTime > 0)
			shootTime -= Time.fixedDeltaTime;
		else
			canShoot = true;
	}

	void OnGUI(){

	}

	void OnMouseDown() {
		Screen.lockCursor = true;
	}

	IEnumerator BasicShot(){
		Vector3 direction = autoAimPosition.forward;
		RaycastHit hit;

		isFiring = false;
		weaponObject.GetComponent<WeaponFire>().PlayBasicAnimation(0.3f);
		weaponObject.GetComponent<WeaponFire>().PlayMuzzleFlash(2, 0.11f, 0.01f, Color.yellow);

		direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));
		if(Physics.Raycast(weaponPosition, direction, out hit, range)){
			if(hit.transform.tag == "enemy" && !hit.transform.gameObject.GetComponent<Entity>().isDead)
				hit.transform.SendMessage("TakeHit", new object[3]{basic_damage, basic_knockback, basic_stun});
			else
				decalManager.AddDecal(hit);
		}

		audio.pitch = Random.Range(0.9f, 1.1f); 
		audio.PlayOneShot(basicFireSound);

		yield return new WaitForSeconds(0.11f);

		direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));
		if(Physics.Raycast(weaponPosition, direction, out hit, range)){
			if(hit.transform.tag == "enemy" && !hit.transform.gameObject.GetComponent<Entity>().isDead)
				hit.transform.SendMessage("TakeHit", new object[3]{basic_damage, basic_knockback, basic_stun});
			else
				decalManager.AddDecal(hit);
		}

		audio.pitch = Random.Range(0.9f, 1.1f); 
		audio.PlayOneShot(basicFireSound);
		isFiring = false;

	}

	IEnumerator PiercingShot(){
		isFiring = true;

		weaponObject.GetComponent<WeaponFire>().PlaySpecialZAnimation(0.3f);
		weaponObject.GetComponent<WeaponFire>().PlayMuzzleFlash(1, 0.11f, 0.1f, Color.cyan);

		Vector3 direction = autoAimPosition.forward;
		RaycastHit[] hit = Physics.RaycastAll(weaponPosition, direction, range);

		if(hit.Length > 0){
			for (int i = 0; i < hit.Length; i++){
				if(hit[i].transform.tag == "enemy" && !hit[i].transform.gameObject.GetComponent<Entity>().isDead)
					hit[i].transform.SendMessage("TakeHit", new object[3]{specialZ_damage, specialZ_knockback, specialZ_stun});
				else
					decalManager.AddDecal(hit[i]);
			}
		}

		audio.pitch = Random.Range(0.9f, 1.2f);
		audio.PlayOneShot(piercingFireSound);
		
		yield return new WaitForSeconds(0.4f);
		isFiring = false;
	}

	IEnumerator SuppressiveFire(){
		isFiring = true;
		RaycastHit hit;
		
		Vector3 direction = autoAimPosition.forward;
		weaponObject.GetComponent<WeaponFire>().PlaySpecialCAnimation(0.8f);
		weaponObject.GetComponent<WeaponFire>().PlayMuzzleFlash(6, 0.075f, 0.02f, Color.yellow);

		for (int i = 0; i < 6; i++){
			direction = autoAimPosition.forward;
			direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
			                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
			                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));

			if(Physics.Raycast(weaponPosition, direction, out hit, range)){
				if(hit.transform.tag == "enemy" && !hit.transform.gameObject.GetComponent<Entity>().isDead)
					hit.transform.SendMessage("TakeHit", new object[3]{specialC_damage, specialC_knockback, specialC_stun});
				else
					decalManager.AddDecal(hit);
			}
						
			audio.pitch = Random.Range(0.9f, 1.1f); 
			audio.PlayOneShot(suppressiveFireSound);
			yield return new WaitForSeconds(0.085f);
		}

		yield return new WaitForSeconds(0.3f);
		isFiring = false;
	}
}
