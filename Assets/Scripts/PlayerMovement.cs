using UnityEngine;
using System.Collections;


public class PlayerMovement
{

    public float speedX = 9.7f;
    public float jumpHeight = 15.27f;

    bool facingRight;
    float speed;

    bool leftKeyPressed;
    bool leftKeyReleased;

    bool jumpButtonPressed;
    bool jumpButtonReleased;

    bool rightKeyPressed;
    bool rightKeyReleased;
    
    bool needToZeroSpeed;

    // Variable to allow jump after jump button is released
    bool canJump;

    Animator anim;
    Rigidbody2D rb;

    //Sounds 
    private AudioClip jumpLaunchSound;
    private AudioClip jumpLandSound;
    private AudioSource audio;

    private Transform transform;
    

    // Use this for initialization
    public PlayerMovement(AudioSource audio, Animator anim, ref Rigidbody2D rb, Transform transform, AudioClip jumpLaunchSound, AudioClip jumpLandSound)
    {
        this.audio = audio;
        this.anim = anim;
        this.rb = rb;
        this.transform = transform;
        this.jumpLaunchSound = jumpLaunchSound;
        this.jumpLandSound = jumpLandSound;

        facingRight = true;
        needToZeroSpeed = false;

        rb.freezeRotation = true;

        canJump = true; // Initial state is able to jump
    }

    // Update is called once per frame
    public void Update()
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

    void OnCollisionEnter2D(Collision2D other) { 

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                audio.PlayOneShot(jumpLandSound, 0.7F);
                return;
            }
    }
}