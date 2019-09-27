using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float RAYCASTDOWNDIST = 0.8f;

	private const float ACCELX = 50;
	private const float MAXSPEEDX = 10;
	private const float INPUTTOL = 0.0f;
	private const float BASEJUMPFORCE = 250;
	private const float JUMPFORCEINC = 150;
	private const float JUMPSQUATFRAMES = 4;
	
    private Rigidbody2D playerBody;
	private SpriteRenderer playerSprite;
	
	//Player inputs
	private float inputHorz;
	private float inputVert;
	private bool inputJumpDown;
	private bool inputJumpUp;
	private bool inputJumpHeld;

	private float jumpForce;
	private int jumpSquatCounter;
	private bool jumpPressed;

	//Basic states
	private bool isMoving;
	private bool isGrounded;
	private bool isMovingUp;
	private bool isMovingDown;
	private bool isInputLeft;
	private bool isInputRight;

	//Combo states
	private bool isJumping;
	


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
		playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		GetInput();
		CheckState();
		UpdateAnim();
		UpdatePhysics();
    }    

	void UpdatePhysics()
	{	
		if(isInputLeft)
		{
			playerBody.AddForce(transform.TransformDirection(Vector3.left) * ACCELX);
		}
		else if(isInputRight)
		{
			playerBody.AddForce(transform.TransformDirection(Vector3.right) * ACCELX);
		}
		else if(isGrounded)
		{
			playerBody.velocity = new Vector2(0, playerBody.velocity.y);	
		}
		if (Mathf.Abs(playerBody.velocity.x) > MAXSPEEDX)
        {
            playerBody.velocity = new Vector2(MAXSPEEDX * Mathf.Sign(playerBody.velocity.x), playerBody.velocity.y);
        }

		if(inputJumpDown && isGrounded)
		{
			jumpSquatCounter = 0;
			jumpForce = BASEJUMPFORCE;
			jumpPressed = true;
		}

		if(inputJumpHeld && isGrounded)
		{	
			if (jumpSquatCounter < JUMPSQUATFRAMES)
			{
				jumpForce += JUMPFORCEINC;
				jumpSquatCounter++;
			}
			else if(jumpPressed)
			{
				Jump();
			}
		}

		if(inputJumpUp && isGrounded && jumpPressed)
		{
			Jump();
		}


	}

	void GetInput()
	{
		inputHorz = Input.GetAxisRaw("Horizontal");
		inputVert = Input.GetAxisRaw("Vertical");
		inputJumpDown = Input.GetButtonDown("Jump");
		inputJumpHeld = Input.GetButton("Jump");
		inputJumpUp = Input.GetButtonUp("Jump");

	}

	void CheckState()
	{
		// Update basic states
		if(inputHorz > INPUTTOL)
		{
			isInputRight = true;
		}
		else
		{
			isInputRight = false;
		}

		if(inputHorz < -INPUTTOL)
		{
			isInputLeft = true;
		}
		else
		{
			isInputLeft = false;
		}

		if(Mathf.Abs(playerBody.velocity.x) > 0)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}

		if(playerBody.velocity.y < 0)
		{
			isMovingUp = true;
		}
		else
		{
			isMovingUp = false;
		}

		if(playerBody.velocity.y > 0)
		{
			isMovingDown = true;
		}
		else
		{
			isMovingDown = false;
		}

		isGrounded = RayCastGround(-0.3f) || RayCastGround(0) || RayCastGround(0.3f);


		//Update combo states
		if(isMovingUp && !isGrounded)
		{
			isJumping = true;
		}
		else
		{
			isJumping = false;
		}
	}

	void UpdateAnim()
	{
		if(isInputRight)
		{
			playerSprite.flipX = true;
		}

		if(isInputLeft)
		{
			playerSprite.flipX = false;
		}


	}

	bool RayCastGround(float offset)
	{
        int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        layerMask = ~layerMask;
		Vector3 currentPos = transform.position;
		currentPos.x += (offset);

        if (Physics2D.Raycast(currentPos, transform.TransformDirection(Vector3.down), RAYCASTDOWNDIST, layerMask).collider == null)
        {
			Debug.DrawRay(currentPos, transform.TransformDirection(Vector3.down) * RAYCASTDOWNDIST, Color.yellow);
            return false;
        }
        else
        {
			Debug.DrawRay(currentPos, transform.TransformDirection(Vector3.down) * RAYCASTDOWNDIST, Color.red);
            return true;
        }
	}

	void Jump()
	{
		playerBody.AddForce(transform.TransformDirection(Vector3.up) * jumpForce);
		jumpPressed = false;
	}

}
