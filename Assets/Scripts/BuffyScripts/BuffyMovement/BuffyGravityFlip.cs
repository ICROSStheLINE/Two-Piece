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
		Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 30f, LayerMask.GetMask("Floor or Wall")))
        {
            transform.position = hit.point;
        }
		rb.gravityScale *= -1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y * -1,gameObject.transform.localScale.z);
		spriteRenderer.flipY = !spriteRenderer.flipY;
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
