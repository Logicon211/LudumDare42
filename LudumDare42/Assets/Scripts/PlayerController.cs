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
private Vector3 DodgeDirection;
private Vector3 NewPos;
private float dodgeCooldown;


	// Use this for initialization
	void Start () {
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        punchCharge =0f;
        Dashing = false;
        Dodging = false;
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
    mousePos.z = -(transform.position.x - Camera.main.transform.position.x);
    direction = Vector3.Normalize(Camera.main.ScreenToWorldPoint(mousePos) - transform.position);

    

        if (direction != Vector2.zero && !Dashing) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        if(SpacebarDown == true && !Dodging){
            Dodging = true;
            dodgeCooldown = 0.25f;
            DodgeDirection = NewPos;
        }

        if(RightClickRelease == true){
            if(punchCharge > 3){
                punchCharge =3f;
            }
                dashDirection=direction;
                Dashing = true;
                dashCooldown=0.3f;   //punchCharge;
                Debug.Log(Dashing);
            //PlayerRigidBody.velocity = 10*punchCharge * direction;
            //Charge punch release
            //transform.

        }






	}


    private void FixedUpdate() {
        //Plater movement
        if(!Dashing && !Dodging){
            NewPos = new Vector3(horizontalMove*playerspeed, verticalMove*playerspeed,0);
            PlayerRigidBody.velocity = playerspeed * NewPos;
        }
        if(Dashing){
            PlayerRigidBody.velocity = 10*punchCharge * dashDirection;
            dashCooldown -= Time.deltaTime;
            if(dashCooldown <= 0){
                punchCharge=0;
                Dashing=false;
                playerspeed=2f;
            }
        }

        if(Dodging){
            PlayerRigidBody.velocity = 10 * DodgeDirection;
            dodgeCooldown -= Time.deltaTime;
            if(dodgeCooldown <= 0){
                Dodging=false;
            }

        }


        if(LeftClick == true){
            //punch attack!
        }

        if(RightClick == true){
            //charge punch attack
            punchCharge += 2*Time.deltaTime;
            Debug.Log(punchCharge);
            playerspeed = 1.2f;
        }



    }

    

}
