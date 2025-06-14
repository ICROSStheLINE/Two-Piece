using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
		if (Input.GetKeyDown(playerStats.dashKey) && (playerStats.playerIsDashing == false) && (playerStats.playerCanDash == true))
		{
			rb.velocity = new Vector2(40 * Mathf.Sign(gameObject.transform.localScale.x),0);
			anim.SetBool("isDashing", true);
			playerStats.playerIsDashing = true;
			playerStats.playerIsDashingButResets1MillisecondEarlier = true;
			playerStats.Invoke("ResetPlayerDashCooldown", 0.5f);
			playerStats.Invoke("ResetTheIsDashingButResets1MillisecondEarlierVariableSoThatTurningWhileDashingIsntGlitched", 0.49f);
		}
    }
	
	
}
