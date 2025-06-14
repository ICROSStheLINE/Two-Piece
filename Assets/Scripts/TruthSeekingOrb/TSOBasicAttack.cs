using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOBasicAttack : MonoBehaviour
{
    [SerializeField] GameObject theHitbox;
	GameObject player;
	Animator anim;
	PlayerStats playerStats;

	static readonly float attackAnimationDurationSpeedMultiplier = 2;
	static readonly float attackAnimationDuration = 1 / attackAnimationDurationSpeedMultiplier;
	static readonly float attackAnimationFrames = 12;
	static readonly float attackHitboxSpawn = (4 / attackAnimationFrames) * attackAnimationDuration;
	static readonly float attackHitboxDespawn = (7 / attackAnimationFrames) * attackAnimationDuration;

	public bool isAttacking = false;
	bool isTSOBasicAttackOnCooldown = false;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
		playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(playerStats.basicAttackKey)) && (!playerStats.isTSOBasicAttacking) && (!isTSOBasicAttackOnCooldown))
		{
			anim.SetBool("basicAttacking", true);
			playerStats.isTSOBasicAttacking = true;
			isTSOBasicAttackOnCooldown = true;
			isAttacking = true;
			Invoke("SpawnHitbox", attackHitboxSpawn);
			Invoke("DespawnHitbox", attackHitboxDespawn);
			Invoke("ResetAttackCooldown", attackAnimationDuration);
		}
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

	void ResetAttackCooldown()
	{
		isAttacking = false;
		playerStats.isTSOBasicAttacking = false;
		isTSOBasicAttackOnCooldown = false;
		anim.SetBool("basicAttacking", false);
	}
}
