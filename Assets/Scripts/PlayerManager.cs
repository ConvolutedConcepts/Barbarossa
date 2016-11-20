using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviour
{
    public GameObject player_Explosion;
    private GameObject instantiateExplosion;
    Transform playerPosition;

    private GameObject Player;

    public Text scoreLabel;

    bool hasKey;

    Animator anim;
    Rigidbody2D rb;

    //Sounds 
    public AudioClip gemPickedUp;
    public AudioClip gotKey;
    public AudioClip walking;
    public AudioClip falling;
    public AudioClip dead;
    new AudioSource audio;


    // Use this for initialization
    void Start()
    {
        Player = gameObject;

        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        hasKey = false;

        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
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

        if(c.gameObject.layer == LayerMask.NameToLayer("Abyss"))
        {
            Debug.Log("Entered");
            fallInAbyss(c);

        }
    }

    //If we touched ground allow jump and set the player to Idle mode
    void OnCollisionEnter2D(Collision2D other)
    {
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
            GameStatus.loadNextLevel();
            return;
        }

    }

    void fallInAbyss(Collider2D other)
    {
        //Physics2D.IgnoreCollision(other.collider, GetComponent< Collider2D >());
        GameStatus.falling = true;
        var target = GameObject.Find("Camera");
        CameraFollow cf = GetComponentInChildren<CameraFollow>();
        //cf.enabled = false;
        Camera c = GetComponentInChildren<Camera>();
    }



    void actionIfDead()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        GameStatus.GoToMainMenuScreen();
    }
}