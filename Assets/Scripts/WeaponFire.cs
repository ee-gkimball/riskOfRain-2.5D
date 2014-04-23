using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour {

	public Texture2D basicSpriteSheet;
	public Texture2D specialZSpriteSheet;
	public Texture2D[] basicSprites = new Texture2D[6];
	public Texture2D[] specialZSprites = new Texture2D[4];
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
		
		StartCoroutine("playAnim", new object[1]{basicSprites});
	}

	public void PlaySpecialZAnimation(float speed){
		animationSpeed = speed - 0.1f;
		if (running)
			resetAnimation();

		//StartCoroutine("playAnim", new object[2]{specialZSpriteSheet, specialZFrames});
	}

	void resetAnimation(){
		StopCoroutine("playAnim");
		this.renderer.material.mainTexture = idleSprite;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0,0));
		renderer.material.SetTextureScale("_MainTex", new Vector2(1,1));
		running = false;
	}

	IEnumerator playAnim(object[] args){
		running = true;
		Texture2D[] sprites = (Texture2D[])args[0];

		for (int i = 0; i < sprites.Length; i++){	
			this.renderer.material.mainTexture = sprites[i];
			yield return new WaitForSeconds(animationSpeed/sprites.Length);
		}
		this.renderer.material.mainTexture = idleSprite;
		running = false;
	}

}
