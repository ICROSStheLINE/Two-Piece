using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlorpAttackingScript : MonoBehaviour
{
	GlorpWalkingScript glorpWalkingScript;
	[SerializeField] GameObject glorpProjectile;
	
    Rigidbody2D rb;
	Animator anim;
	
	bool isAttacking = false;
	bool attackOnCooldown = false;
	
	
    void Start()
    {
		glorpWalkingScript = GetComponent<GlorpWalkingScript>();
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
    }

    void Update()
    {
        if ((isAttacking) && (!attackOnCooldown))
		{
			attackOnCooldown = true;
			glorpWalkingScript.PauseMovementDueToAttack();
			
			anim.SetBool("isAttacking", true);
			Invoke("DeactivateGlorpAttack", 0.75f);
			Invoke("ResetAttackCooldown", 2f);
			Invoke("DeactivateAttackAnimation", 0.75f);
		}
    }
	
	public void ActivateGlorpAttack()
	{
		isAttacking = true;
	}
	
	void DeactivateGlorpAttack()
	{
		isAttacking = false;
		glorpWalkingScript.UnpauseMovementDueToAttack();
	}
	
	void ResetAttackCooldown()
	{
		attackOnCooldown = false;
	}
	
	void DeactivateAttackAnimation()
	{
		anim.SetBool("isAttacking", false);
		if (Mathf.Sign(gameObject.transform.localScale.x) == 1)
		{
			Instantiate(glorpProjectile, (gameObject.transform.position + new Vector3(2,0,0)), Quaternion.Euler(0,0,0));
		}
		else
		{
			Instantiate(glorpProjectile, (gameObject.transform.position - new Vector3(2,0,0)), Quaternion.Euler(0,0,180));
		}
	}
}
