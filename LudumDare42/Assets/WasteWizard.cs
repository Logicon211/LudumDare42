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
	
	public SpriteRenderer NormalFaceSprite;
	public SpriteRenderer AngerFaceSprite;
	public GarbageSpawnController garbageSpawner;
	public GameObject garbageBall;
	public bool firstphase;
	private float ChosenSpell;
	private int fistCounter;
	private Transform PlayerTransform;
	private int NumSpellsCast;
	private float VulnerableCooldown;
	private bool vulnMode;
	private float movementCooldown;
	private bool handSwitch;
	public GameObject LeftHandSmash;
	public GameObject RightHandSmash;
	private float wizardHealth;
	public AudioClip CastFist;
	public AudioClip CastHailOfTrash;
	public AudioClip CastGarbageBall;

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
		firstphase = true;
		fistCounter =0;
		garbageBall.SetActive(false);
		vulnMode = false;
		wizardHealth=100;
		
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		NumSpellsCast=0;

	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangePhase();
        }
	}

	void FixedUpdate() {
		if(firstphase){
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
					wizardBody.velocity = new Vector2(0,8);
					wizardHands.velocity = new Vector2(0,8);
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
					audiosource.Play();
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
					
					float playerX = PlayerTransform.position.x;
					float playerY = PlayerTransform.position.y;
					garbageSpawner.SpawnRandomGarbageAtLocation(playerX,playerY,true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
					
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
					NormalFace.SetActive(true);
					AngerFace.SetActive(false);					
					Debug.Log("Spell done");
				}
			}

		}
		}
		//FINAL PHASE
		else{

			if(castTimer < 0){
				CastBossSpell();
				castTimer -= TimerReset;
				TimerReset -= 0.5f;
		}


			if(NumSpellsCast ==2){

				if(SpellStage==0){
					Debug.Log("into SpellStage 0");
					transform.position = Vector3.Lerp(transform.position, new Vector3(0,14.4f,0), 2 *Time.deltaTime); 
					wizardHandsHolder.transform.position = Vector3.Lerp(wizardHandsHolder.transform.position, new Vector3(0.25f, 14.45f, 0.2109375f), 2 *Time.deltaTime);

					Debug.Log("wizard position: " + transform.position);
					Debug.Log("Hands position: " + wizardHandsHolder.transform.position);
					movementCooldown -= Time.fixedDeltaTime;
					if(movementCooldown < 0){
						SpellStage=1;
						VulnerableCooldown = 8f;
						Debug.Log("Moved into vuln area");
						//Make wizard and hands able to be hurt
					}
				}
				else if (SpellStage ==1){
					VulnerableCooldown-=Time.fixedDeltaTime;
					if(VulnerableCooldown < 0){
						wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
						Debug.Log("vulnerability complete");
						SpellStage =2;
						NumSpellsCast=0;

					RightHandFist.enabled = true;
					RightHandFull.enabled = false;
					RightHandHalf.enabled = false;
					LeftHandFist.enabled = true;
					LeftHandFull.enabled = false;
					LeftHandHalf.enabled = false;
					LeftHandHalf.sortingOrder = 0;
					RightHandHalf.sortingOrder = 0;
					vulnMode =false;
					}
				}
			}
				

			
			

			if(!DuringCast && !vulnMode){

				if(transform.position.x > 12){
				MoveRight = -1f;
				}
				else if(transform.position.x < -12){
					MoveRight = 1f;
				}
				
				if(transform.position.y > 34){
					MoveUp = -1f;
				}
				else if(transform.position.y < 29){
					MoveUp = 1f;
				}
			
				castTimer -= Time.fixedDeltaTime;
				Move();
			}

			if(DuringCast){
				
				
					//garbage ball
					if(ChosenSpell <7){
						
						if(SpellStage==0){
							wizardHands.velocity = new Vector2(0,-8);
							LeftHandFist.sortingOrder=0;
							RightHandFist.sortingOrder=0;
							SpellStage=1;
						}
						else if (SpellStage ==1){
							if(wizardHandsHolder.transform.position.y < -6){
								garbageBall.SetActive(true);
								wizardHands.velocity = new Vector2(0,25);
								Debug.Log("SpellStage 2");
								SpellStage =2;
								audiosource.clip = CastGarbageBall;
								audiosource.Play();
							}
						}
							else if(SpellStage ==2){
								if(wizardHandsHolder.transform.position.y > 50){
									garbageBall.SetActive(false);
									wizardHands.velocity = new Vector2(0,0);
									SpellStage =3;
									Debug.Log("SpellStage 3, garbage ball cast complete");

									float playerX = PlayerTransform.position.x;
									float playerY = PlayerTransform.position.y;
									garbageSpawner.SpawnAtLocation(1,playerX,playerY,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+10,playerY,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+5f,playerY+5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX,playerY+10f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-5f,playerY+5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-10f,playerY,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-5f,playerY-5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX,playerY-10f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+7.5f,playerY-7.5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+15,playerY,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+7.5f,playerY+7.5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX,playerY+15f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-7.5f,playerY+7.5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-15f,playerY,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX-7.5f,playerY-7.5f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX,playerY-15f,true);
									garbageSpawner.SpawnRandomGarbageAtLocation(playerX+7.5f,playerY-7.5f,true);
									garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
									garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
									garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
									garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
									garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								
									DuringCast = false;
									NormalFace.SetActive(true);
									AngerFace.SetActive(false);		
									NumSpellsCast++;
									if(NumSpellsCast==2){
										RightHandFist.enabled = false;
										RightHandFull.enabled = true;
										RightHandHalf.enabled = true;
										LeftHandFist.enabled = false;
										LeftHandFull.enabled = true;
										LeftHandHalf.enabled = true;
										LeftHandHalf.sortingOrder = 3;
										RightHandHalf.sortingOrder = 3;
										vulnMode = true;
										SpellStage=0;
										movementCooldown = 5;
									}
									else{								
										wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
									}
								
								}
						}

					}

					//Hail of trash
					else if(ChosenSpell < 14){
						if(SpellStage==0){
							wizardHands.velocity = new Vector2(0,5);
							SpellStage=1;
						}
						else if (SpellStage ==1){
							if(wizardHandsHolder.transform.position.y > 30){
								wizardHands.velocity = new Vector2(0,-20);
								Debug.Log("SpellStage 2");
								SpellStage =2;
								audiosource.clip = CastHailOfTrash;
								audiosource.Play();
							}
						}
						else if(SpellStage ==2){
							if(wizardHandsHolder.transform.position.y < 20){
								Camera.main.GetComponent<Camera_controller>().CameraShake();
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
								
								wizardHands.velocity = new Vector2(0,0);
								SpellStage =3;
								//wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
								Debug.Log("SpellStage 3, hail of trash complete");
								
								DuringCast = false;
								NormalFace.SetActive(true);
								AngerFace.SetActive(false);	
								NumSpellsCast++;	
									if(NumSpellsCast==2){
										RightHandFist.enabled = false;
										RightHandFull.enabled = true;
										RightHandHalf.enabled = true;
										LeftHandFist.enabled = false;
										LeftHandFull.enabled = true;
										LeftHandHalf.enabled = true;
										LeftHandHalf.sortingOrder = 3;
										RightHandHalf.sortingOrder = 3;
										vulnMode =true;
										SpellStage=0;
										movementCooldown = 5;
									}
									else{								
										wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
									}
							}
						}

					}//Hail of trash end

					//Fist
					else if(ChosenSpell < 20){

						if(SpellStage==0){
							wizardHands.velocity = new Vector2(0,10);
							SpellStage=1;
						}
						else if (SpellStage ==1){
							if(wizardHandsHolder.transform.position.y > 40){
								audiosource.clip = CastFist;
								audiosource.Play();
								Debug.Log("SpellStage 2");
								SpellStage =2;
								fistCounter =0;
								StartCoroutine("Fist", 1);
								StartCoroutine("Fist", 3);
								StartCoroutine("Fist", 5);
								wizardHands.velocity = new Vector2(0,0);
							}
						}
						else if(SpellStage ==2){
							if(fistCounter==3){
								SpellStage =3;
								//wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
								Debug.Log("SpellStage 3, fist spell complete");
								
								DuringCast = false;
								NormalFace.SetActive(true);
								AngerFace.SetActive(false);	
								NumSpellsCast++;
								if(NumSpellsCast==2){
									RightHandFist.enabled = false;
									RightHandFull.enabled = true;
									RightHandHalf.enabled = true;
									LeftHandFist.enabled = false;
									LeftHandFull.enabled = true;
									LeftHandHalf.enabled = true;
									LeftHandHalf.sortingOrder = 3;
									RightHandHalf.sortingOrder = 3;
									vulnMode =true;
									SpellStage=0;
									movementCooldown = 5;
								}
								else{
									wizardHandsHolder.transform.localPosition = new Vector3(0.25f, -6.4f, 0.2109375f);
								}
							}
							
						}

					}


















					}

				if(SpellStage ==0){
					if(transform.position.y < -2){
						transform.position = new Vector2(0,-2);
						wizardBody.velocity = new Vector2(0,8);
						wizardHands.velocity = new Vector2(0,8);
						NormalFace.SetActive(false);
						AngerFace.SetActive(true);
						SpellStage =1;
						Debug.Log("SpellStage 1");
					}	
				}
			}

		}//Else for final phase



	

	public void ChangePhase(){
		firstphase=false;
		castTimer=10f;
		TimerReset = 10f;
		RightHandFist.enabled = true;
		RightHandFull.enabled = false;
		RightHandHalf.enabled = false;
		LeftHandFist.enabled = true;
		LeftHandFull.enabled = false;
		LeftHandHalf.enabled = false;
		RightHandFist.sortingOrder=3;
		LeftHandFist.sortingOrder=3;
		DuringCast =false;
		AngerFaceSprite.sortingOrder=3;
		NormalFaceSprite.sortingOrder=3;
}

	void CastSpell(){
		DuringCast = true;
		wizardBody.velocity = new Vector2(0f, -10f);
		SpellStage = 0;
		
	}

	void CastBossSpell(){
		DuringCast = true;
		wizardBody.velocity = new Vector2(0f, 0f);
		SpellStage = 0;
		Random.seed = System.DateTime.Now.Millisecond;
		ChosenSpell = Random.Range(1f, 20f);
		//ChosenSpell = 5;
		NormalFace.SetActive(false);
		AngerFace.SetActive(true);	
	}

	void Move(){
		Vector2 NewPos = new Vector2(wizardSpeedHori*MoveRight, wizardSpeedVert*MoveUp);
        wizardBody.velocity = NewPos;
		wizardHands.velocity = NewPos;
	}

	public void damageWizard(int damageIn){
		wizardHealth-=damageIn;

		if(wizardHealth <= 0){
			//End of game?
		}

	}


	IEnumerator Fist(int waitTime) {			
		yield return new  WaitForSeconds(waitTime);  // or however long you want it to wait
			float playerX = PlayerTransform.position.x;
			float playerY = PlayerTransform.position.y;

			if(handSwitch){
				LeftHandSmash.GetComponent<WizardFistController>().DropFistAtLocation(playerX,playerY);
				handSwitch = false;
			}
			else{
				RightHandSmash.GetComponent<WizardFistController>().DropFistAtLocation(playerX,playerY);
				handSwitch = true;
			}

			garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
			garbageSpawner.SpawnRandomGarbageAtRandomLocation(true);
			Camera.main.GetComponent<Camera_controller>().CameraShake();
			fistCounter++;
		
	}
}
