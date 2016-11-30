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
        if (c.gameObject.tag == "Death")
        {
            actionIfDead();
        }

        if(c.gameObject.layer == LayerMask.NameToLayer("Abyss"))
        {
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

        if(other.gameObject.tag == "Death")
        {
            death();
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

    void death()
    {
        stopMovement();
        audio.PlayOneShot(dead, 0.8F);
        StartCoroutine(wait(1));
    }


    void fallInAbyss(Collider2D other)
    {
        stopMovement();
        audio.PlayOneShot(falling, 0.8F);
        StartCoroutine(wait(1));

    }

    IEnumerator wait(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        actionIfDead();
    }


    void stopMovement()
    {
        PlayerMovement pm = GetComponent<PlayerMovement>();
        pm.enabled = false;
        HookMechanic hm = GetComponent<HookMechanic>();
        hm.enabled = false;
    }



    void actionIfDead()
    {
        GameStatus.currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        GameStatus.GoToMainMenuScreen();
    }
}