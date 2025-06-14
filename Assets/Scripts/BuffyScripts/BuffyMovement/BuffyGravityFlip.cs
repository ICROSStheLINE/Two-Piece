using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffyGravityFlip : MonoBehaviour
{
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	Animator anim;
	PlayerStats playerStats;
	
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
		if (Input.GetKeyDown(playerStats.gravityShiftKey) && !playerStats.playerMidActionNoDash && !playerStats.midCutscene)
		{
			playerStats.IgnoreEnemyCollisions(true);
			playerStats.playerCanDash = false;
			playerStats.ResetPlayerDashCooldown();
			playerStats.playerMidGravityShift = true;
			playerStats.playerCanMove = false;
			Invoke("GravityInverse", 0.375f);
			Invoke("ResetCooldown", 0.75f);
		}

		anim.SetBool("isGravityShifting", playerStats.playerMidGravityShift);
    }

	void GravityInverse()
	{
		rb.gravityScale *= -100;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y * -1,gameObject.transform.localScale.z);
		spriteRenderer.flipY = !spriteRenderer.flipY;
		Invoke("GravityReset", 0.1f);
	}

	void GravityReset()
	{
		rb.gravityScale /= 100;
	}

	void ResetCooldown()
	{
		playerStats.IgnoreEnemyCollisions(false);
		playerStats.playerMidGravityShift = false;
		spriteRenderer.flipY = !spriteRenderer.flipY;
		playerStats.playerCanMove = true;
		playerStats.playerCanDash = true;
	}
}
