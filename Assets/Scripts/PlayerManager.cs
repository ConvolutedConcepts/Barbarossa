using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviour {

    public float speedX = 1;
    public float jumpHeight = 3;

    public GameObject player_Explosion;
    private GameObject instantiateExplosion;
    Transform playerPosition;


    private GameObject Player;
    //Bullet
    public GameObject rightBullet;
    public GameObject leftBullet;

    Transform firePos;

    public Text scoreLabel;

    bool facingRight, hasKey;
    float speed;

    bool leftKeyPressed;
    bool leftKeyReleased;

    bool jumpButtonPressed;
    bool jumpButtonReleased;

    bool rightKeyPressed;
    bool rightKeyReleased;

    bool isPlayerFrozen;
    bool needToZeroSpeed;

    Animator anim;
    Rigidbody2D rb;

    //Lives:
    public GameObject live1;
    public GameObject live2;
    public GameObject live3;
    //Sounds 
    public AudioClip jumpLaunchSound;
    public AudioClip jumpLandSound;
    public AudioClip gemPickedUp;
    public AudioClip gotSpecial;
    public AudioClip gotKey;
    public AudioClip walking;
    public AudioClip falling;
    public AudioClip dead;
    new AudioSource audio;

    private Vector3 spown;

    void Awake()
    {
        Player = gameObject;
    }

	// Use this for initialization
	void Start () {
		firePos = transform.FindChild ("FirePosition");
		spown = transform.position;
		audio = GetComponent<AudioSource>();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D>();

		facingRight = true;
		hasKey = false;

		isPlayerFrozen = false;
		needToZeroSpeed = false;

        rb.freezeRotation = true;
	}
		
	// Update is called once per frame
	void Update () {
		Jet();
		Flip ();
		MovePlayer (speed);

		leftKeyPressed = Input.GetKey(KeyCode.LeftArrow);
		leftKeyReleased = Input.GetKeyUp(KeyCode.LeftArrow);

		rightKeyPressed = Input.GetKey(KeyCode.RightArrow);
		rightKeyReleased = Input.GetKeyUp(KeyCode.RightArrow);

        jumpButtonPressed = Input.GetKeyDown(KeyCode.UpArrow);
		jumpButtonReleased = Input.GetKeyUp(KeyCode.UpArrow);

		playerPosition = Player.transform;

		if (Input.GetKeyDown (KeyCode.LeftControl) && Time.time > GameStatus.nextFire && GameStatus.isGunPickedUp) {
			Fire();
		}

		if (leftKeyPressed && !GameStatus.isJetPackOn) {
			WalkLeft();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			GoToMainMenu ();
		}

		if (rightKeyPressed && !GameStatus.isJetPackOn) {
			WalkRight();
		}

		if ((leftKeyReleased || rightKeyReleased) && !GameStatus.isJetPackOn) {
			LeftOrRightButtonGotReleased ();
		}
			
		if (jumpButtonPressed) {
            JumpButtonPressed ();
		}

		if (jumpButtonReleased) {
			JumpButtonReleased();
		}
	}

	void MovePlayer(float playerSpeed) {
		if (GameStatus.isJetPackOn) {
			speed = 0;
		}
		rb.velocity = new Vector3 (speed, rb.velocity.y, 0);
	}
	//Flip Player when Left arrow pressed
	void Flip() {
		if (speed > 0 && !facingRight || speed < 0 && facingRight) {
			if (needToZeroSpeed) {
				speed = 0;
				needToZeroSpeed = false;
			}
			facingRight = !facingRight;
			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
	}
		
	void GoToNextLevel() {
		if (GameStatus.currentLevel == 1) {
			SceneManager.LoadScene ("Level1_Complete");
		}
		if (GameStatus.currentLevel == 2) {
			SceneManager.LoadScene ("Level2_Complete");
		}
		if (GameStatus.currentLevel == 3) {
			SceneManager.LoadScene ("Level3_Complete");
		}
	}

	void RestartCurrentLevel() {

		Player.SetActive (false);
		speed = 0;
		GameStatus.isAltButtonPressed = false;
		GameStatus.isJetPackOn = false;
		rb.gravityScale = 1;
		rb.constraints = RigidbodyConstraints2D.None;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		isPlayerFrozen = false;
		transform.position = spown;
		Player.SetActive (true);

		//Spawn player with facing right
		Vector3 temp = transform.localScale;
		if (temp.x < 0) {
			needToZeroSpeed = true;
			speed = 1;
		}

	}

	void OnTriggerEnter2D(Collider2D c) {

		if (c.gameObject.tag == "monster_bullet") {
			actionIfDead ();
			Destroy (c.gameObject);
		}
	}

	//If we touched ground allow jump and set the player to Idle mode
	void OnCollisionEnter2D(Collision2D other) {

		if(other.gameObject.tag == "gun") {
            audio.PlayOneShot(gotSpecial, 0.8F);
			Destroy(other.gameObject);
			GameStatus.allowToFire = true;
			GameStatus.isGunPickedUp = true;
		}
			
		if (other.gameObject.tag == "gem") {
			audio.PlayOneShot(gemPickedUp, 0.8F);
			Destroy(other.gameObject);
			GameStatus.score += 100;
			scoreLabel.text = GameStatus.score.ToString ();
		}

		if (other.gameObject.tag == "enemy" || other.gameObject.tag == "hazard") {
			actionIfDead ();
		}

		if (other.gameObject.tag == "key") {
			//GameObject.Find ("Level1_C_Text").SetActive (false);
			audio.PlayOneShot(gotKey, 0.7F);
			hasKey = true;
			Destroy(other.gameObject);
		}

		if (other.gameObject.tag == "door" && hasKey == true) {
			GoToNextLevel ();
		}

        if (GameStatus.isGrounded)
        {
            audio.PlayOneShot(jumpLandSound, 0.7F);
        }
    }

	void actionIfDead() {
		//isPlayerAlive = false; //player will not get killed twice
		isPlayerFrozen = true;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

		audio.PlayOneShot(dead, 0.8F);
		instantiateExplosion = (GameObject)	Instantiate (player_Explosion, playerPosition.position, Quaternion.identity);
		Destroy (instantiateExplosion, 1.5f);
		GameStatus.lives -= 1;

		if (GameStatus.lives == 2) {
			live3.SetActive (false);
			Invoke("RestartCurrentLevel", 1.5f );
		}
		else if (GameStatus.lives == 1) {
			live2.SetActive (false);
			Invoke("RestartCurrentLevel", 1.5f );
		}
		else if (GameStatus.lives == 0) {
			live1.SetActive (false);
			Invoke("RestartCurrentLevel", 1.5f );
		}
		else if (GameStatus.lives < 0) {
			GameStatus.score = 0;
			GameStatus.lives = 3;
			GameStatus.currentLevel = 1;
			StartCoroutine (ExitToMainMenu());			
		}
	}

//	void loadLevel1() {
//		SceneManager.LoadScene ("Level1");
//	}

	IEnumerator ExitToMainMenu() {
		yield return new WaitForSeconds (2);
		SceneManager.LoadScene ("Main_Menu");
	}

	//Mobile UI Stuff
	public void WalkLeft() {
		if (!isPlayerFrozen && !GameStatus.isJetPackOn) {
			anim.SetInteger ("State", 2);
			speed = -speedX;
		}
	}

	public void WalkRight() {
		if (!isPlayerFrozen && !GameStatus.isJetPackOn) {
			anim.SetInteger ("State", 2);
			speed = speedX;
		}
	}

	public void LeftOrRightButtonGotReleased() {
		if (!isPlayerFrozen) {
			speed = 0;
			anim.SetInteger ("State", 0);
		}
	}

	public void JumpButtonReleased() {
		if (!isPlayerFrozen) {
			anim.SetInteger ("State", 0);
		}
	}

    public void JumpButtonPressed()
    {
        print("jump button pressed");
        print("groundedness"+GameStatus.isGrounded);
        if (GameStatus.isGrounded)
        {
            print("doing jump");
            audio.PlayOneShot(jumpLaunchSound, 0.7F);
            print("jump sound played");
            anim.SetInteger("State", 1);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse); //Old Way
            //rb.AddForce (new Vector2 (rb.velocity.x, jumpspeedY)); //another old way

        } else
        {
            print("jump while not grounded");
        }
    }

	public void GoToMainMenu() {
		SceneManager.LoadScene ("Main_Menu");
	}
		
	public void Fire() {
        if (Time.time > GameStatus.nextFire && GameStatus.isGunPickedUp && !isPlayerFrozen) {
			if (facingRight) {
				Instantiate (rightBullet, firePos.position, Quaternion.identity);
			}
			if (!facingRight) {
				Instantiate (leftBullet, firePos.position, Quaternion.identity);
			}
			GameStatus.nextFire = Time.time + GameStatus.fireRate;
		}
	}

	void  Jet() {
		if (Input.GetKeyDown (KeyCode.LeftAlt) && GameStatus.isJetPackPickedUp) {
			ActivateJetPackButtonPressed ();
		}
	}
	//Call this function when UI Button pressed
	public void ActivateJetPackButtonPressed() {

		if (!GameStatus.isAltButtonPressed) {
			speed = 0;
			GameStatus.isAltButtonPressed = true;
			anim.SetInteger ("State", 99);
			GameStatus.isJetPackOn = true;
			rb.gravityScale = 0;

		} else {
			speed = 0;
			GameStatus.isAltButtonPressed = false;
			GameStatus.isJetPackOn = false;
			rb.gravityScale = 1;
			anim.SetInteger ("State", 0);
		}
	}
	//Jet Go Up
	public void JetPackGoUpButtonPressed() {
		GameStatus.isJetUpButtonPressed = true;
	}
	public void JetPackGoUpButtonReleased() {
		GameStatus.isJetUpButtonPressed = false;
	}
	//Jet Go Down
	public void JetPackGoDownButtonPressed() {
		GameStatus.isJetDownButtonPressed = true;
	}
	public void JetPackGoDownButtonReleased() {
		GameStatus.isJetDownButtonPressed = false;
	}
	//Jet Go Left
	public void JetPackGoLeftButtonPressed() {
		GameStatus.isJetLeftButtonPressed = true;
	}
	public void JetPackGoLeftButtonReleased() {
		GameStatus.isJetLeftButtonPressed = false;
	}
	//Jet Go Right
	public void JetPackGoRightButtonPressed() {
		GameStatus.isJetRightButtonPressed = true;
	}
	public void JetPackGoRightButtonReleased() {
		GameStatus.isJetRightButtonPressed = false;
	}
}