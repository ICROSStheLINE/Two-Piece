using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordHitbox : MonoBehaviour
{
	Animator shlorpNGlorpAnimator;
	IceBossStats iceBossStats;
	
    void Start()
    {
		iceBossStats = GameObject.FindWithTag("Ice Boss").GetComponent<IceBossStats>();
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.gameObject.tag == "Glorp") || (collision.gameObject.tag == "Shlorp"))
		{
			shlorpNGlorpAnimator = collision.GetComponent<Animator>();
			shlorpNGlorpAnimator.SetBool("isDying", true);
		}
		else if ((collision.gameObject.tag == "Ice Boss Head") || (collision.gameObject.tag == "Ice Boss Jaw"))
		{
			iceBossStats.IceBossLoseHealthBy(1);
		}
	}
}