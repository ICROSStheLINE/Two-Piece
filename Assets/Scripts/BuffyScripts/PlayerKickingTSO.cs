using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickingTSO : MonoBehaviour
{
	Animator anim;
	Rigidbody2D rb;
	GameObject truthSeekingOrb;
	[SerializeField] GameObject truthSeekingOrbPrefab;
	GameObject tsoBeingKicked;
	[SerializeField] GameObject tsoBeingKickedPrefab;
	PlayerStats playerStats;

	static readonly float kickingAnimationDurationSpeedMultiplier = 1;
	static readonly float kickingAnimationDuration = 0.833f / kickingAnimationDurationSpeedMultiplier;
	static readonly float kickingAnimationFrames = 10;
	static readonly float tsoProjectileSpawn = (5 / kickingAnimationFrames) * kickingAnimationDuration;

	static readonly float teleportingAnimationDurationSpeedMultiplier = 1;
	static readonly float teleportingAnimationDuration = 0.667f / teleportingAnimationDurationSpeedMultiplier;
//	static readonly float teleportingAnimationFrames = 8;

	static readonly float ballAirtimeDuration = 0.4f;

	//[HideInInspector] public bool playerStats.playerMidKickingTSO = false;
	bool ableToTeleport = false;
	//[HideInInspector] public bool playerStats.playerMidKickingTSOButForTheCameraGameObject = false;
	bool touchingFloorOrWall = false;
	float originalYPosMidTeleporting = 0;

    void Start()
    {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		truthSeekingOrb = GameObject.FindWithTag("Truth Seeking Orb");
		playerStats = GetComponent<PlayerStats>();
    }


    void Update()
    {
		// Kick out ball
        if ((Input.GetKeyDown(playerStats.orbKickKey)) && !playerStats.playerMidActionNoDash && !playerStats.midCutscene && !playerStats.isTSOBasicAttacking && (anim.GetFloat("verticalVelocity") == 0f) && (!ableToTeleport))
		{
			Destroy(truthSeekingOrb);
			
			playerStats.playerMidKickingTSOButForTheCameraGameObject = true;
			
			// Cancel Movement
			playerStats.playerCanDash = false;
			playerStats.ResetPlayerDashCooldown();
			playerStats.playerCanMove = false;
			
			playerStats.playerMidKickingTSO = true;
			
			// Animate
			anim.SetBool("kickingTSO", true);
			Invoke("SpawnOrbProjectile", tsoProjectileSpawn);
			Invoke("ResetAbilityToTPCooldown", tsoProjectileSpawn + ballAirtimeDuration);
			Invoke("ResetCooldown", kickingAnimationDuration);
		}
		// Teleport to ball
		else if ((Input.GetKeyDown(playerStats.orbKickKey)) && (!playerStats.playerMidGravityShift) && (!playerStats.playerMidTeleport) && (!playerStats.playerMidShielding) && (!playerStats.isTSOBasicAttacking) && (anim.GetFloat("verticalVelocity") == 0f) && (ableToTeleport) && (tsoBeingKicked != null))
		{
			CancelInvoke("SpawnTSOPrefab");
			// Cancel Movement
			playerStats.playerCanDash = false;
			playerStats.ResetPlayerDashCooldown();
			playerStats.playerCanMove = false;
			
			CancelInvoke("ResetCooldown");
			CancelInvoke("ResetAbilityToTPCooldown");
			ableToTeleport = false;
			// Animate
			anim.SetBool("kickingTSO", false);
			anim.SetBool("kickingTSOP2", true);
			
			Invoke("ResetCooldown", teleportingAnimationDuration);
			TeleportToBall();
			for (float i = 0; i < 0.2f; i += 0.01f)
				Invoke("TPToOriginalPosVariable", i);
			Invoke("FreezeConstraints", 0.19f);
			Invoke("UnfreezeConstraints", teleportingAnimationDuration + 0.2f);
		}
    }

	public void SpawnTSOPrefab()
	{
		Destroy(truthSeekingOrb);
		truthSeekingOrb = Instantiate(truthSeekingOrbPrefab, tsoBeingKicked.transform.position, Quaternion.Euler(0,0,0));
	}

	void UnfreezeConstraints()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void FreezeConstraints()
	{
		rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
	}

	void TeleportToBall()
	{
		SpawnTSOPrefab();
		
		gameObject.transform.position = tsoBeingKicked.transform.position;
		originalYPosMidTeleporting = gameObject.transform.position.y;
		Invoke("GiveSomeVelocityAfterTeleporting", 0.1f);
		Destroy(tsoBeingKicked);
	}

	void SpawnOrbProjectile()
	{
		ableToTeleport = true;
		if (Mathf.Sign(gameObject.transform.localScale.x) == 1)
		{
			tsoBeingKicked = Instantiate(tsoBeingKickedPrefab, (gameObject.transform.position + new Vector3(2,0,0)), Quaternion.Euler(0,0,0));
		}
		else
		{
			tsoBeingKicked = Instantiate(tsoBeingKickedPrefab, (gameObject.transform.position + new Vector3(-2,0,0)), Quaternion.Euler(0,0,180));
		}
		Invoke("SpawnTSOPrefab", ballAirtimeDuration);
	}

	void ResetAbilityToTPCooldown()
	{
		ableToTeleport = false;
	}

	void ResetCooldown()
	{
		playerStats.playerCanDash = true;
		playerStats.playerCanMove = true;
		
		anim.SetBool("kickingTSO", false);
		anim.SetBool("kickingTSOP2", false);
		playerStats.playerMidKickingTSO = false;
		
		playerStats.Invoke("ResetPlayerMidKickingTSOButForTheCameraGameObjectVariable", 0.2f);
	}

	void GiveSomeVelocityAfterTeleporting()
	{
		if (touchingFloorOrWall)
			rb.velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 13,0);
		else
			rb.velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 2,0);
	}
	
	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
			touchingFloorOrWall = true;
		else
			touchingFloorOrWall = false;
	}
	
	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
			touchingFloorOrWall = false;
	}
	
	void TPToOriginalPosVariable()
	{
		gameObject.transform.position = new Vector3(transform.position.x, originalYPosMidTeleporting, transform.position.z);
	}
}
