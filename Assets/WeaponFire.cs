using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour {

	public Texture2D basicSpriteSheet;
	public Texture2D specialZSpriteSheet;
	public Texture2D idleSprite;
	public int basicFrames;
	public int specialZFrames;
	public float animationSpeed;
	private bool running;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	public void PlayBasicAnimation(float speed){
		animationSpeed = speed - 0.1f;
		if (running)
			resetAnimation();
		
		StartCoroutine("playAnim", new object[2]{basicSpriteSheet, basicFrames});
	}

	public void PlaySpecialZAnimation(float speed){
		animationSpeed = speed - 0.1f;
		if (running)
			resetAnimation();

		StartCoroutine("playAnim", new object[2]{specialZSpriteSheet, specialZFrames});
	}

	void resetAnimation(){
		StopCoroutine("playAnim");
		GetComponent<WeaponSway>().midpoint = -0.21f;
		this.renderer.material.mainTexture = idleSprite;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
		renderer.material.SetTextureScale("_MainTex", new Vector2(1,1));
		running = false;
	}

	IEnumerator playAnim(object[] args){
		running = true;
		Texture2D spriteSheet = (Texture2D)args[0];
		float frames = (int)args[1];
		Vector2 size = new Vector2(1.0f / frames, 1);
		Vector2 offset = new Vector2(0, 0);
		this.renderer.material.mainTexture = spriteSheet;
		GetComponent<WeaponSway>().midpoint = -0.19f;

		for (int i = 0; i < frames; i++){
			offset = new Vector2(i * size.x, 0);
			renderer.material.SetTextureOffset("_MainTex", offset);
			renderer.material.SetTextureScale("_MainTex", size);		
			yield return new WaitForSeconds(animationSpeed/frames);
		}

		GetComponent<WeaponSway>().midpoint = -0.21f;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
		renderer.material.SetTextureScale("_MainTex", new Vector2(1,1));
		this.renderer.material.mainTexture = idleSprite;
		running = false;
	}

}
