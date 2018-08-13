using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

private Rigidbody2D PlayerRigidBody;
private Vector3 velocity=Vector3.zero;
private float horizontalMove;
private float verticalMove;

public float defaultPlayerSpeed = 4f;
public float playerspeed = 4f;
public float playerChargeSpeed = 1.4f;

public GameObject hitEffect;

public float maxHealth = 100f;
public float health = 100f;
public float energy = 0f;
public float maxPunchCharge = 3f;
public float punchDamage = 5f;
public float maxDashDamage = 20f;
public float dashDamage = 0f;
public float dashCooldownTime = 0.3f;
public float bulletVelocity = 10f;
public float lazerDrain = 0.5f;
public float speedBoostAmount = 1f;

public Slider healthSlider;
public Slider energySlider;
public Slider chargeSlider;
public GameObject bullet;
public AudioClip hurtSound;
public AudioClip shotgunSound;
public AudioClip shieldHitSound;

public bool Dashing;
public GameObject gameControllerObject;
private GameManager gameManager;

private bool LeftClickDown;
private bool LeftClick;
private bool RightClick;
private bool RightClickRelease;
private bool RightClickDown;
private float punchCharge;
Vector2 direction;
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

private bool usingShotgun = false;
private bool usingLazer = false;
private bool isShielded = false;
private bool usingWrench = false;
public float speedBoostTime = 0f;

private GameObject punchCollider;
private AudioSource AS;
private GameObject lazer;
private GameObject shield;
private GameObject particles;

private Transform shootPosition;

