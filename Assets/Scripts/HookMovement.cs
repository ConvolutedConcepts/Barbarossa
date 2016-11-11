using UnityEngine;
using System.Collections;

public class HookMovement : MonoBehaviour {

    public Rigidbody2D rb;
    private float drag;
    private float maxSwingSpeed;
    public Vector2 null_speed = new Vector2(0, 0);
    private float speedRampUp;

	// Use this for initialization
	void Start () {
        rb = GetComponent <Rigidbody2D>();
        drag = .5f;
        maxSwingSpeed = 20f;
        speedRampUp = 7.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled) { rb.drag = drag; }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (rb.velocity.x < 0 || rb.velocity == null_speed)
            {
                rb.velocity = new Vector2(rb.velocity.x + -speedRampUp, 0);
                if(rb.velocity.x < -maxSwingSpeed)
                {
                    rb.velocity = new Vector2(-maxSwingSpeed, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rb.velocity.x >= 0 || rb.velocity == null_speed)
            {
                rb.velocity = new Vector2(rb.velocity.x + speedRampUp, 0);
                if (rb.velocity.x > maxSwingSpeed)
                {
                    rb.velocity = new Vector2(maxSwingSpeed, 0);
                }
            }
        }
	}
}
