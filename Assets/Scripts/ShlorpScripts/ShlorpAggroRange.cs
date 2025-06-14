using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpAggroRange : MonoBehaviour
{
    [HideInInspector] public bool isWakingUp = false;
	[HideInInspector] public bool playerInAggroRange = false;
	bool isNowAwake = false;
    Animator anim;
	
	
    void Start()
    {
        anim = gameObject.transform.parent.GetComponent<Animator>();
    }
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (!isNowAwake)
			{
				WakeUp();
			}
			
			playerInAggroRange = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerInAggroRange = false;
		}
	}
	
	void WakeUp()
	{
		anim.SetBool("timeToWakeUp", true);
		isWakingUp = true;
		Invoke("ResetIsWakingUpVar", 1);
		isNowAwake = true;
	}
	
	void ResetIsWakingUpVar()
	{
		isWakingUp = false;
	}
}
