using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporting : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	PlayerStats playerStats;
	SpriteRenderer playerSpriteRenderer;
	
	static readonly float animationDurationSpeedMultiplier = 1.5f;
	static readonly float animationDuration = 1.083f / animationDurationSpeedMultiplier;
	static readonly float animationFrames = 13f;
	static readonly float teleportFrame = 9f;
	static readonly float secondsUntilTeleport = (teleportFrame / animationFrames) * animationDuration;
	static readonly float secondsBetweenTeleportAndEnd = animationDuration - secondsUntilTeleport;
	
	//[HideInInspector] public bool playerStats.playerMidTeleport = false;
	[SerializeField] float teleportDistance;
	float teleportHeight = 0;
	
	[SerializeField] GameObject teleportIndicatorPrefab;
	GameObject teleportIndicator;
	Color purple = new Color(0.688f,0f,1f,1f);
	Color red = new Color(1f,0f,0f,1f);
	
	[SerializeField] GameObject teleportSpriteMaskPrefab;
	GameObject teleportSpriteMask;
	
	
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(playerStats.teleportKey)) // If holding the teleport button
		{
			if (!playerStats.playerMidActionNoDash && !playerStats.midCutscene)
			{
				playerStats.playerQueuingTeleport = true;
				playerSpriteRenderer.color = new Color(1f,0.5f,1f,1f);
			
				if (teleportIndicator == null)
					teleportIndicator = Instantiate(teleportIndicatorPrefab, transform.position + new Vector3(teleportDistance * Mathf.Sign(gameObject.transform.localScale.x),teleportHeight,0), transform.rotation);
			}
			else if (playerStats.playerQueuingTeleport)
			{
				//float indicatorHeightFromPlayer = teleportIndicator.transform.position.y - transform.position.y;
				teleportIndicator.transform.position = transform.position + new Vector3(teleportDistance * Mathf.Sign(gameObject.transform.localScale.x),teleportHeight,0);
				teleportIndicator.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), Mathf.Sign(transform.localScale.y), Mathf.Sign(transform.localScale.z));
			}
			
			
			if (Input.GetKey(playerStats.aimUpKey) && Input.GetKey(playerStats.aimDownKey))
			{
				if (teleportHeight != 0)
					RemoveIndicator();
				teleportHeight = 0;
			}
			else if (Input.GetKey(playerStats.aimUpKey))
			{
				if (teleportHeight != teleportDistance/2)
					RemoveIndicator();
				teleportHeight = teleportDistance/2;
			}
			else if (Input.GetKey(playerStats.aimDownKey))
			{
				if (teleportHeight != teleportDistance/2 * -1)
					RemoveIndicator();
				teleportHeight = teleportDistance/2 * -1;
			}
			else 
			{
				if (teleportHeight != 0)
					RemoveIndicator();
				teleportHeight = 0;
			}
		}
		else if (playerStats.playerQueuingTeleport) // If they let go of the teleport button
		{
			if (teleportIndicator.GetComponent<SpriteRenderer>().color == purple)
			{
				StartCoroutine("Teleport");
			}
			else
				RemoveIndicator();
		}
		else if ((playerStats.playerMidTeleport) && teleportIndicator != null)
		{
			if (teleportIndicator.GetComponent<SpriteRenderer>().color == red)
			{
				// This if statement exists because there's a small bug where someone can teleport when they're not supposed to.
				// This happens when the player turns while having the indicator out.
				// If the indicator is purple before they turned, the indicator will be purple for a split second before turning red (if it's supposed to).
				StopCoroutine("Teleport");
				ResetCooldown();
				RemoveIndicator();
			}
		}

		anim.SetBool("isTeleporting", playerStats.playerMidTeleport);
    }

	IEnumerator Teleport()
	{
		playerStats.playerQueuingTeleport = false;
		playerStats.playerCanDash = false;
		playerStats.ResetPlayerDashCooldown();
		playerStats.playerMidTeleport = true;
		playerStats.playerCanMove = false;
		
		teleportSpriteMask = Instantiate(teleportSpriteMaskPrefab, transform.position, transform.rotation);
		teleportSpriteMask.transform.parent = gameObject.transform;
		playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
		
		yield return new WaitForSeconds(secondsUntilTeleport);
		
		playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
		Destroy(teleportSpriteMask);
		
		transform.position = teleportIndicator.transform.position;
		rb.velocity = new Vector2(Mathf.Abs(teleportHeight)/2 * Mathf.Sign(transform.localScale.x),teleportHeight * 5);
		RemoveIndicator();
		
		yield return new WaitForSeconds(secondsBetweenTeleportAndEnd);
		
		ResetCooldown();
	}

	void ResetCooldown()
	{
		playerStats.playerMidTeleport = false;
		playerStats.playerCanMove = true;
		playerStats.playerCanDash = true;
	}
	
	void RemoveIndicator()
	{
		Destroy(teleportIndicator);
		teleportIndicator = null;
		playerStats.playerQueuingTeleport = false;
		
		playerSpriteRenderer.color = new Color(1f,1f,1f,1f);
	}
	
	IEnumerator SpriteMaskAnimation()
	{
		// The SpriteMask should try to follow the teleport effect animation
		// How should I code that? 
		// Should I just eyeball it and hardcode its localScale shrinking?
		// MAYBE I should take the secondsUntilTeleport variable and have it linearly shrink until it hits 0 at the secondsUntilTeleport mark
		
	}
}
