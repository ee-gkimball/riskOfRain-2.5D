using UnityEngine;
using System.Collections;

public class MusicBox : MonoBehaviour {
	public bool musicActive;
	public float audioVolume;
	public float maxVolume;
	public float fadeSpeed;
	public bool fadeOut;
	public bool fadeIn;
	
	public string currentTrack;
	public AudioClip menuMusic;
	public AudioClip level1;
	
	/// <summary>
	/// Fades in the music.
	/// (menu, break, assemblyLine)
	/// </summary>
	/// <param name="track">Track you wish to play</param>
	public void fadeInMusic(string track){
		if (GetComponent<AudioSource>().isPlaying){
			StartCoroutine(swapMusic(track));
		}
		else{
			GetComponent<AudioSource>().clip = getTrack(track);
			fadeIn = true;
			fadeOut = false;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		}
	}
	
	public void fadeOutMusic(){
		fadeIn = false;
		fadeOut = true;
	}
	
	public AudioClip getTrack(string track){
		if (track == "menu")
			return menuMusic;
		else if (track == "level1")
			return level1;
		else
			return menuMusic;
	}
	
	IEnumerator swapMusic(string track){
		fadeIn = false;
		fadeOut = true;
		yield return new WaitForSeconds(fadeSpeed);
		GetComponent<AudioSource>().clip = getTrack(track);
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
		fadeOut = false;
		fadeIn = true;
	}
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);

	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn){
			if (GetComponent<AudioSource>().volume < maxVolume)
				GetComponent<AudioSource>().volume += fadeSpeed * Time.deltaTime;
		}
		
		if (fadeOut){
			if (GetComponent<AudioSource>().volume > 0)
				GetComponent<AudioSource>().volume -= fadeSpeed * Time.deltaTime;
			else
				GetComponent<AudioSource>().Stop();
		}
	}
}
