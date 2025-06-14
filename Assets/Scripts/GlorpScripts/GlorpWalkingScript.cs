using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlorpWalkingScript : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	GameObject player;
	
	[SerializeField] bool isWalkingTowardsPlayer = false;
	[SerializeField] float movementSpeed = 5;
	bool isCurrentlyAttacking = false;
	float glorpXScale;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		glorpXScale = gameObject.transform.localScale.x;
    }

    void Update()
    {
		if ((isWalkingTowardsPlayer) && (!isCurrentlyAttacking))
		{
			rb.position += new Vector2(((movementSpeed * Time.deltaTime) * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x)), 0);
			gameObject.transform.localScale = new Vector3(glorpXScale * Mathf.Sign(player.transform.position.x - gameObject.transform.position.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		}
		anim.SetBool("isWalking", isWalkingTowardsPlayer);
    }

	public void ActivateGlorpAggro()
	{
		isWalkingTowardsPlayer = true;
	}

	public void PauseMovementDueToAttack()
	{
		isCurrentlyAttacking = true;
	}

	public void UnpauseMovementDueToAttack()
	{
		isCurrentlyAttacking = false;
	}
}
