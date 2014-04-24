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

	public float piercingShotCooldown;
	private float piercingShotTimer;
	public float suppressiveFireCooldown;
	private float suppressiveFireTimer;

	// Use this for initialization
	void Start () {
		decalManager = GameObject.Find("DecalHolder").GetComponent<DecalManager>();
		weaponObject = GameObject.Find("WeaponPlane");
		autoAimPosition = GameObject.Find("autoAim").transform;
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

		if (suppressiveFireTimer <= 0 && Input.GetKeyDown(KeyCode.C)){
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

	IEnumerator BasicShot(){
		Vector3 direction = autoAimPosition.forward;
		RaycastHit hit;

		
		weaponObject.GetComponent<WeaponFire>().PlayBasicAnimation(0.3f);
		direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
		                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));
		if(Physics.Raycast(weaponPosition, direction, out hit, range)){
			if(hit.transform.tag == "enemy")
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
			if(hit.transform.tag == "enemy")
				hit.transform.SendMessage("TakeHit", new object[3]{basic_damage, basic_knockback, basic_stun});
			else
				decalManager.AddDecal(hit);
		}

		audio.pitch = Random.Range(0.9f, 1.1f); 
		audio.PlayOneShot(basicFireSound);

	}

	IEnumerator PiercingShot(){
				weaponObject.GetComponent<WeaponFire>().PlaySpecialZAnimation(0.3f);

		Vector3 direction = autoAimPosition.forward;
		RaycastHit[] hit = Physics.RaycastAll(weaponPosition, direction, range);

		if(hit.Length > 0){
			for (int i = 0; i < hit.Length; i++){
				if(hit[i].transform.tag == "enemy")
					hit[i].transform.SendMessage("TakeHit", new object[3]{specialZ_damage, specialZ_knockback, specialZ_stun});
				else
					decalManager.AddDecal(hit[i]);
			}
		}

		audio.pitch = Random.Range(0.9f, 1.2f);
		audio.PlayOneShot(piercingFireSound);

		yield return null;
	}

	IEnumerator SuppressiveFire(){
		RaycastHit hit;		
		weaponObject.GetComponent<WeaponFire>().PlaySpecialCAnimation(0.8f);

		for (int i = 0; i < 6; i++){
			Vector3 direction = transform.forward;
			direction = new Vector3(direction.x + (Random.Range(-spreadFactor, spreadFactor)),
			                        direction.y + (Random.Range(-spreadFactor, spreadFactor)),
			                        direction.z + (Random.Range(-spreadFactor, spreadFactor)));

			if(Physics.Raycast(weaponPosition, direction, out hit, range)){
				if(hit.transform.tag == "enemy")
					hit.transform.SendMessage("TakeHit", new object[3]{specialC_damage, specialC_knockback, specialC_stun});
				else
					decalManager.AddDecal(hit);
			}
						
			audio.pitch = Random.Range(0.9f, 1.1f); 
			audio.PlayOneShot(suppressiveFireSound);
			yield return new WaitForSeconds(0.095f);
		}


		yield return null;
	}
}
