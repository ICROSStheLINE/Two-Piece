using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporting : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	PlayerStats playerStats;
	SpriteRenderer playerSpriteRenderer;
	
	static readonly float teleportInDurationSpeedMultiplier = 1.5f;
	static readonly float teleportInDuration = 0.583f / teleportInDurationSpeedMultiplier;
	static readonly float teleportInFrames = 7f;
	
	static readonly float teleportOutDurationSpeedMultiplier = 1.5f;
	static readonly float teleportOutDuration = 0.417f / teleportOutDurationSpeedMultiplier;
	static readonly float teleportOutFrames = 5f;
	static readonly float teleportOutFrameDuration = teleportOutDuration / teleportOutFrames;
	
	static readonly float teleportOutAirDurationSpeedMultiplier = 1.5f;
	static readonly float teleportOutAirDuration = 0.5f / teleportOutAirDurationSpeedMultiplier;
	static readonly float teleportOutAirFrames = 6f;
	
	static readonly float totalTeleportDuration = teleportInDuration + teleportOutDuration;
	static readonly float totalTeleportFrames = teleportInFrames + teleportOutFrames;
	
	static readonly float totalTeleportAirDuration = teleportInDuration + teleportOutFrameDuration + teleportOutAirDuration;
	static readonly float totalTeleportAirFrames = teleportInFrames + 1f + teleportOutAirFrames;
	
	//[HideInInspector] public bool playerStats.playerMidTeleport = false;
	[SerializeField] float teleportDistance;
	float teleportHeight = 0;
	
	[SerializeField] GameObject teleportIndicatorPrefab;
	GameObject teleportIndicator;
	Color purple = new Color(0.688f,0f,1f,1f);
	Color red = new Color(1f,0f,0f,1f);
	
	[SerializeField] GameObject teleportSpriteMaskPrefab;
	GameObject teleportSpriteMask;
	[SerializeField] GameObject teleportEffectPrefab;
	GameObject teleportEffect;
	
	
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

		
    }

	IEnumerator Teleport()
	{
		playerStats.playerQueuingTeleport = false;
		
		playerStats.playerMidTeleport = true;
		
		
		teleportSpriteMask = Instantiate(teleportSpriteMaskPrefab, transform.position, transform.rotation);
		teleportSpriteMask.transform.parent = gameObject.transform;
		playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
		teleportEffect = Instantiate(teleportEffectPrefab, transform.position, transform.rotation);
		teleportEffect.transform.parent = gameObject.transform;
		StartCoroutine("SpriteMaskAnimation");
		
		yield return new WaitForSeconds(teleportInDuration);
		
		anim.SetBool("isTeleporting", true);
		playerStats.playerCanDash = false;
		playerStats.ResetPlayerDashCooldown();
		playerStats.playerCanMove = false;
		
		playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
		Destroy(teleportSpriteMask);
		Destroy(teleportEffect);
		
		transform.position = teleportIndicator.transform.position;
		rb.velocity = new Vector2(Mathf.Abs(teleportHeight)/2 * Mathf.Sign(transform.localScale.x),teleportHeight * 5);
		RemoveIndicator();
		
		yield return new WaitForSeconds(teleportOutFrameDuration);
		
		// This part basically just lets the first frame pass, then checks to see if the player is in mid-air or not.
		// If the player's in mid-air it just sets the animation duration to that of the mid-air teleport animation.
		
		if (anim.GetFloat("verticalVelocity") > 10)
			yield return new WaitForSeconds(teleportOutAirDuration - teleportOutFrameDuration);
		else
			yield return new WaitForSeconds(teleportOutDuration - teleportOutFrameDuration);
		
		ResetCooldown();
	}

	void ResetCooldown()
	{
		playerStats.playerMidTeleport = false;
		anim.SetBool("isTeleporting", false);
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
	
	// Idk this is just some vibecoded method that makes the SpriteMask's localScale lower linearly until it reaches 0
	IEnumerator SpriteMaskAnimation()
	{
		Vector3 startScale = teleportSpriteMask.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float time = 0f;

        while (time < teleportInDuration)
        {
            float t = time / teleportInDuration; // normalized 0 → 1
            teleportSpriteMask.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        // ensure exact zero at the end
        teleportSpriteMask.transform.localScale = targetScale;
	}
}
