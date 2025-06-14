using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossJaw : MonoBehaviour
{
    IceBossStats iceBossStats;
	GameObject bossHead;
	GameObject player;
	
	Vector3 velocity = Vector3.zero; // This variable exists for a stupid reason LOL
	float followSpeed = 0.1f;

    void Start()
    {
        iceBossStats = transform.parent.GetComponent<IceBossStats>();
		bossHead = gameObject.transform.parent.transform.GetChild(0).gameObject;
		player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
		
		Vector3 targetPosition = bossHead.transform.position - new Vector3(0,6.5f,0);
		
		if (targetPosition.x - transform.position.x < -1) // If the target position is to the LEFT of this gameobjects position
			TiltLeft();
		else if (targetPosition.x - transform.position.x > 1) // If the target position is to the RIGHT of this gameobjects position
			TiltRight();
		else if (!iceBossStats.iceBossMidOrb)
			TiltReset();
		
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
		
		
		if (iceBossStats.iceBossAttemptOrb && !iceBossStats.iceBossMidOrb)
		{
			Invoke("TiltBasedOffDirection", 0.25f);
		}
    }
	
	/*void ShakeRetardedly()
	{
		Invoke("TiltRight", 0.15f);
		Invoke("TiltLeft", 0.3f);
		Invoke("TiltRight", 0.45f);
		Invoke("TiltLeft", 0.6f);
		Invoke("TiltRight", 0.75f);
		Invoke("TiltLeft", 0.9f);
		Invoke("TiltReset", 1f);
	}*/
	
	void TiltRight()
	{
		transform.rotation = Quaternion.Euler(0,0,330);
	}
	
	void TiltLeft()
	{
		transform.rotation = Quaternion.Euler(0,0,30);
	}
	
	void TiltReset()
	{
		transform.rotation = Quaternion.Euler(0,0,0);
	}

	void TiltBasedOffDirection()
	{
		if (Mathf.Sign(player.transform.position.x - bossHead.transform.position.x) == -1)
		{
			TiltReset();
			TiltLeft();
		}
		else
		{
			TiltReset();
			TiltRight();
		}
	}
}
