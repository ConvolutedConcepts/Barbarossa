using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviour
{

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

    // Variable to allow jump after jump button is released
    bool canJump;

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
    void Start()
    {
        firePos = transform.FindChild("FirePosition");
        spown = transform.position;
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        facingRight = true;
        hasKey = false;

        isPlayerFrozen = false;
        needToZeroSpeed = false;

        rb.freezeRotation = true;

        canJump = true; // Initial state is able to jump
    }

    // Update is called once per frame
    void Update()
    {
        anim.enabled = GameStatus.isGrounded;
        Flip();
        MovePlayer(speed);

        leftKeyPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        leftKeyReleased = Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);

        rightKeyPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        rightKeyReleased = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);

        jumpButtonPressed = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        jumpButtonReleased = Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

        playerPosition = Player.transform;

        if (leftKeyPressed)
            speed = -speedX;

        if (rightKeyPressed)
            speed = speedX;

        if (leftKeyReleased || rightKeyReleased)
            speed = 0;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }

        if (jumpButtonPressed && canJump)
        {
            canJump = false;
            JumpButtonPressed();
        }

        if (jumpButtonReleased)
        {
            canJump = true;             // Allow Jump after button is released
        }
    }

    void MovePlayer(float playerSpeed)
    {
        rb.velocity = new Vector3(speed, rb.velocity.y, 0);
    }
    //Flip Player when Left arrow pressed
    void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight)
        {
            if (needToZeroSpeed)
            {
                speed = 0;
                needToZeroSpeed = false;
            }
            facingRight = !facingRight;
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {

        if (c.gameObject.tag == "monster_bullet")
        {
            actionIfDead();
            Destroy(c.gameObject);
        }

        if (c.gameObject.tag == "Hazard")
        {
            actionIfDead();
        }
    }

    //If we touched ground allow jump and set the player to Idle mode
    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "gun")
        {
            audio.PlayOneShot(gotSpecial, 0.8F);
            Destroy(other.gameObject);
            GameStatus.allowToFire = true;
            GameStatus.isGunPickedUp = true;
            return;
        }

        if (other.gameObject.tag == "gem")
        {
            audio.PlayOneShot(gemPickedUp, 0.8F);
            Destroy(other.gameObject);
            GameStatus.score += 100;
            scoreLabel.text = GameStatus.score.ToString();
            return;
        }

        if (other.gameObject.tag == "enemy" ||
            other.gameObject.tag == "Hazard" ||
            other.gameObject.layer == LayerMask.NameToLayer("Hazard")
        )
        {
            actionIfDead();
            return;
        }

        if (other.gameObject.tag == "key")
        {
            //GameObject.Find ("Level1_C_Text").SetActive (false);
            audio.PlayOneShot(gotKey, 0.7F);
            hasKey = true;
            Destroy(other.gameObject);
            return;
        }

        if (other.gameObject.tag == "Door")
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            audio.PlayOneShot(jumpLandSound, 0.7F);
            return;
        }
    }

    void actionIfDead()
    {
        SceneManager.LoadScene("Level1");
    }

    public void JumpButtonPressed()
    {
        if (GameStatus.isGrounded)
            jump();
    }

    public void jump()
    {
        audio.PlayOneShot(jumpLaunchSound, 0.7F);
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}