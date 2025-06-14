using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpCombatScript : MonoBehaviour
{
	Animator anim;
	Rigidbody2D rb;
	ShlorpAggroRange shlorpAggroRange;
	SpriteRenderer spriteRenderer;
	GameObject player;
	[SerializeField] GameObject shlorpProjectile;
	
	readonly float teleportStageOneSpeed = (0.75f / 2.4f);
	readonly float teleportStageTwoSpeed = (0.667f / 2.4f);
	readonly float attackSpeed = (0.75f / 3);
	
	bool isAttacking = false;
	bool isTeleporting = false;
	
	float shlorpXScale;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		
		shlorpXScale = gameObject.transform.localScale.x;
        shlorpAggroRange = gameObject.GetComponentInChildren<ShlorpAggroRange>();
		
		rb.gravityScale *= 500;
    }

    void Update()
    {
        if ((!shlorpAggroRange.isWakingUp) && (shlorpAggroRange.playerInAggroRange) && (!isAttacking) && (!isTeleporting))
		{
			gameObject.transform.localScale = new Vector3(shlorpXScale * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
			BeginAttackSequence();
			
		}
    }


	void BeginAttackSequence()
	{
		isTeleporting = true;
		// 1st teleport
		SetTPStageTo1();
		if (Mathf.Sign(gameObject.transform.localScale.y) == -1) // if on ceiling, get on ground
			Invoke("TeleportAbovePlayer", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 2));
		Invoke("TeleportBehindPlayer", teleportStageOneSpeed);
		Invoke("SetTPStageTo2", teleportStageOneSpeed);
		Invoke("SetTPStageTo0", (teleportStageOneSpeed + teleportStageTwoSpeed));
		// 2nd teleport
		Invoke("SetTPStageTo1", (teleportStageOneSpeed + teleportStageTwoSpeed));
		Invoke("TeleportAbovePlayer", (teleportStageOneSpeed * 2 + teleportStageTwoSpeed)); // get on ceiling
		Invoke("SetTPStageTo2", (teleportStageOneSpeed * 2 + teleportStageTwoSpeed));
		Invoke("SetTPStageTo0", (teleportStageOneSpeed * 2 + teleportStageTwoSpeed * 2));
		// 3rd teleport
		Invoke("SetTPStageTo1", (teleportStageOneSpeed * 2 + teleportStageTwoSpeed * 2));
		// if player on ground, get on ground
		Invoke("IfOnGroundGetOnGround", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 2));
		Invoke("TeleportBehindPlayer", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 2));
		Invoke("SetTPStageTo2", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 2));
		Invoke("SetTPStageTo0", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 3));
		// Attack
		Invoke("Attack", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 3));
		// Reset the isAttacking and isTeleporting variables to false
		Invoke("ResetATKCooldown", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 3 + attackSpeed));
		Invoke("ResetTPCooldown", (teleportStageOneSpeed * 3 + teleportStageTwoSpeed * 3 + attackSpeed));
	}

	void TeleportBehindPlayer()
	{
		
		gameObject.transform.position = new Vector3(player.transform.position.x + (10 * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x)), gameObject.transform.position.y, gameObject.transform.position.z);
		gameObject.transform.localScale = new Vector3(shlorpXScale * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
	}

	void IfOnGroundGetOnGround()
	{
		if (Mathf.Sign(player.transform.localScale.y) == 1)
			TeleportAbovePlayer();
	}

	void TeleportAbovePlayer()
	{
		gameObject.transform.position = new Vector3(player.transform.position.x - (5 * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x)), gameObject.transform.position.y, gameObject.transform.position.z);
		gameObject.transform.localScale = new Vector3(shlorpXScale * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x), gameObject.transform.localScale.y * -1, gameObject.transform.localScale.z);
		SetInvisibleForASec();
		rb.gravityScale *= -1;
	}

	void Attack()
	{
		isAttacking = true;
		anim.SetBool("isAttacking", true);
		gameObject.transform.localScale = new Vector3(shlorpXScale * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		Invoke("LaunchProjectile", (attackSpeed));
	}

	void LaunchProjectile()
	{
		if (Mathf.Sign(gameObject.transform.localScale.x) == 1)
		{
			Instantiate(shlorpProjectile, (gameObject.transform.position + new Vector3(2,0,0)), Quaternion.Euler(0,0,0));
		}
		else
		{
			Instantiate(shlorpProjectile, (gameObject.transform.position - new Vector3(2,0,0)), Quaternion.Euler(0,0,180));
		}
	}
	void ResetTPCooldown()
	{
		isTeleporting = false;
	}
	void ResetATKCooldown()
	{
		isAttacking = false;
		anim.SetBool("isAttacking", false);
	}
	void SetInvisibleForASec()
	{
		spriteRenderer.color = new Color(0f,0f,0f,0f);
		Invoke("SetVisibleAgain", 0.1f);
	}
	void SetVisibleAgain()
	{
		spriteRenderer.color = new Color(1f,1f,1f,1f);
	}
	void SetTPStageTo1()
	{
		anim.SetInteger("teleportStage", 1);
	}
	void SetTPStageTo2()
	{
		anim.SetInteger("teleportStage", 2);
	}
	void SetTPStageTo0()
	{
		anim.SetInteger("teleportStage", 0);
	}
}
