using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShielding : MonoBehaviour
{
	Animator anim;
	[SerializeField] GameObject theHitbox;
	PlayerStats playerStats;

	PlayerTracker playerTracker;

	readonly float shieldingStageOneSpeed = (0.5f / 2);
	readonly float shieldingStageTwoSpeed = (0.5f / 2);

	//[HideInInspector] public bool playerStats.playerMidShielding = false;
	bool playerShieldIsUp = false;

    void Start()
    {
        anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();

		playerTracker = GameObject.FindWithTag("MainCamera").GetComponent<PlayerTracker>();
    }

    void Update()
    {
		if ((Input.GetKey(playerStats.orbShieldKey)) && !playerStats.playerMidActionNoDash && !playerStats.midCutscene)
		{
			playerStats.playerCanDash = false;
			playerStats.ResetPlayerDashCooldown();
			playerStats.playerCanMove = false;


			ShieldStartup();
			Invoke("ShieldMiddle", shieldingStageOneSpeed);
		}
		else if ((!Input.GetKey(playerStats.orbShieldKey)) && (!playerStats.playerMidGravityShift) && (!playerStats.playerMidTeleport) && (playerShieldIsUp))
		{
			ShieldEnd();
			Invoke("FinishShielding", shieldingStageTwoSpeed);
		}
    }

	void ShieldStartup()
	{
		anim.SetInteger("shieldingStage", 1);
		playerStats.playerMidShielding = true;
	}

	void ShieldMiddle()
	{
		anim.SetInteger("shieldingStage", 2);
		playerShieldIsUp = true;
		GameObject referenceObject = Instantiate(theHitbox, gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.localScale.x),0,0), gameObject.transform.rotation);
		referenceObject.transform.parent = gameObject.transform;
	}

	void ShieldEnd()
	{
		anim.SetInteger("shieldingStage", 3);
		playerShieldIsUp = false;
		Destroy(GameObject.FindWithTag("PlayerShieldHitbox"));
	}

	void FinishShielding()
	{
		playerStats.playerCanDash = true;
		playerStats.playerCanMove = true;

		playerStats.playerMidShielding = false;
		anim.SetInteger("shieldingStage", 0);
	}
}
