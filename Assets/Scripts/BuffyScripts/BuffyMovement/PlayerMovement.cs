using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	SpriteRenderer spriteRenderer;

	float force = 0f;
	float playerXScale;
	PlayerStats playerStats;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerStats = GetComponent<PlayerStats>();
		
		playerXScale = gameObject.transform.localScale.x;
    }

    // Use FixedUpdate instead of Update because FixedUpdate is more friendly with Rigidbody2D physics
    void FixedUpdate()
    {
		CheckNormalMovement();
		BeginMovement();
		
		CheckIfFallingAndAnimateAccordingly();
    }

	void CheckNormalMovement()
	{
		if (playerStats.playerCanMove)
		{
			if (Input.GetKey(playerStats.moveRightKey))
			{
				force += playerStats.playerMovementSpeed * Time.deltaTime;
				if (!playerStats.playerIsDashingButResets1MillisecondEarlier)
				{
					gameObject.transform.localScale = new Vector3(playerXScale,gameObject.transform.localScale.y,playerXScale);
				}
			}
			if (Input.GetKey(playerStats.moveLeftKey))
			{
				force -= playerStats.playerMovementSpeed * Time.deltaTime;
				if (!playerStats.playerIsDashingButResets1MillisecondEarlier)
				{
					gameObject.transform.localScale = new Vector3(-playerXScale,gameObject.transform.localScale.y,playerXScale);
				}
			}
		}
	}
	
	void BeginMovement()
	{
		// P.S. Don't combine transform.position with rigidbody stuff LOL
		rb.position += new Vector2(force, 0);

		anim.SetFloat("FORCE", Mathf.Abs(force));

		force = 0;
	}
	
	void CheckIfFallingAndAnimateAccordingly()
	{
		anim.SetFloat("verticalVelocity", Mathf.Abs(rb.velocity.y));
		
		if ((playerStats.playerIsDashing) || (playerStats.playerMidGravityShift) || (playerStats.playerMidTeleport) || (playerStats.playerMidShielding))
			anim.SetFloat("verticalVelocity", 0f);
	}
}
