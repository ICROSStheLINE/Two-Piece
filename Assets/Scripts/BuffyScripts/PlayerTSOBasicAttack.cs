using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTSOBasicAttack : MonoBehaviour
{
    Animator anim;
	PlayerStats playerStats;

    static readonly float animationDurationSpeedMultiplierStageOne = 1f;
	static readonly float animationDurationStageOne = 1f / animationDurationSpeedMultiplierStageOne;
	// static readonly float animationFramesStageOne = 12f;
    static readonly float animationDurationSpeedMultiplierStageTwo = 1f;
    static readonly float animationDurationStageTwo = 1f / animationDurationSpeedMultiplierStageTwo;
    // static readonly float animationFramesStageTwo = 12f;

    bool midAnimation = false;
    Coroutine attackCoroutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        
    }

    public void TriggerPlayerAnimation(string stage)
    {
        if (playerStats.isTSOBasicAttacking) // If he is basic attacking,
        {
            if (!playerStats.isSprinting) // Check if he's sprinting or not. If he isn't...
            {
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(stage);
            }
        }
    }

    IEnumerator StageOne()
    {
        midAnimation = true;
        playerStats.playerCanMove = false; // Make him not be able to move or turn
        playerStats.playerCanDash = false;
        playerStats.ResetPlayerDashCooldown();
        anim.SetInteger("basicAttackStage", 1); // Make him do the attack hand motions
        yield return new WaitForSeconds(animationDurationStageOne);
        midAnimation = false;
        playerStats.playerCanMove = true;
        playerStats.playerCanDash = true;
        anim.SetInteger("basicAttackStage", 0);
    }

    IEnumerator StageTwo()
    {
        midAnimation = true;
        playerStats.playerCanMove = false;
        playerStats.playerCanDash = false;
        playerStats.ResetPlayerDashCooldown();
        anim.SetInteger("basicAttackStage", 2);
        yield return new WaitForSeconds(animationDurationStageTwo);
        midAnimation = false;
        playerStats.playerCanMove = true;
        playerStats.playerCanDash = true;
        anim.SetInteger("basicAttackStage", 0);
    }
}
