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

	// Stage 1
	static readonly float attackAnimationDurationSpeedMultiplierStageOne = 1.5f;
	static readonly float attackAnimationDurationStageOne = 1f / attackAnimationDurationSpeedMultiplierStageOne;
	static readonly float attackAnimationFramesStageOne = 12;
	static readonly float attackHitboxSpawnStageOne = (4 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float attackHitboxDespawnStageOne = (7 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float attackFollowUpWindowStageOne = (8 / attackAnimationFramesStageOne) * attackAnimationDurationStageOne;
	static readonly float secondsBetweenAttackHitboxDespawnAndSpawnStageOne = Mathf.Abs(attackHitboxDespawnStageOne - attackHitboxSpawnStageOne);
	static readonly float secondsBetweenDespawnAndFollowUpWindowStageOne = Mathf.Abs(attackFollowUpWindowStageOne - attackHitboxDespawnStageOne);
	static readonly float secondsBetweenFollowUpWindowAndEndStageOne = Mathf.Abs(attackAnimationDurationStageOne - attackFollowUpWindowStageOne);
	// Stage 2
	static readonly float attackAnimationDurationSpeedMultiplierStageTwo = 1.5f;
	static readonly float attackAnimationDurationStageTwo = 1f / attackAnimationDurationSpeedMultiplierStageTwo;
	static readonly float attackAnimationFramesStageTwo = 12;
	static readonly float attackHitboxSpawnStageTwo = (4 / attackAnimationFramesStageTwo) * attackAnimationDurationStageTwo;
	static readonly float attackHitboxDespawnStageTwo = (7 / attackAnimationFramesStageTwo) * attackAnimationDurationStageTwo;
	static readonly float secondsBetweenAttackHitboxDespawnAndSpawnStageTwo = Mathf.Abs(attackHitboxDespawnStageTwo - attackHitboxSpawnStageTwo);
	static readonly float secondsBetweenDespawnAndEndStageTwo = Mathf.Abs(attackAnimationDurationStageTwo - attackHitboxDespawnStageTwo);
	bool followUpWindow = false;
	Coroutine attackCoroutine;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
		playerStats = player.GetComponent<PlayerStats>();
		playerTSOBasicAttack = player.GetComponent<PlayerTSOBasicAttack>();
    }

    void Update()
    {
		if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && playerStats.isSprinting)
		{
			playerTSOBasicAttack.TriggerPlayerAnimation("SprintAttack");
		}
        else if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && !playerStats.isTSOBasicAttacking && !followUpWindow)
		{
			attackCoroutine = StartCoroutine("StageOne");
			playerTSOBasicAttack.TriggerPlayerAnimation("StageOne");
		}
		else if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && followUpWindow)
		{
			if (attackCoroutine != null) StopCoroutine(attackCoroutine);
			attackCoroutine = StartCoroutine("StageTwo");
			playerTSOBasicAttack.TriggerPlayerAnimation("StageTwo");
		}
    }

	public void StartSpringStageOne()
		{StartCoroutine("SprintStageOne");}

	IEnumerator SprintStageOne()
    {
        anim.SetInteger("sprintAttackStage", 1);
		playerStats.isTSOBasicAttacking = true;
		yield return new WaitForSeconds(attackHitboxSpawnStageOne);
		SpawnHitbox();
		yield return new WaitForSeconds(secondsBetweenAttackHitboxDespawnAndSpawnStageOne);
		DespawnHitbox();
		yield return new WaitForSeconds(secondsBetweenDespawnAndFollowUpWindowStageOne);
		followUpWindow = true;
		yield return new WaitForSeconds(secondsBetweenFollowUpWindowAndEndStageOne);
		playerStats.isTSOBasicAttacking = false;
		anim.SetInteger("sprintAttackStage", 0);
		yield return new WaitForSeconds(1f);
		followUpWindow = false;
    }

	IEnumerator StageOne()
	{
		anim.SetInteger("basicAttackStage", 1);
		playerStats.isTSOBasicAttacking = true;
		yield return new WaitForSeconds(attackHitboxSpawnStageOne);
		SpawnHitbox();
		yield return new WaitForSeconds(secondsBetweenAttackHitboxDespawnAndSpawnStageOne);
		DespawnHitbox();
		yield return new WaitForSeconds(secondsBetweenDespawnAndFollowUpWindowStageOne);
		followUpWindow = true;
		yield return new WaitForSeconds(secondsBetweenFollowUpWindowAndEndStageOne);
		playerStats.isTSOBasicAttacking = false;
		anim.SetInteger("basicAttackStage", 0);
		yield return new WaitForSeconds(1f);
		followUpWindow = false;
	}

	IEnumerator StageTwo()
	{
		followUpWindow = false;
		anim.SetInteger("basicAttackStage", 2);
		playerStats.isTSOBasicAttacking = true;
		yield return new WaitForSeconds(attackHitboxSpawnStageTwo);
		SpawnHitbox();
		yield return new WaitForSeconds(secondsBetweenAttackHitboxDespawnAndSpawnStageTwo);
		DespawnHitbox();
		yield return new WaitForSeconds(secondsBetweenDespawnAndEndStageTwo);
		playerStats.isTSOBasicAttacking = false;
		anim.SetInteger("basicAttackStage", 0);
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
