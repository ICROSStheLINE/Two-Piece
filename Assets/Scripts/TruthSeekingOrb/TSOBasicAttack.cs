using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOBasicAttack : MonoBehaviour
{
    [SerializeField] GameObject theHitbox;
	GameObject player;
	Animator anim;
	PlayerStats playerStats;
	PlayerTSOBasicAttack playerTSOBasicAttack;

	static readonly float attackAnimationDurationSpeedMultiplierStageOne = 1f;
	static readonly float attackAnimationDurationStageOne = 1f / attackAnimationDurationSpeedMultiplierStageOne;
	static readonly float attackAnimationFramesStageOne = 12;
	static readonly float attackHitboxSpawnStageOne = (4 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float attackHitboxDespawnStageOne = (7 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float attackOffCooldownStageOne = (8 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float secondsBetweenAttackHitboxDespawnAndSpawnStageOne = Mathf.Abs(attackHitboxDespawnStageOne - attackHitboxSpawnStageOne);
	static readonly float secondsBetweenDespawnAndOffCooldownStageOne = Mathf.Abs(attackOffCooldownStageOne - attackHitboxDespawnStageOne);
	static readonly float secondsBetweenOffCooldownAndEndStageOne = Mathf.Abs(attackAnimationDurationStageOne - attackOffCooldownStageOne);

	bool isTSOBasicAttackOnCooldown = false;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
		playerStats = player.GetComponent<PlayerStats>();
		playerTSOBasicAttack = player.GetComponent<PlayerTSOBasicAttack>();
    }

    void Update()
    {
        if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && !playerStats.isTSOBasicAttacking && !isTSOBasicAttackOnCooldown)
		{
			StartCoroutine("StageOne");
			playerTSOBasicAttack.TriggerPlayerAnimation();
		}
		if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && playerStats.isTSOBasicAttacking && !isTSOBasicAttackOnCooldown)
		{
			// StopCoroutine("StageOne");
			// StartCoroutine("StageTwo");
		}
    }

	IEnumerator StageOne()
	{
		anim.SetBool("basicAttacking", true);
		playerStats.isTSOBasicAttacking = true;
		isTSOBasicAttackOnCooldown = true;
		yield return new WaitForSeconds(attackHitboxSpawnStageOne);
		SpawnHitbox();
		yield return new WaitForSeconds(secondsBetweenAttackHitboxDespawnAndSpawnStageOne);
		DespawnHitbox();
		yield return new WaitForSeconds(secondsBetweenDespawnAndOffCooldownStageOne);
		isTSOBasicAttackOnCooldown = false;
		yield return new WaitForSeconds(secondsBetweenOffCooldownAndEndStageOne);
		playerStats.isTSOBasicAttacking = false;
		anim.SetBool("basicAttacking", false);
	}

	void SpawnHitbox()
	{
		GameObject referenceObject = Instantiate(theHitbox, gameObject.transform.position + new Vector3(2.5f * Mathf.Sign(gameObject.transform.localScale.x),-1f * Mathf.Sign(gameObject.transform.localScale.y),0), gameObject.transform.rotation);
		referenceObject.transform.parent = gameObject.transform;
		referenceObject.transform.localScale += new Vector3(2.5f * Mathf.Sign(gameObject.transform.localScale.x),2 * Mathf.Sign(gameObject.transform.localScale.y),0);
	}

	void DespawnHitbox()
	{
		GameObject existingHitbox = gameObject.transform.GetChild(0).gameObject;
		Destroy(existingHitbox);
	}
}
