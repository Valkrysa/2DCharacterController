using UnityEngine;
using System.Collections;

public class Character2DController : MonoBehaviour {

	public float jumpForce = 700f;
	public float maxSpeed = 10f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	
	private float groundRadius = 0.2f;
	private bool grounded = false;
	private bool facingRight = true;
	private Animator animator;
	private bool doubleJump = false;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void Update (){
		if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space)) {
			animator.SetBool("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			
			if (!doubleJump && !grounded) {
				doubleJump = true;
			}
		}
	}
	
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		animator.SetBool ("Ground", grounded);
		
		if (grounded) {
			doubleJump = false;
		}
		
		animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
		
		if (!grounded) {
			return; // don't allow control in the air
		}
	
		float move = Input.GetAxis("Horizontal");
		
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		animator.SetFloat("Speed", Mathf.Abs(move));
		
		if (move > 0 && !facingRight) {
			Flip();
		} else if (move < 0 && facingRight) {
			Flip();
		}
	}
	
	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
