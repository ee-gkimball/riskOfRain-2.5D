using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Texture2D weaponSprite;
	public Vector3 weaponPosition = new Vector3(0,0,0);
	public GameObject weaponObject;
	public AudioClip basicFireSound;
	public AudioClip piercingFireSound;
	bool canShoot;
	bool isShooting;
	public float fireRate;
	public int range;
	float shootTime;
	public GameObject bulletDecal;
	public DecalManager decalManager;
	public float spreadFactor = 0.02f;

	public float piercingShotCooldown;
	private float piercingShotTimer;

	// Use this for initialization
	void Start () {
		decalManager = GameObject.Find("DecalHolder").GetComponent<DecalManager>();
		weaponObject = GameObject.Find("WeaponPlane");
		shootTime = 60.0f / fireRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftControl))
			isShooting = true;
		if (Input.GetKeyUp(KeyCode.LeftControl))
			isShooting = false;

		if (piercingShotTimer <= 0 && Input.GetKeyDown(KeyCode.Z)){
			StartCoroutine(PiercingShot());
			piercingShotTimer = piercingShotCooldown;
		}

		if (piercingShotTimer > 0)
			piercingShotTimer -= Time.deltaTime;
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

	IEnumerator BasicShot(){
		Vector3 direction = transform.forward;
		RaycastHit hit;
		direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));
		if(Physics.Raycast(weaponPosition, direction, out hit, range)){
			decalManager.AddDecal(hit);
		}

		audio.pitch = Random.Range(0.9f, 1.1f); 
		audio.PlayOneShot(basicFireSound);
		yield return new WaitForSeconds(0.11f);

		direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));
		if(Physics.Raycast(weaponPosition, direction, out hit, range)){
			decalManager.AddDecal(hit);
		}

		audio.pitch = Random.Range(0.9f, 1.1f); 
		audio.PlayOneShot(basicFireSound);

		weaponObject.GetComponent<WeaponFire>().PlayBasicAnimation(60.0f / fireRate);
	}

	IEnumerator PiercingShot(){
		Vector3 direction = transform.forward;
		RaycastHit[] hit = Physics.RaycastAll(weaponPosition, direction, range);

		if(hit.Length > 0){
			for (int i = 0; i < hit.Length; i++){
				decalManager.AddDecal(hit[i]); 
				Debug.Log(hit[i].ToString());
			}
		}

		audio.pitch = Random.Range(0.9f, 1.2f);
		Debug.Log(audio.pitch);
		audio.PlayOneShot(piercingFireSound);
		weaponObject.GetComponent<WeaponFire>().PlaySpecialZAnimation(60.0f / fireRate);

		yield return null;
	}
}
