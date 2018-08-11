using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable<float> {

private Rigidbody2D PlayerRigidBody;
private Vector3 velocity=Vector3.zero;
private float horizontalMove;
private float verticalMove;

public float defaultPlayerSpeed = 4f;
public float playerspeed = 4f;

public float punchDamage = 5f;
public GameObject hitEffect;

public float health = 100f;
public float energy = 0f;


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
private bool punchCharging;
private Vector2 punchChargeDirection;
private bool punching;
private float punchCooldown;
private Vector2 punchDirecion;

private GameObject punchCollider;


	// Use this for initialization
	void Start () {
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        punchCharge =0f;
        Dashing = false;
        Dodging = false;
		NewPos = transform.position;
        animator = GetComponent<Animator>();
        punchCollider = transform.Find("punchCollider").gameObject;
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
        } else if (Dashing) {
            PlayerRigidBody.freezeRotation = true;
        } else {
            PlayerRigidBody.freezeRotation = false;
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
                animator.SetTrigger("PunchUnleash");
                //Charge punch release
            //transform.

        }
	
        //PlaYer movement
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
            PlayerRigidBody.velocity = 30*punchCharge * dashDirection;
			dashCooldown -= Time.fixedDeltaTime;
            if(dashCooldown <= 0){
                punchCharge=0;
                Dashing=false;
                playerspeed=defaultPlayerSpeed;
                animator.SetBool("PunchCharge", false);
            }
        }

        if(Dodging){
            PlayerRigidBody.velocity = 35 * DodgeDirection;
			dodgeCooldown -= Time.fixedDeltaTime;
            if(dodgeCooldown <= 0){
                Dodging=false;
            }

        }


        if(LeftClick == true){
            //if(!punching){
                animator.SetBool("Punching", true);
                punching = true;
            //}
        }

        if(RightClick == true){
            //charge punch attack
			punchCharge += 2*Time.fixedDeltaTime;
            playerspeed = 1.2f;
            animator.SetBool("PunchCharge", true);
        }

	}


    private void FixedUpdate() {

    }

    public void Punch() {
        Collider2D[] contacts = new Collider2D[2];
        ContactFilter2D filter = new ContactFilter2D();
        punchCollider.GetComponent<Collider2D>().OverlapCollider(filter.NoFilter(), contacts);


        foreach(Collider2D collision in contacts) {
            if(collision != null && collision.gameObject.GetComponent<IDamageable<float>>() != null) {
                Instantiate(hitEffect, new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z - 1f), Quaternion.identity);
                IDamageable<float> damagable = collision.gameObject.GetComponent<IDamageable<float>>();
                damagable.Damage(punchDamage);
            }
        }
        Debug.Log("FINISH PUNCH");
        punching = false;
        animator.SetBool("Punching", false);
    }

    public void ResetPunchState() {
        punching = false;
        animator.SetBool("Punching", false);
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
    public void Damage(float damageTaken) {
        health -= damageTaken;
    }
    

}
