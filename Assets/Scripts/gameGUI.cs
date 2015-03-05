using UnityEngine;
using System.Collections;

public class gameGUI : MonoBehaviour {

	public Texture2D hud_texture;
	public Texture2D hud_base;
	public Texture2D healthXp_base;
	public Texture2D health_bar;
	public Texture2D basicAbility_icon;
	public Texture2D specialZAbility_icon;
	public Texture2D specialXAbility_icon;
	public Texture2D speicalCAbility_icon;
	public Texture2D cooldown_overlay;
	public bool draw_hud;

	public float old_health_ratio;
	public float current_old_HR;
	public float health_ratio;
	public float drawn_health_ratio;

	public PlayerController playerData;
	public Game gameData;
	public GUISkin skin;

	// Use this for initialization
	void Start () {
		playerData = GameObject.Find("Player").GetComponent<PlayerController>();
		gameData = GameObject.Find("GameManager").GetComponent<Game>();
		drawn_health_ratio = 1;
	}
	
	// Update is called once per frame
	void Update () {
		health_ratio = playerData.hp / playerData.hpMax;
	}

	void OnGUI(){
		GUI.skin = skin;
		if (draw_hud){
			//Overall hud
			//GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), hud_texture);
			GUI.DrawTexture(new Rect(Screen.width*0.33229f, Screen.height*0.9314f,
			                         (Screen.width*0.33333f) * health_ratio, Screen.height*0.0333f), health_bar);
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), healthXp_base);

			//Ability Icons
			GUI.DrawTexture(new Rect(Screen.width*0.403125f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f),
			                         basicAbility_icon);

			GUI.skin.label.fontSize = (int)(0.05*Screen.width);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;

			GUI.DrawTexture(new Rect(Screen.width*0.451041f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f),
			                specialZAbility_icon);
			if (playerData.specialZ_timer > 0){
				GUI.DrawTexture(new Rect(Screen.width*0.451041f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f), cooldown_overlay);
				GUI.Label(new Rect(Screen.width*0.451041f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f), 
				          ((int)playerData.specialZ_timer).ToString());
			}

			GUI.DrawTexture(new Rect(Screen.width*0.498958f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f),
			                         specialXAbility_icon);

			GUI.DrawTexture(new Rect(Screen.width*0.546875f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f),
			                         speicalCAbility_icon);
			if (playerData.specialC_timer > 0){
				GUI.DrawTexture(new Rect(Screen.width*0.546875f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f), cooldown_overlay);
				GUI.Label(new Rect(Screen.width*0.546875f, Screen.height * 0.827777f, Screen.width*0.0375f, Screen.width*0.0375f), 
				          ((int)playerData.specialC_timer).ToString());
			}


			//Time and Level Labels
			GUI.skin.label.fontSize = (int)(0.05*Screen.width);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(Screen.width*0.337f, Screen.height*0.809259f, Screen.width*0.0479166f, Screen.width*0.0479166f), 
			          playerData.current_level.ToString());

			int minutes = Mathf.FloorToInt(gameData.game_time / 60f);
			int seconds = Mathf.FloorToInt(gameData.game_time - minutes * 60);

			GUI.skin.label.fontSize = (int)(0.065*Screen.width);
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label(new Rect(Screen.width*0.897f, -Screen.height* 0.007f, Screen.width*0.06f, 50), string.Format("{00:00}", minutes));
			GUI.skin.label.fontSize = (int)(0.03*Screen.width);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(Screen.width*0.955f, Screen.height * 0.015f, Screen.width*0.035f, 50), string.Format(":{00:00}", seconds));
		}
	}
}
