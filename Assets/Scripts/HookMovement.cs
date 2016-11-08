using UnityEngine;
using System.Collections;

public class HookMovement : MonoBehaviour {

    public Rigidbody2D rb;
    public KeyCode lastKey;

	// Use this for initialization
	void Start () {
        rb = GetComponent <Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled) { rb.drag = .5f; }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lastKey != KeyCode.A)
            {
                print("Adding force to the left");
                rb.velocity = new Vector2(-10f, 0);
                lastKey = KeyCode.A;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (lastKey != KeyCode.D)
            {
                print("Adding force to the right");
                rb.velocity = new Vector2(10f, 0);
                lastKey = KeyCode.D;
            }
        }
	}
}
