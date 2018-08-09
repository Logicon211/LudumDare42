using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.


	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	private bool sceneEnding = false;
	public int sceneToLoadIndex;

	void Awake ()
	{
		// Set the texture so that it is the the size of the screen and covers it.
		GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}


	void Update ()
	{
		foreach (Touch touch in Input.touches) {
			sceneEnding = true;
		}

		if (Input.anyKey) {
			sceneEnding = true;
			//Application.LoadLevel();
		}

		// If the scene is starting...
		if (sceneStarting) {
			// ... call the StartScene function.
			StartScene ();
		}

		else if(sceneEnding){
			StartCoroutine("EndScene");
			//EndScene();
		}
	}


	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
	}


	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
	}


	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();

		// If the texture is almost clear...
		if(GetComponent<GUITexture>().color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			GetComponent<GUITexture>().color = Color.clear;
			GetComponent<GUITexture>().enabled = false;

			// The scene is no longer starting.
			sceneStarting = false;
		}
	}


	//	public void EndScene ()
	//	{
	//		// Make sure the texture is enabled.
	//		GetComponent<GUITexture>().enabled = true;
	//		
	//		// Start fading towards black.
	//		FadeToBlack();
	//		
	//		// If the screen is almost black...
	//		if(GetComponent<GUITexture>().color.a >= 0.95f)
	//			// ... reload the level.
	//			Application.LoadLevel(sceneToLoad);
	//	}

	IEnumerator EndScene() {			
		yield return new  WaitForSeconds(3);  // or however long you want it to wait

		// Make sure the texture is enabled.
		GetComponent<GUITexture>().enabled = true;

		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (GetComponent<GUITexture> ().color.a >= 0.95f) {
			// ... reload the level.
			//Application.LoadLevel(sceneToLoad);
			//PersistentGameObject PGO = GameObject.FindGameObjectWithTag ("PersistentObject").GetComponent<PersistentGameObject> ();
			//PersistentGameObject.// (player.transform.Find ("RifleWeapon").gameObject.GetComponent<TrackMouse> ().weapon);
			LoadingScreenManager.LoadScene (sceneToLoadIndex);
		}
	}
}