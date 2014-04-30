using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public MusicBox musicBox;

	public float game_time;

	public bool spawnEnemies;
	public bool spawnTimer;
	public float min_spawn_time;
	public float max_spawn_time;
	public float timeToNextSpawn;
	public List<GameObject> enemies = new List<GameObject>();
	public GameObject[] availableBaddies = new GameObject[1];
	public GameObject enemy_holder;
	public float max_spawn_x;
	public float max_spawn_z;

	// Use this for initialization
	void Start () {
		musicBox = GameObject.Find("MusicBox").GetComponent<MusicBox>();

		if (Application.loadedLevelName == "mainmenu")
			musicBox.fadeInMusic("menu");
		if (Application.loadedLevelName == "level1")
			musicBox.fadeInMusic("level1");
	}
	
	// Update is called once per frame
	void Update () {
		game_time += Time.deltaTime;
		timeToNextSpawn -= Time.deltaTime;

		if (timeToNextSpawn <= 0 ){
			SpawnEnemy();
			timeToNextSpawn = Random.Range(min_spawn_time, max_spawn_time);
		}
	}

	void SpawnEnemy(){
		int random_enemy = Random.Range(0, availableBaddies.Length);
		GameObject new_enemy = (GameObject)Instantiate(availableBaddies[random_enemy], 
		                                               new Vector3(Random.Range(-max_spawn_x, max_spawn_x), 2, Random.Range(-max_spawn_z, max_spawn_z)),
		                                               Quaternion.identity);
		enemies.Add(new_enemy);
		new_enemy.GetComponent<Entity>().PlaySound("spawn");
		new_enemy.transform.parent = enemy_holder.transform;
	}
}
