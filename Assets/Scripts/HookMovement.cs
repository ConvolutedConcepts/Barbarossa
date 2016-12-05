using UnityEngine;
using System.Collections;

public class HookMovement : MonoBehaviour {

    private Rigidbody2D rb;
    public float drag;
    public float maxSwingSpeed;
    private Vector2 null_speed = new Vector2(0, 0);
    public float speedRampUp;
    private bool swingLeft;
    private bool swingRight;

	// Use this for initialization
	void Start () {
        rb = GetComponent <Rigidbody2D>();
        drag = .5f;
        maxSwingSpeed = 100f;
        speedRampUp = 13.5f;
        swingLeft = false;
        swingRight = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled) { rb.drag = drag; }
        setBoolSwing();

        if (Input.GetKeyDown(KeyCode.A))
        {
            if ((rb.velocity.x < 0 || rb.velocity == null_speed) && swingLeft)
            {
                swingLeft = false;
                rb.velocity = new Vector2(rb.velocity.x + -speedRampUp, 0);
                if(rb.velocity.x < -maxSwingSpeed)
                {
                    rb.velocity = new Vector2(-maxSwingSpeed, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if ((rb.velocity.x >= 0 || rb.velocity == null_speed) && swingRight)
            {
                swingRight = false;
                rb.velocity = new Vector2(rb.velocity.x + speedRampUp, 0);
                if (rb.velocity.x > maxSwingSpeed)
                {
                    rb.velocity = new Vector2(maxSwingSpeed, 0);
                }
            }
        }
	}

    void setBoolSwing()
    {
        if (rb.velocity.x < 0)
            swingRight = true;
        else if (rb.velocity.x > 0)
            swingLeft = true;
    }
}
