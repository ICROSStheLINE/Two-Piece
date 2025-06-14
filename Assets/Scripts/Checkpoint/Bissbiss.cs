using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bissbiss : MonoBehaviour
{
	GameObject player;
	
	static readonly float tailWagAnimationDurationSpeedMultiplier = 0.5f;
	static readonly float tailWagAnimationDuration = 1f / tailWagAnimationDurationSpeedMultiplier;
	static readonly float gettingUpDurationSpeedMultiplier = 0.3f;
	static readonly float gettingUpAnimationDuration = 0.333f / gettingUpDurationSpeedMultiplier;
	static readonly float walkingDurationSpeedMultiplier = 0.5f;
	static readonly float walkingAnimationDuration = 0.333f / walkingDurationSpeedMultiplier;

	Animator anim;
	Vector3 destination = default(Vector3);

    void Start()
    {
		anim = GetComponent<Animator>();
		player = GameObject.FindWithTag("Player");
		
        InvokeRepeating("TailWag", 1, tailWagAnimationDuration + 4);
    }
	
	void FixedUpdate()
	{
		if (destination != default(Vector3))
		{
			transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
			if (transform.position == destination)
			{
				StartCoroutine(StopWalking());
				destination = default(Vector3);
			}
		}
	}

	public IEnumerator WalkToPosition(Vector3 destination_ = default(Vector3))
	{
		StopTailWag();
		anim.SetBool("isGettingUp", true);
		yield return new WaitForSeconds(gettingUpAnimationDuration/2);
		transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(destination_.x - transform.position.x), transform.localScale.y, transform.localScale.z);
		yield return new WaitForSeconds(gettingUpAnimationDuration/2);
		anim.SetBool("isGettingUp", false);
		anim.SetBool("isWalking", true);
		destination = new Vector3(destination_.x, transform.position.y, transform.position.z);
	}

	IEnumerator StopWalking()
	{
		anim.SetBool("isSittingDown", true);
		anim.SetBool("isWalking", false);
		yield return new WaitForSeconds(gettingUpAnimationDuration/2);
		transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(player.transform.localScale.x),transform.localScale.y,transform.localScale.z);
		yield return new WaitForSeconds(gettingUpAnimationDuration/2);
		anim.SetBool("isSittingDown", false);
		anim.SetBool("isBeingPet", true);
		yield return new WaitForSeconds(2);
		anim.SetBool("isBeingPet", false);
	}

	void TailWag()
	{
		anim.SetBool("WagTail", true);
		Invoke("StopTailWag", tailWagAnimationDuration);
	}

	void StopTailWag()
	{
		anim.SetBool("WagTail", false);
	}
}

