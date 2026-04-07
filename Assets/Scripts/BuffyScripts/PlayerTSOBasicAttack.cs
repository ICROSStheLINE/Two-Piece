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

    bool midAnimation = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.isTSOBasicAttacking && !midAnimation) // If he is basic attacking,
        {
            if (!playerStats.isSprinting) // Check if he's sprinting or not. If he isn't...
            {
                StartCoroutine("StageOne");
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
}
