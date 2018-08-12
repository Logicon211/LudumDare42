﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float cooldownBetweenSpawns = 4f;
	private float currentTimeBetweenSpawns;

	private GameObject player;
	private PlayerController playerController;
	public static GameManager instance = null;

	private GameObject cameraObject;
	private AudioListener listener;
	private WaveSpawnManager spawnManager;

	private bool paused = false;
	private bool inCutScene = false;

	private int enemyCount = 0;
	private int currentLevel = 0;
	private int maxLevel = 0;

	private string[] cutscenes = {"Pre-BossScene", "SecondBossScene"};

	private void Awake() {
		//Check if instance already exists
		if (instance == null)
			//if not, set instance to this
			instance = this;
		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		//Get a component reference to the attached BoardManager script
		//boardScript = GetComponent<BoardManager>();
		//Call the InitGame function to initialize the first level 
		//InitGame();
		spawnManager = GetComponent<WaveSpawnManager>();
		maxLevel = spawnManager.GetNumOfLevels();
		currentTimeBetweenSpawns = cooldownBetweenSpawns;
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		cameraObject = GameObject.FindWithTag("MainCamera");
		listener = cameraObject.GetComponent<AudioListener>();
		playerController = player.GetComponent<PlayerController>();

		enemyCount = spawnManager.GetNumOfEnemiesOnLevel(currentLevel);
		spawnManager.SpawnWave(currentLevel);
	}
	
	// Update is called once per frame
	void Update () {
		CheckForWaveChange();
		CheckGameOver();
	}

	// Logic for checking for a wave change
	private void CheckForWaveChange() {
		if (enemyCount == 0) {
			currentTimeBetweenSpawns -= Time.deltaTime;
			if (currentTimeBetweenSpawns <= 0f) {
				currentLevel++;

				if (currentLevel <= maxLevel) {
					enemyCount = spawnManager.GetNumOfEnemiesOnLevel(currentLevel);
					// Enemy count under 0 indicates a cutscene
					if (enemyCount < 0) {
						StartCutScene();
					}
					else {
						SpawnWave(currentLevel);
						currentTimeBetweenSpawns = cooldownBetweenSpawns;
					}
				}
			}
		}
	}

	private void SpawnWave(int currentLevel) {
		spawnManager.SpawnWave(currentLevel);
	}

	public void StartCutScene() {
		SceneManager.LoadScene(cutscenes[(enemyCount * -1) - 1], LoadSceneMode.Additive);
		listener.enabled = false; // Disabling the main cameras audio listener so that we have exactly one listener
		PauseGame();
	}

	public void StopCutScene() {
		SceneManager.UnloadSceneAsync(cutscenes[(enemyCount * -1) - 1]);
		listener.enabled = true;
		SetEnemyCountToZero();
		UnPauseGame();
	}

	public void CheckGameOver() {
		if (SceneManager.GetActiveScene().name != "GameOverScreen"){
			if (playerController.GetHealth() <= 0f)
				SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
		}
	}

	public void Victory() {
		SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
	}

	public void PauseGame() {
		paused = true;
		Time.timeScale = 0;	
	}

	public void UnPauseGame() {
		paused = false;
		Time.timeScale = 1;
	}

	public void DecreaseEnemyCount() {
		if (enemyCount > 0)
			enemyCount--;
	}

	//Used By cutscenes to indicate they are done
	public void SetEnemyCountToZero() {
		enemyCount = 0;
	}
	
	//Utility Methods
	
	// -1 indicates the integer is not in the array
	private int FindInArray(int[] arr, int find) {
		for (int i = 0; i < arr.Length; i++) {
			if (find == arr[i])
				return i;
		}
		return -1;
	}
}
