using UnityEngine;
using System.Collections;

public class HookMovement : MonoBehaviour {

    public Rigidbody2D rb;
    private KeyCode lastKey;
    private float drag = .5f;
    private float hook_speed = 10f;

	// Use this for initialization
	void Start () {
        rb = GetComponent <Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled) { rb.drag = drag; }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(-hook_speed, 0);
                lastKey = KeyCode.A;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(hook_speed, 0);
                lastKey = KeyCode.D;
            }
        }
	}
}
