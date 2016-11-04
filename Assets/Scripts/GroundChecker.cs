using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour {

	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public Transform feet;
	private bool grounded;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate() {
		grounded = Physics2D.OverlapCircle (feet.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
		GameStatus.isGrounded = grounded;
	}
}
