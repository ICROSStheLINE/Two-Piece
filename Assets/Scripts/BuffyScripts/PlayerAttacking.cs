/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
	[SerializeField] GameObject theHitbox;
	// This variable exists to make sure only one hitbox spawns at a time (even if you hold down the attacking button)
	bool HitboxSingularityTimer = false;
	Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey("g"))
		{
			AttackingAnimation();

			if (HitboxSingularityTimer == false) 
			{
				//The current animation length is 0.583 seconds. I could set that to a variable later for ease of access.
				Invoke("SpawnHitbox", 0.3f);
				Invoke("ResetTimer", 0.583f);
			}
			HitboxSingularityTimer = true;

			Invoke("AttackingAnimationDeactivation", 0.1f);
		}
    }


	void SpawnHitbox()
	{
		//Spawns the Hitbox prefab and sets it as a child of the player object so that it follows the player around
		GameObject referenceObject = Instantiate(theHitbox, gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.localScale.x),0,0), gameObject.transform.rotation);
		referenceObject.transform.parent = gameObject.transform;
	}

	//Sets the timer variable to false, thus granting permission for another hitbox to be created
	void ResetTimer()
	{
		HitboxSingularityTimer = false;
	}

	//Sets the "isAttacking" variable in the animation controller to true. 
	//This causes the attacking animation to trigger since it waits for that variable to be true.
	void AttackingAnimation()
	{
		anim.SetBool("isAttacking", true);
	}

	//Sets the "isAttacking" variable to false, thus ending the animation (AFTER IT FINISHES PLAYING IN ITS ENTIRETY)
	void AttackingAnimationDeactivation()
	{
		anim.SetBool("isAttacking", false);
	}
}
*/