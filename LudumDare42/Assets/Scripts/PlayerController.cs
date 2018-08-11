using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

private Rigidbody2D PlayerRigidBody;
private Vector3 velocity=Vector3.zero;
private float horizontalMove;
private float verticalMove;
public float playerspeed = 2f;

private bool LeftClick;
private bool RightClick;
private bool RightClickRelease;
private float punchCharge;
Vector2 direction;
private bool Dashing;
private Vector2 dashDirection;
private float dashCooldown;
private bool SpacebarDown;
private bool Dodging;
private Vector2 DodgeDirection;
private Vector2 NewPos;
private float dodgeCooldown;
Animator animator;


	// Use this for initialization
	void Start () {
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        punchCharge =0f;
        Dashing = false;
        Dodging = false;
		NewPos = transform.position;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
 
        //Get Movement unputs
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        //Get Action inputs
        LeftClick = Input.GetButtonDown("Fire1");
        RightClick = Input.GetButton("Fire2");
        RightClickRelease = Input.GetButtonUp("Fire2");
        SpacebarDown = Input.GetButtonDown("Jump");

    	//Player rotation to mouse          
    	Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		direction = Camera.main.ScreenToWorldPoint (mousePos) - transform.position;
		direction.Normalize ();


    

        if (direction != Vector2.zero && !Dashing) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        if(SpacebarDown == true && !Dodging){
            Dodging = true;
            dodgeCooldown = 0.25f;
			DodgeDirection = NewPos;
			DodgeDirection.Normalize ();
        }

        if(RightClickRelease == true){
            if(punchCharge > 3){
                punchCharge =3f;
            }
			dashDirection = direction;
                Dashing = true;
                dashCooldown=0.3f;   //punchCharge;
                //Charge punch release
            //transform.

        }
	






	}


    private void FixedUpdate() {
        //Plater movement
        if(!Dashing && !Dodging){
            NewPos = new Vector2(horizontalMove*playerspeed, verticalMove*playerspeed);
            PlayerRigidBody.velocity = playerspeed * NewPos;
            if(horizontalMove != 0f || verticalMove != 0f){
                animator.SetBool("isWalking", true);
            }
            else{
                animator.SetBool("isWalking", false);
            }

        }
        if(Dashing){
            PlayerRigidBody.velocity = 10*punchCharge * dashDirection;
			dashCooldown -= Time.fixedDeltaTime;
            if(dashCooldown <= 0){
                punchCharge=0;
                Dashing=false;
                playerspeed=2f;
            }
        }

        if(Dodging){
            PlayerRigidBody.velocity = 20 * DodgeDirection;
			dodgeCooldown -= Time.fixedDeltaTime;
            if(dodgeCooldown <= 0){
                Dodging=false;
            }

        }


        if(LeftClick == true){
            //punch attack!
        }

        if(RightClick == true){
            //charge punch attack
			punchCharge += 2*Time.fixedDeltaTime;
            playerspeed = 1.2f;
        }



    }

    /*
        Takes an int corresponding to the current powerup craig will get
         0 - nothing
         1 - Laser
         2 - Shotgun
         3 - Health
         4 - Wrench?
         5 - Speed Boost
         6 - Shiny Teeth
    */
    public void SetPowerUp(int powerupValue) {
        //Do Stuff
    }

    

}
