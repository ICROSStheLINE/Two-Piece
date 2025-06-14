using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpDeathScript : MonoBehaviour
{
    Animator anim;
	BoxCollider2D boxCollider;
	[SerializeField] GameObject shlorpSoul;
	ShlorpCombatScript shlorpCombatScript;
	
	bool scheduledToDie = false;
	
    void Start()
    {
        anim = GetComponent<Animator>();
		shlorpCombatScript = GetComponent<ShlorpCombatScript>();
		boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (anim.GetBool("isDying"))
		{
			if (!scheduledToDie)
			{
				Invoke("KILLYOURSELF", (0.833f / 0.35f));
				scheduledToDie = true;
			}
			anim.SetInteger("teleportStage", 0);
			anim.SetBool("isAttacking", false);
			shlorpCombatScript.CancelInvoke();
			shlorpCombatScript.Invoke("SetVisibleAgain", 0);
			shlorpCombatScript.enabled = false;
			boxCollider.excludeLayers = 11000000;
			// LayerMasks are stupid.
		}
    }

	void KILLYOURSELF()
	{
		Instantiate(shlorpSoul, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}