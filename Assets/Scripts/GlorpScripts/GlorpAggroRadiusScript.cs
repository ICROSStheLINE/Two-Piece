using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlorpAggroRadiusScript : MonoBehaviour
{
	GlorpWalkingScript glorpWalkingScript;
	GlorpAttackingScript glorpAttackingScript;
	
	bool activateAggo = false;
	bool activateAttack = false;
	
    void Start()
    {
        glorpWalkingScript = transform.parent.GetComponent<GlorpWalkingScript>();
		glorpAttackingScript = transform.parent.GetComponent<GlorpAttackingScript>();
    }

    void Update()
    {
        if (activateAggo)
		{
			glorpWalkingScript.ActivateGlorpAggro();
		}
		if (activateAttack)
		{
			glorpAttackingScript.ActivateGlorpAttack();
		}
    }
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			activateAggo = true;
			Invoke("ACTIVATEATTACKONEMILLISECONDLATERSOTHATHECANWALKANDTHEREFORECHANGEDIRECTIONBEFORESHOOTINGIMMEDIATELY", 0.01f);
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			activateAttack = false;
		}
	}
	
	void ACTIVATEATTACKONEMILLISECONDLATERSOTHATHECANWALKANDTHEREFORECHANGEDIRECTIONBEFORESHOOTINGIMMEDIATELY()
	{
		activateAttack = true;
	}
}
