using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float cooldownBetweenSpawns = 4f;

	public bool spawn = true;
	private float currentTimeBetweenSpawns;

	private GameObject player;
	private PlayerController playerController;
	public static GameManager instance = null;

	private GameObject cameraObject;
	private AudioListener listener;
	private WaveSpawnManager spawnManager;

	private bool paused = false;
	private bool inCutScene = false;
	private bool victory = false;
	private bool loss = false;

	private int enemyCount = 0;
	private int bossCount = 0;
	private int currentLevel = 0;
	private int maxLevel = 0;

	private bool bossMessageDone;
	private int bossMessageCount;
	private string[] cutscenes = {"Pre-BossScene", "SecondBossScene"};

	private int nextCutsceneIndex = 0;
	public AudioSource AS;
	public AudioClip AC1;


	private void Awake() {
		//Check if instance already exists
		// if (instance == null)
		// 	//if not, set instance to this
		// 	instance = this;
		// //If instance already exists and it's not this:
		// else if (instance != this)
		// 	//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
		// 	Destroy(gameObject);    
		// //Sets this to not be destroyed when reloading scene
		// DontDestroyOnLoad(gameObject);
		//Get a component reference to the attached BoardManager script
		//boardScript = GetComponent<BoardManager>();
		//Call the InitGame function to initialize the first level 
		//InitGame();
		spawnManager = GetComponent<WaveSpawnManager>();
		if(spawn){
			maxLevel = spawnManager.GetNumOfLevels();
			currentTimeBetweenSpawns = cooldownBetweenSpawns;
		}
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		cameraObject = GameObject.FindWithTag("MainCamera");
		listener = cameraObject.GetComponent<AudioListener>();
		playerController = player.GetComponent<PlayerController>();

		if(spawn){
			enemyCount = spawnManager.GetNumOfEnemiesOnLevel(currentLevel);
			bossCount = spawnManager.GetNumOfBossesOnLevel(currentLevel);
			spawnManager.SpawnWave(currentLevel);
		}
		bossMessageDone=false;
		bossMessageCount=0;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForWaveChange();
		if (player != null)
			CheckGameOver();
	}

	
	
	// Logic for checking for a wave change
	private void CheckForWaveChange() {
		if (enemyCount <= 0 && bossCount <= 0 && spawn) {
			currentTimeBetweenSpawns -= Time.deltaTime;
			
			if(!bossMessageDone && bossMessageCount <2){
				if(spawnManager.GetNumOfBossesOnLevel(currentLevel+2) >0){
				
					bossMessageDone = true;
					if(bossMessageCount ==0){
						bossMessageCount++;
						AS.Play();
						Debug.Log("playing AC1");
					
					}
					else if(bossMessageCount ==1){
						AS.clip = AC1;
						AS.Play();
						bossMessageCount++;
						Debug.Log("playing AC2");
					}
				}

			}	
			if (currentTimeBetweenSpawns <= 0f) {
				currentLevel++;
				if (currentLevel <= maxLevel) {
					enemyCount = spawnManager.GetNumOfEnemiesOnLevel(currentLevel);
					bossCount = spawnManager.GetNumOfBossesOnLevel(currentLevel);
					
					// Enemy count under 0 indicates a cutscene
					if (enemyCount < 0) {
						
						bossMessageDone =false;
						StartCutScene();
					}
					else {
						SpawnWave(currentLevel);
						currentTimeBetweenSpawns = cooldownBetweenSpawns;
					}
				}
				else if (currentLevel > maxLevel) {
					Debug.Log("VICTORY");
					Victory();
				}
			}
		}
	}

	private void SpawnWave(int currentLevel) {
		spawnManager.SpawnWave(currentLevel);
	}

	public void StartCutScene() {
		Debug.Log("START SCENE INDEX: " + nextCutsceneIndex);
		SceneManager.LoadScene(cutscenes[nextCutsceneIndex], LoadSceneMode.Additive);
		listener.enabled = false; // Disabling the main cameras audio listener so that we have exactly one listener
		PauseGame();
	}

	public void StopCutScene() {
		Debug.Log("END SCENE INDEX: " + nextCutsceneIndex);
		SceneManager.UnloadSceneAsync(cutscenes[nextCutsceneIndex]);
		nextCutsceneIndex++;
		listener.enabled = true;
		SetEnemyCountToZero();
		UnPauseGame();
	}

	public void CheckGameOver() {
		if (SceneManager.GetActiveScene().name != "GameOverScreen" && !victory){
			if (playerController.GetHealth() <= 0f){
				loss = true;
				SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
			}
		}
	}

	public void Victory() {
		if (SceneManager.GetActiveScene().name != "VictoryScene" && !loss) {
			victory = true;
			SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
		}

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

	public void DecreaseBossCount() {
		if (bossCount > 0)
			bossCount--;
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

	// 1 - intro screen
	// 2 - start of gameplay
	public void Reset() {
		Debug.Log("RESET");
		victory = false;
		loss = false;
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
		currentLevel = 0;
		enemyCount = spawnManager.GetNumOfEnemiesOnLevel(currentLevel);
		bossCount = spawnManager.GetNumOfBossesOnLevel(currentLevel);
		spawnManager.SpawnWave(currentLevel);
	}

	public void LoadScene(int sceneIndex) {
		Debug.Log("SCENE INDEX " + sceneIndex);
		if (sceneIndex == 0) {
			SceneManager.LoadScene(sceneIndex);
			Destroy(gameObject);
		}
		else if (sceneIndex == 2) {
			Reset();
		}
		else {
			SceneManager.LoadScene(sceneIndex);
		}
	}

	public bool IsPaused () {
		return paused;
	}
}
