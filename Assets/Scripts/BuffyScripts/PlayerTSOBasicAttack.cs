using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTSOBasicAttack : MonoBehaviour
{
    Animator anim;
	PlayerStats playerStats;

    static readonly float animationDurationSpeedMultiplierStageOne = 1.5f;
	static readonly float animationDurationStageOne = 1f / animationDurationSpeedMultiplierStageOne;
	// static readonly float animationFramesStageOne = 12f;
    static readonly float animationDurationSpeedMultiplierStageTwo = 1.5f;
    static readonly float animationDurationStageTwo = 1f / animationDurationSpeedMultiplierStageTwo;
    // static readonly float animationFramesStageTwo = 12f;
    static readonly float animationDurationSpeedMultiplierSprint = 1f;
    static readonly float animationDurationSprint = 0.750f / animationDurationSpeedMultiplierSprint;

    Coroutine attackCoroutine;
    GameObject truthSeekingOrb;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        truthSeekingOrb = GameObject.FindWithTag("Truth Seeking Orb");
    }

    void Update()
    {
        
    }

    public void TriggerPlayerAnimation(string stage)
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(stage);
    }

    IEnumerator SprintAttack()
    {
        truthSeekingOrb.SetActive(false);
        anim.SetInteger("sprintAttackStage", 1);
        playerStats.playerCanMove = false;
        playerStats.playerCanDash = false;
        playerStats.isSprinting = false;
        playerStats.ResetPlayerDashCooldown();
        yield return new WaitForSeconds(animationDurationSprint);
        // TODO: Make the orb spawn like 1 or 2 frames before the animation finishes.
        truthSeekingOrb.SetActive(true);
        truthSeekingOrb.transform.GetComponent<TSOBasicAttack>().StartStageOne();
        // TODO: Also make the orb spawn a bit forward so it looks like it's getting launched out of buffy's hand
        anim.SetInteger("sprintAttackStage", 0);
        playerStats.playerCanMove = true;
        playerStats.playerCanDash = true;
    }

    IEnumerator StageOne()
    {
        playerStats.playerCanMove = false; // Make him not be able to move or turn
        playerStats.playerCanDash = false;
        playerStats.ResetPlayerDashCooldown();
        anim.SetInteger("basicAttackStage", 1); // Make him do the attack hand motions
        yield return new WaitForSeconds(animationDurationStageOne);
        playerStats.playerCanMove = true;
        playerStats.playerCanDash = true;
        anim.SetInteger("basicAttackStage", 0);
    }

    IEnumerator StageTwo()
    {
        playerStats.playerCanMove = false;
        playerStats.playerCanDash = false;
        playerStats.ResetPlayerDashCooldown();
        anim.SetInteger("basicAttackStage", 2);
        yield return new WaitForSeconds(animationDurationStageTwo);
        playerStats.playerCanMove = true;
        playerStats.playerCanDash = true;
        anim.SetInteger("basicAttackStage", 0);
    }
}
