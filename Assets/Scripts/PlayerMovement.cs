using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{

    public float speedX = 9.7f;
    public float jumpHeight = 15.27f;

    Transform playerPosition;


    private GameObject Player;


    bool facingRight;
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

    //Sounds 
    public AudioClip jumpLaunchSound;
    public AudioClip jumpLandSound;
    new AudioSource audio;

    private Vector3 spown;

    void Awake()
    {
        Player = gameObject;
    }

    // Use this for initialization
    void Start()
    {
        spown = transform.position;
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        facingRight = true;

        isPlayerFrozen = false;
        needToZeroSpeed = false;

        rb.freezeRotation = true;

        canJump = true; // Initial state is able to jump
    }

    // Update is called once per frame
    void Update()
    {
        anim.enabled = GameStatus.isGrounded;

        getKeyPress();
        getKeyRelease();

        if (leftKeyPressed)
            speed = -speedX;

        if (rightKeyPressed)
            speed = speedX;

        if (leftKeyReleased || rightKeyReleased)
            speed = 0;

        if (jumpButtonPressed && canJump)
        {
            canJump = false;
            JumpButtonPressed();
        }

        if (jumpButtonReleased)
        {
            canJump = true;             // Allow Jump after button is released
        }

        Flip();
        MovePlayer(speed);

        playerPosition = Player.transform;
    }

    private bool getKeyPress()
    {
        leftKeyPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        rightKeyPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        jumpButtonPressed = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);

        return leftKeyPressed || rightKeyPressed || jumpButtonReleased;
    }

    private bool getKeyRelease()
    {
        leftKeyReleased = Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);

        rightKeyReleased = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);

        jumpButtonReleased = Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

        return leftKeyReleased || rightKeyPressed || jumpButtonReleased;
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
}