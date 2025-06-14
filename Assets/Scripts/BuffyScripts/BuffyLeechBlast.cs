using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffyLeechBlast : MonoBehaviour
{
	Animator anim;
	TSOBasicAttack tsoBasicAttack;
	PlayerStats playerStats;
	GameObject healthBar;
	HealthScript healthScript;
	
	[SerializeField] GameObject textPrefab;
	GameObject existingText;
	[SerializeField] GameObject projectilePrefab;

	static readonly float animationDurationMultiplier = 1f;
	static readonly float animationDuration = 2.25f / animationDurationMultiplier;
	static readonly float animationFrames = 27f;
	static readonly float textSpawnFrame = (2f / animationFrames) * animationDuration;
	static readonly float blastProjectileSpawn = (27f / animationFrames) * animationDuration;
	static readonly float animationWDurationMultiplier = 1f;
	static readonly float animationWDuration = 0.917f / animationWDurationMultiplier;
	static readonly float animationLDurationMultiplier = 1f;
	static readonly float animationLDuration = 0.5f / animationLDurationMultiplier;

	//[HideInInspector] public bool playerStats.playerMidLeechBlast = false;


    void Start()
    {
        anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(playerStats.leechBlastKey)) && !playerStats.playerMidActionNoDash && (anim.GetFloat("verticalVelocity") == 0f))
		{
			playerStats.playerCanDash = false;
			playerStats.ResetPlayerDashCooldown();
			playerStats.playerCanMove = false;
			
			playerStats.playerMidLeechBlast = true;
			anim.SetBool("isLeechBlasting", true);
			
			Invoke("SpawnText", textSpawnFrame);
			Invoke("SpawnProjectile", blastProjectileSpawn);
		}
    }

	void SpawnText()
	{
		float spawnDistance = Mathf.Sign(transform.localScale.x) * 3;
		existingText = Instantiate(textPrefab, (gameObject.transform.position + new Vector3(spawnDistance,0,0)), Quaternion.Euler(0,0,0));
	}

	void SpawnProjectile()
	{
		Destroy(existingText);
		if (Mathf.Sign(gameObject.transform.localScale.x) == 1)
		{
			Instantiate(projectilePrefab, (gameObject.transform.position + new Vector3(0,0,0)), Quaternion.Euler(0,0,0));
		}
		else
		{
			Instantiate(projectilePrefab, (gameObject.transform.position + new Vector3(0,0,0)), Quaternion.Euler(0,0,180));
		}
	}

	void AnimateL()
	{
		anim.SetBool("isLeechBlastingL", true);
		anim.SetBool("isLeechBlasting", false);
		Invoke("ResetCooldown", animationLDuration);
	}

	void AnimateW()
	{
		anim.SetBool("isLeechBlastingW", true);
		anim.SetBool("isLeechBlasting", false);
		healthScript.LoseHealthBy(-1);
		Invoke("ResetCooldown", animationWDuration);
	}

	void ResetCooldown()
	{
		playerStats.playerCanDash = true;
		playerStats.playerCanMove = true;
		
		playerStats.playerMidLeechBlast = false;
		anim.SetBool("isLeechBlasting", false);
		anim.SetBool("isLeechBlastingW", false);
		anim.SetBool("isLeechBlastingL", false);
	}
}
