using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private GameObject player;
	private PlayerController playerController;
	public static GameManager instance = null;

	private bool paused = false;

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
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z) && !paused)
        {
            SceneManager.LoadScene("ChadsSceneForTestingSceneTransitionWithoutLosingShit", LoadSceneMode.Additive);
			PauseGame();
        }
		if (Input.GetKeyDown(KeyCode.X) && paused)
        {
    		SceneManager.UnloadSceneAsync("ChadsSceneForTestingSceneTransitionWithoutLosingShit");
			UnPauseGame();
        }
	}

	public void GameOver() {
		SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
	}

	public void PauseGame() {
		paused = true;
		Time.timeScale = 0;	
	}

	public void UnPauseGame() {
		paused = false;
		Time.timeScale = 1;
	}

}
