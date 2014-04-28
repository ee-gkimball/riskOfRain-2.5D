using UnityEngine;
using System.Collections;

public class gameGUI : MonoBehaviour {

	public Texture2D hud_texture;
	public bool draw_hud;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (draw_hud){
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), hud_texture);
		}

	}
}