private int BASE_ANIMATION_LAYER = 0;
private int SHOTGUN_ANIMATION_LAYER = 1;
private int LAZER_ANIMATION_LAYER = 2;

	// Use this for initialization
	void Start () {
		if (gameControllerObject == null) {
			gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		}
		gameManager = gameControllerObject.GetComponent<GameManager> ();
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        punchCharge =0f;
        Dashing = false;
        Dodging = false;
		NewPos = transform.position;
        animator = GetComponent<Animator>();
        punchCollider = transform.Find("punchCollider").gameObject;
        AS = GetComponent<AudioSource>();
        shootPosition = transform.Find("ShootPosition");
        lazer = transform.Find("Lazer").gameObject;
        shield = transform.Find("Shield").gameObject;
        particles = transform.Find("ParticleEffect").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
 
        //Get Movement unputs
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        //Get Action inputs
        LeftClickDown = Input.GetButtonDown("Fire1");
        LeftClick = Input.GetButton("Fire1");
        RightClick = Input.GetButton("Fire2");
        RightClickRelease = Input.GetButtonUp("Fire2");
        RightClickDown = Input.GetButtonDown("Fire2");
        SpacebarDown = Input.GetButtonDown("Jump");

    	//Player rotation to mouse          
    	Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		direction = Camera.main.ScreenToWorldPoint (mousePos) - transform.position;
		direction.Normalize ();

        if(speedBoostTime > 0f) {
            PlayerRigidBody.mass = 100;
            particles.SetActive(true);
            speedBoostTime -= Time.deltaTime;
			if(RightClick) {
                playerspeed = playerChargeSpeed + (speedBoostAmount/2);
            } else {
                playerspeed = defaultPlayerSpeed + speedBoostAmount;
            }
        } else {
            PlayerRigidBody.mass = 1;
            if(!Dashing) {
                particles.SetActive(false);
            }
            if(RightClick) {
                playerspeed = playerChargeSpeed;
            } else {
                playerspeed = defaultPlayerSpeed;
            }
            speedBoostTime = 0f;
        }


    

        if (direction != Vector2.zero && !Dashing) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        } else if (Dashing) {
            PlayerRigidBody.freezeRotation = true;
        } else {
            PlayerRigidBody.freezeRotation = false;
        }

        // if(SpacebarDown == true && !Dodging){
        //     Dodging = true;
        //     dodgeCooldown = 0.25f;
		// 	DodgeDirection = NewPos;
		// 	DodgeDirection.Normalize ();
        // }

        if(RightClickRelease == true){
			dashDirection = direction;
            Dashing = true;
            dashCooldown=dashCooldownTime;   //punchCharge;
            //animator.SetTrigger("PunchUnleash");
            animator.SetBool("PunchUnleashed", true);
                //Charge punch release
            //transform.

        }
	
        //PlaYer movement
        if(!Dashing && !Dodging){
            NewPos = Vector3.Normalize(new Vector2(horizontalMove*playerspeed, verticalMove*playerspeed));
            PlayerRigidBody.velocity = playerspeed * NewPos;
            if(horizontalMove != 0f || verticalMove != 0f){
                animator.SetBool("isWalking", true);
            }
            else{
                animator.SetBool("isWalking", false);
            }

        }
        if(Dashing){
            particles.SetActive(true);
            if(speedBoostTime <= 0) {
                PlayerRigidBody.velocity = 30*punchCharge * dashDirection;
            } else {
                PlayerRigidBody.velocity = 38*punchCharge * dashDirection;
            }
			dashCooldown -= Time.fixedDeltaTime;
            PlayerRigidBody.mass = 100;
            if(dashCooldown <= 0){
                particles.SetActive(false);
                PlayerRigidBody.mass = 1;
                punchCharge=0;
                Dashing=false;
                //playerspeed=defaultPlayerSpeed;
                animator.SetBool("PunchCharge", false);
                animator.SetBool("PunchUnleashed", false);
                dashDamage = 0;
            }
        }

        // if(Dodging){
        //     PlayerRigidBody.velocity = 35 * DodgeDirection;
		// 	dodgeCooldown -= Time.fixedDeltaTime;
        //     if(dodgeCooldown <= 0){
        //         Dodging=false;
        //     }

        // }


		if(LeftClickDown && !RightClick && !Dashing  && !gameManager.IsPaused()){
            if(usingShotgun) {
                ShootShotgun();
                animator.SetTrigger("ShootGun");
            } else if (usingLazer) {
                //Hold for left click held down below
            } else {
                animator.SetBool("Punching", true);
                punching = true;
            }
        }

		if(LeftClick && !RightClick && !Dashing && usingLazer  && !gameManager.IsPaused()) {
            lazer.SetActive(true);
            energy -= lazerDrain;

            if(energy <= 0f) {
                energy = 0f;
                RunOutOfPickupEnergy();
            }
        } else {
            lazer.SetActive(false);
        }

        if(RightClickDown) {
            //Reset charge on the first frame right click being down
            punchCharge = 0f;
        }

        if(RightClick){
            //charge punch attack
			punchCharge += 2*Time.fixedDeltaTime;
            //playerspeed = 1.2f;
            animator.SetBool("PunchCharge", true);

            if(punchCharge > maxPunchCharge){
                punchCharge = maxPunchCharge;
            }

            float punchPercentage = (punchCharge / maxPunchCharge);
            dashDamage = maxDashDamage * punchPercentage;

            chargeSlider.gameObject.SetActive(true);
            chargeSlider.value = (punchCharge / maxPunchCharge) * 100;
        } else {
            chargeSlider.gameObject.SetActive(false);
        }

        //Update Health Slider
        healthSlider.value = health;
        energySlider.value = energy;

	}


    private void FixedUpdate() {

    }

    public void Punch() {
        Collider2D[] contacts = new Collider2D[5];
        ContactFilter2D filter = new ContactFilter2D();
        punchCollider.GetComponent<Collider2D>().OverlapCollider(filter.NoFilter(), contacts);


        foreach(Collider2D collision in contacts) {
            if(collision != null && collision.gameObject.GetComponent<IDamageable<float>>() != null) {
                Instantiate(hitEffect, new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z - 1f), Quaternion.identity);
                IDamageable<float> damagable = collision.gameObject.GetComponent<IDamageable<float>>();
                damagable.Damage(punchDamage);
            }
        }
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
        if(!isShielded) {
            health -= damageTaken;
            AS.PlayOneShot(hurtSound);
        } else {
            isShielded = false;
            //TODO: Play shield sound?
            AS.PlayOneShot(shieldHitSound);
            shield.SetActive(false);
        }
    }

    public float GetHealth() {
        return health;
    }

    public void RunOutOfPickupEnergy() {
        animator.SetLayerWeight(BASE_ANIMATION_LAYER, 100f);
        animator.SetLayerWeight(SHOTGUN_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(LAZER_ANIMATION_LAYER, 0f);

        usingShotgun = false;
        usingLazer = false;
        usingWrench = false;
    }

    public void PickupShotgun() {
        animator.SetLayerWeight(BASE_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(SHOTGUN_ANIMATION_LAYER, 100f);
        animator.SetLayerWeight(LAZER_ANIMATION_LAYER, 0f);

        usingShotgun = true;
        usingLazer = false;
        usingWrench = false;

        energySlider.maxValue = 3;
        energy = 3;
    }

    public void PickupLazer() {
        animator.SetLayerWeight(BASE_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(SHOTGUN_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(LAZER_ANIMATION_LAYER, 100f);

        usingShotgun = false;
        usingLazer = true;
        usingWrench = false;

        energySlider.maxValue = 100f;
        energy = 100f;
    }

    public void PickupHealth() {
        health += 15f;
        if(health > maxHealth) {
            health = maxHealth;
        }
    }

    public void PickupShield() {
        isShielded = true;
        shield.SetActive(true);
    }

    public void PickupSpeedBoost() {
        speedBoostTime = 5f;
    }

    public void PickupWrench() {
        animator.SetLayerWeight(BASE_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(SHOTGUN_ANIMATION_LAYER, 0f);
        animator.SetLayerWeight(LAZER_ANIMATION_LAYER, 100f);

        usingShotgun = false;
        usingLazer = false;
        usingWrench = true;

        energySlider.maxValue = 100f;
        energy = 100f;
    }

    public void ShootShotgun() {
        AS.PlayOneShot(shotgunSound);

        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(Vector3.forward * 90);

        GameObject projectileLaunched = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched2 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched3 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched4 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched5 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched6 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched7 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched8 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;
        GameObject projectileLaunched9 = Instantiate (bullet, shootPosition.position, rotation) as GameObject;

        projectileLaunched.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched2.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched3.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched4.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched5.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched6.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched7.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched8.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched9.transform.Rotate (0, 0, Random.Range (-15, 15));
        projectileLaunched.GetComponent<Rigidbody2D> ().velocity = projectileLaunched.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched2.GetComponent<Rigidbody2D> ().velocity = projectileLaunched2.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched3.GetComponent<Rigidbody2D> ().velocity = projectileLaunched3.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched4.GetComponent<Rigidbody2D> ().velocity = projectileLaunched4.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched5.GetComponent<Rigidbody2D> ().velocity = projectileLaunched5.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched6.GetComponent<Rigidbody2D> ().velocity = projectileLaunched6.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched7.GetComponent<Rigidbody2D> ().velocity = projectileLaunched7.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched8.GetComponent<Rigidbody2D> ().velocity = projectileLaunched8.transform.right * (bulletVelocity + Random.Range (-2, 2));
        projectileLaunched9.GetComponent<Rigidbody2D> ().velocity = projectileLaunched9.transform.right * (bulletVelocity + Random.Range (-2, 2));

        energy -= 1;
        if(energy <= 0) {
            RunOutOfPickupEnergy();
        }
    }
    

}
