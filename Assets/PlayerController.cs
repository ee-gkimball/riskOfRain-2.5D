using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Texture2D weaponSprite;
	public Rect weaponPosition;

	// Use this for initialization
	void Start () {
	
		weaponPosition = new Rect(Screen.width/2-((weaponSprite.width*3)/2),
		           Screen.height-weaponSprite.height*2, 
		           weaponSprite.width*3, weaponSprite.height*3);
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnGUI(){

	}
}
