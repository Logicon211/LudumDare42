﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteWizard : MonoBehaviour {
	public AudioSource audiosource;
	private float MoveUp;
	private float MoveRight;
	private float castTimer;
	private float TimerReset;
	private bool DuringCast;
	public Rigidbody2D wizardBody;
	public Rigidbody2D wizardHands;
	private float wizardSpeedHori;
	private float wizardSpeedVert;
	private float SpellStage;
	public GameObject NormalFace;
	public GameObject AngerFace;
	public GameObject wizardHandsHolder;
	public SpriteRenderer RightHandFull;
	public SpriteRenderer RightHandHalf;
	public SpriteRenderer LeftHandFull;
	public SpriteRenderer LeftHandHalf;
	public SpriteRenderer RightHandFist;
	public SpriteRenderer LeftHandFist;


	// Use this for initialization
	void Start () {
		MoveUp = 1f;
		MoveRight = 1f;
		castTimer = 10f;
		TimerReset = 10f;
		wizardSpeedHori = 2f;
		wizardSpeedVert = 1f;
		SpellStage = 0;
		AngerFace.SetActive(false);
		RightHandFist.enabled = false;
		LeftHandFist.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if(castTimer < 0){
			CastSpell();
			castTimer -= TimerReset;
			TimerReset -= 0.5f;
		}

		if(transform.position.x > 12){
			MoveRight = -1f;
		}
		else if(transform.position.x < -12){
			MoveRight = 1f;
		}
		
		if(transform.position.y > 20){
			MoveUp = -1f;
		}
		else if(transform.position.y < 14){
			MoveUp = 1f;
		}

		if(!DuringCast){
			
			castTimer -= Time.fixedDeltaTime;
			Move();
		}
		if(DuringCast){
			if(SpellStage ==0){
				if(transform.position.y < -2){
					transform.position = new Vector2(0,-2);
					wizardBody.velocity = new Vector2(0,7);
					wizardHands.velocity = new Vector2(0,7);
					NormalFace.SetActive(false);
					AngerFace.SetActive(true);
					SpellStage =1;
					Debug.Log("SpellStage 1");
				}	
			}
			else if(SpellStage ==1){
				if(transform.position.y > 20){
					wizardBody.velocity = new Vector2(0,0);
				}
				if(wizardHandsHolder.transform.position.y > 22){
					wizardHands.velocity = new Vector2(0,0);
					LeftHandHalf.sortingOrder = 3;
					RightHandHalf.sortingOrder = 3;
				}
				if(wizardHandsHolder.transform.position.y > 22 && transform.position.y > 20){
					SpellStage =2;
					wizardHands.velocity = new Vector2(0,-7);
					wizardBody.velocity = new Vector2(0,7);
					Debug.Log("SpellStage 2");
				}
			}
			else if(SpellStage ==2){
				if(transform.position.y > 32){
					wizardBody.velocity = new Vector2(0,0);
				}
				
				if(wizardHandsHolder.transform.position.y < 16){
					wizardHands.velocity = new Vector2(0,0);
				}
				if(wizardHandsHolder.transform.position.y <= 16 && transform.position.y >= 32){
					SpellStage =3;
					wizardHands.velocity = new Vector2(0,5);
					//wizardBody.velocity = new Vector2(0,7);
					Debug.Log("SpellStage 3");
				}

			}
			else if (SpellStage ==3){
				if(transform.position.y > 32){
					wizardBody.velocity = new Vector2(0,0);
				}
				
				if(wizardHandsHolder.transform.position.y > 23){
					wizardHands.velocity = new Vector2(0,0);
				}
				if(wizardHandsHolder.transform.position.y >= 23 && transform.position.y >= 32){
					SpellStage =4;
					wizardHands.velocity = new Vector2(0,5);
					//wizardBody.velocity = new Vector2(0,7);
					Debug.Log("SpellStage 4");
				}

			}
			else if (SpellStage ==4){
				if(wizardHandsHolder.transform.position.y > 30){
					wizardHands.velocity = new Vector2(0,-20);
					RightHandFist.enabled = true;
					RightHandFull.enabled = false;
					RightHandHalf.enabled = false;
					LeftHandFist.enabled = true;
					LeftHandFull.enabled = false;
					LeftHandHalf.enabled = false;
					RightHandFist.sortingOrder=3;
					LeftHandFist.sortingOrder=3;
					Debug.Log("SpellStage 5");
					SpellStage =5;
				}
			}
			else if(SpellStage ==5){
				if(wizardHandsHolder.transform.position.y < 20){
					Camera.main.GetComponent<Camera_controller>().CameraShake();
					wizardHands.velocity = new Vector2(0,3);
					SpellStage =6;
					
					Debug.Log("SpellStage 6");
				}
			}
			else if(SpellStage ==6){
				if(wizardHandsHolder.transform.position.y > 23){
					wizardHands.velocity = new Vector2(0,-8);
					wizardBody.velocity = new Vector2(0,-8);
					SpellStage =7;
					RightHandFist.sortingOrder=0;
					LeftHandFist.sortingOrder=0;
					RightHandFull.sortingOrder=0;
					RightHandHalf.sortingOrder=0;
					LeftHandFull.sortingOrder=0;
					LeftHandHalf.sortingOrder=0;
					
					Debug.Log("SpellStage 7");
				}
			}
			else if(SpellStage ==7){
				if(wizardHandsHolder.transform.position.y < 8){
					wizardHands.velocity = new Vector2(0,0);
					//SpellStage =7;
					RightHandFist.enabled = false;
					RightHandFull.enabled = true;
					RightHandHalf.enabled = true;
					LeftHandFist.enabled = false;
					LeftHandFull.enabled = true;
					LeftHandHalf.enabled = true;
					DuringCast = false;
					
					Debug.Log("Spell done");
				}
			}

		}


	}

	void CastSpell(){
		DuringCast = true;
		wizardBody.velocity = new Vector2(0f, -10f);
		SpellStage = 0;
	}

	void Move(){
		Vector2 NewPos = new Vector2(wizardSpeedHori*MoveRight, wizardSpeedVert*MoveUp);
        wizardBody.velocity = NewPos;



	}

}