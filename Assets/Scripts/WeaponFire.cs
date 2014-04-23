using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour {

	public Texture2D basicSpriteSheet;
	public Texture2D specialZSpriteSheet;
	public Texture2D[] basicSprites = new Texture2D[6];
	public Texture2D[] specialZSprites = new Texture2D[4];
	public Texture2D[] specialCSprites = new Texture2D[8];
	public Texture2D idleSprite;
	public float basicMidpoint;
	public float specialZMidpoint;
	public float specialCMidpoint;
	public Vector3 basicXScale;
	public Vector3 specialZScale;
	public Vector3 specialCScale;
	public float idleMidpoint;
	public int basicFrames;
	public int specialZFrames;
	public int specialCFrames;
	public float animationSpeed;
	public float basicSpeed;
	public float specialZSpeed;
	public float specialCSpeed;
	private bool running;

	// Use this for initialization
	void Start () {
	
		idleMidpoint = GetComponent<WeaponSway>().midpoint;
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	public void PlayBasicAnimation(float speed){
		if (running)
			resetAnimation();

		animationSpeed = speed;
		GetComponent<WeaponSway>().midpoint = basicMidpoint;
		this.transform.localScale = basicXScale;
		StartCoroutine("playAnim", new object[1]{basicSprites});
	}

	public void PlaySpecialZAnimation(float speed){
		if (running)
			resetAnimation();

		animationSpeed = speed;
		GetComponent<WeaponSway>().midpoint = specialZMidpoint;
		transform.localScale = specialZScale;
		StartCoroutine("playAnim", new object[1]{specialZSprites});
	}

	public void PlaySpecialCAnimation(float speed){
		if (running)
			resetAnimation();
		
		animationSpeed = speed;
		GetComponent<WeaponSway>().midpoint = specialCMidpoint;
		transform.localScale = specialCScale;
		StartCoroutine("playAnim", new object[1]{specialCSprites});
	}

	void resetAnimation(){
		StopCoroutine("playAnim");
		this.renderer.material.mainTexture = idleSprite;
		running = false;
	}

	IEnumerator playAnim(object[] args){
		running = true;
		Texture2D[] sprites = (Texture2D[])args[0];

		for (int i = 0; i < sprites.Length; i++){	
			this.renderer.material.mainTexture = sprites[i];
			yield return new WaitForSeconds(animationSpeed/sprites.Length);
		}
		GetComponent<WeaponSway>().midpoint = idleMidpoint;
		this.renderer.material.mainTexture = idleSprite;
		transform.localScale = basicXScale;
		running = false;
	}

}
