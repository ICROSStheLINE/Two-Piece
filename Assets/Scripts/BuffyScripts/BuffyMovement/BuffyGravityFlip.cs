using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffyGravityFlip : MonoBehaviour
{
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	Animator anim;
	PlayerStats playerStats;
	LayerMask layerMask;
	
	static readonly float animationDurationSpeedMultiplier = 1f;
	static readonly float animationDuration = 0.750f / animationDurationSpeedMultiplier;
	static readonly float animationFrames = 9f;
	static readonly float flipFrame = 6f - 1f; 
	// Note that the frame duration is the duration of the animation until the END of this frame, hence the -1 in the line above.
	static readonly float flipFrameDuration = (flipFrame / animationFrames) * animationDuration;
	static readonly float secondsBetweenFlipFrameDurationAndAnimationEnd = animationDuration - flipFrameDuration;
	
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		layerMask = LayerMask.GetMask("Floor or Wall");
    }

    void Update()
    {
		if (Input.GetKeyDown(playerStats.gravityShiftKey) && !playerStats.playerMidActionNoDash && !playerStats.midCutscene)
		{
			StartCoroutine("GravityShift");
		}

		anim.SetBool("isGravityShifting", playerStats.playerMidGravityShift);
    }
	
	IEnumerator GravityShift()
	{
		playerStats.IgnoreEnemyCollisions(true);
		playerStats.playerCanDash = false;
		playerStats.ResetPlayerDashCooldown();
		playerStats.playerMidGravityShift = true;
		anim.SetBool("isGravityShifting", true);
		playerStats.playerCanMove = false;
		yield return new WaitForSeconds(flipFrameDuration);
		GravityInverse();
		yield return new WaitForSeconds(secondsBetweenFlipFrameDurationAndAnimationEnd);
		playerStats.IgnoreEnemyCollisions(false);
		playerStats.playerMidGravityShift = false;
		anim.SetBool("isGravityShifting", false);
		spriteRenderer.flipY = !spriteRenderer.flipY;
		playerStats.playerCanMove = true;
		playerStats.playerCanDash = true;
	}

	private void GravityInverse()
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.up * Mathf.Sign(gameObject.transform.localScale.y), 100f, layerMask);
		if (hit.collider != null)
        {
			float distanceToFeet = spriteRenderer.bounds.extents.y;
            transform.position = hit.point + new Vector2(0,-distanceToFeet * Mathf.Sign(gameObject.transform.localScale.y));
        }
		rb.gravityScale *= -1;
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y * -1,gameObject.transform.localScale.z);
		spriteRenderer.flipY = !spriteRenderer.flipY;
	}
}
