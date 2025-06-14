using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOHoverScript : MonoBehaviour
{
    GameObject player;
	PlayerStats playerStats;

	Vector3 wherePlayerWasFacing;
	Vector3 velocity = Vector3.zero; // This variable exists for a stupid reason LOL
	float followSpeed = 0.1f;
	[SerializeField] bool oldCode = true;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerStats = player.GetComponent<PlayerStats>();
	}

    void FixedUpdate()
    {
		// (If player is attacking, don't switch the direction of the orb, or else it'll switch the direction of its attack mid-way)
		if (!playerStats.isTSOBasicAttacking)
			CheckWherePlayerFacing();
		// The following line has the ball constantly moving towards the player at an incredibly slow speed. 
		// However, this speed increases multiplicitively when it's farther away from the player.
		
		// write if statement that checks if player walking animation is being played, if so then set orb speed to infinite
		Vector3 targetPosition = new Vector3(player.transform.position.x - Mathf.Sign(wherePlayerWasFacing.x)*1,player.transform.position.y + Mathf.Sign(player.transform.localScale.y)*1,gameObject.transform.position.z);
		if (oldCode)
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x - Mathf.Sign(wherePlayerWasFacing.x)*1,player.transform.position.y + Mathf.Sign(player.transform.localScale.y)*1,gameObject.transform.position.z), (10 + (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) * 5) + (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) * 5)) * Time.deltaTime);		
		else
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(wherePlayerWasFacing.x),Mathf.Abs(transform.localScale.y) * Mathf.Sign(wherePlayerWasFacing.y),gameObject.transform.localScale.z);
    }

	void CheckWherePlayerFacing()
	{
		wherePlayerWasFacing = player.transform.localScale;
	}
}
