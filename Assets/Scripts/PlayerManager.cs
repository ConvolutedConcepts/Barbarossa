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
    //Bullet
    public GameObject rightBullet;
    public GameObject leftBullet;

    Transform firePos;

    public Text scoreLabel;

    bool hasKey;

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

    }

    void actionIfDead()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}