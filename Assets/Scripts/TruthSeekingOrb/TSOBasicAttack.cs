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
	TrailRenderer trailRenderer;

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
	Coroutine movementCoroutine;
	

    void Start()
    {
		player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
		playerStats = player.GetComponent<PlayerStats>();
		playerTSOBasicAttack = player.GetComponent<PlayerTSOBasicAttack>();
		trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
		CheckForAttackInput();
    }

	void CheckForAttackInput()
	{
		if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && playerStats.isSprinting)
		{
			playerTSOBasicAttack.TriggerPlayerAnimation("SprintAttack");
		}
        else if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && !playerStats.isTSOBasicAttacking && !followUpWindow)
		{
			attackCoroutine = StartCoroutine("StageOne");
			movementCoroutine = StartCoroutine("StageOneMovements");
			playerTSOBasicAttack.TriggerPlayerAnimation("StageOne");
		}
		else if (Input.GetKeyDown(playerStats.basicAttackKey) && playerStats.canTSOAttack && followUpWindow)
		{
			if (attackCoroutine != null) StopCoroutine(attackCoroutine);
			if (movementCoroutine != null) StopCoroutine(movementCoroutine);
			attackCoroutine = StartCoroutine("StageTwo");
			movementCoroutine = StartCoroutine("StageTwoMovements");
			playerTSOBasicAttack.TriggerPlayerAnimation("StageTwo");
		}
	}

	IEnumerator SprintStageOneMovements()
	{
		float frameRate = 1f / 100f;
		int frameIndex = 0;
		playerStats.TSOHover = false;
		trailRenderer.emitting = true;
		Vector3 simulatedPosition = transform.position;
		Vector3 attackTarget = new Vector3(player.transform.position.x + Mathf.Sign(player.transform.localScale.x) * 10, transform.position.y, transform.position.z);
		Vector3 startPos = transform.position;
		// This for loop makes it follow the parabola on the way there
		for (float secondsPassed = 0; secondsPassed < attackHitboxDespawnStageOne; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / (attackHitboxDespawnStageOne);
			Vector3 axisDir = (attackTarget - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x)).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, attackTarget, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		startPos = transform.position;
		frameIndex = 0;
		float theRestOfTheAnimationDuration = secondsBetweenDespawnAndFollowUpWindowStageOne + secondsBetweenFollowUpWindowAndEndStageOne;
		// This for loop makes it follow the parabola on the way back
		for (float secondsPassed = 0; secondsPassed < theRestOfTheAnimationDuration; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / theRestOfTheAnimationDuration;
			Vector3 axisDir = (player.transform.position - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x)).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, player.transform.position, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		playerStats.TSOHover = true;
		trailRenderer.emitting = false;
	}

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

	IEnumerator StageOneMovements()
	{
		float firstFrame = (1/attackAnimationFramesStageOne) / attackAnimationDurationStageOne;
		float frameRate = 1f / 100f;
		int frameIndex = 0;
		playerStats.TSOHover = false;
		trailRenderer.emitting = true;
		yield return new WaitForSeconds(firstFrame);
		Vector3 simulatedPosition = transform.position;
		Vector3 attackTarget = new Vector3(player.transform.position.x + Mathf.Sign(player.transform.localScale.x) * 10, transform.position.y, transform.position.z);
		Vector3 startPos = transform.position;
		// This for loop makes it follow the parabola on the way there
		for (float secondsPassed = 0; secondsPassed < attackHitboxDespawnStageOne - firstFrame; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / (attackHitboxDespawnStageOne - firstFrame);
			Vector3 axisDir = (attackTarget - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x)).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, attackTarget, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		startPos = transform.position;
		frameIndex = 0;
		float theRestOfTheAnimationDuration = secondsBetweenDespawnAndFollowUpWindowStageOne + secondsBetweenFollowUpWindowAndEndStageOne - firstFrame;
		// This for loop makes it follow the parabola on the way back
		for (float secondsPassed = 0; secondsPassed < theRestOfTheAnimationDuration; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / theRestOfTheAnimationDuration;
			Vector3 axisDir = (player.transform.position - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x)).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, player.transform.position, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		playerStats.TSOHover = true;
		trailRenderer.emitting = false;
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

	IEnumerator StageTwoMovements()
	{
		float firstFrame = (1/attackAnimationFramesStageOne) / attackAnimationDurationStageOne;
		float frameRate = 1f / 100f;
		int frameIndex = 0;
		playerStats.TSOHover = false;
		trailRenderer.emitting = true;
		yield return new WaitForSeconds(firstFrame);
		Vector3 simulatedPosition = transform.position;
		Vector3 attackTarget = new Vector3(player.transform.position.x + Mathf.Sign(player.transform.localScale.x) * 10, transform.position.y, transform.position.z);
		Vector3 startPos = transform.position;
		// This for loop makes it follow the parabola on the way there
		for (float secondsPassed = 0; secondsPassed < attackHitboxDespawnStageOne - firstFrame; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / (attackHitboxDespawnStageOne - firstFrame);
			Vector3 axisDir = (attackTarget - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x) * -1f).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, attackTarget, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		startPos = transform.position;
		frameIndex = 0;
		float theRestOfTheAnimationDuration = secondsBetweenDespawnAndFollowUpWindowStageOne + secondsBetweenFollowUpWindowAndEndStageOne - firstFrame;
		// This for loop makes it follow the parabola on the way back
		for (float secondsPassed = 0; secondsPassed < theRestOfTheAnimationDuration; secondsPassed += Time.deltaTime)
		{
			float t = secondsPassed / theRestOfTheAnimationDuration;
			Vector3 axisDir = (player.transform.position - startPos).normalized;
			Vector3 perpDir = Vector3.Cross(axisDir, Vector3.forward * Mathf.Sign(player.transform.localScale.x) * -1f).normalized;
			Vector3 basePoint = Vector3.Lerp(startPos, player.transform.position, t);
			float height = 1.5f;
			float offset = 4 * t * (1 - t) * height;
			simulatedPosition = basePoint + perpDir * offset;
			yield return null;
			if (secondsPassed >= frameIndex * frameRate)
			{
				transform.position = simulatedPosition;
				frameIndex++;
			}
		}
		playerStats.TSOHover = true;
		trailRenderer.emitting = false;
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
		GameObject referenceObject = Instantiate(theHitbox, transform.GetComponent<Renderer>().bounds.center, gameObject.transform.rotation);
		referenceObject.transform.parent = gameObject.transform;
		referenceObject.transform.localScale += new Vector3(2.5f * Mathf.Sign(gameObject.transform.localScale.x),2 * Mathf.Sign(gameObject.transform.localScale.y),0);
	}

	void DespawnHitbox()
	{
		GameObject existingHitbox = gameObject.transform.GetChild(0).gameObject;
		Destroy(existingHitbox);
	}

	public void StartSprintStageOne()
	{
		if (attackCoroutine != null) StopCoroutine(attackCoroutine);
		if (movementCoroutine != null) StopCoroutine(movementCoroutine);
		attackCoroutine = StartCoroutine("SprintStageOne");
		movementCoroutine = StartCoroutine("SprintStageOneMovements");
	}
}
