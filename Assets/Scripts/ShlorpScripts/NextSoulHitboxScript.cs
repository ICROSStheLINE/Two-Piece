using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSoulHitboxScript : MonoBehaviour
{
    bool activateTrigger = false;
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }
	
	public void DeleteTheNextSoulLaser()
	{
		activateTrigger = true;
	}
	
	void OnTriggerStay2D (Collider2D collision)
	{
		if (activateTrigger)
		{
			if (collision.gameObject.tag == "ShlorpSoulLaserBeam")
				collision.gameObject.transform.GetChild(0).gameObject.GetComponent<NextSoulHitboxScript>().DeleteTheNextSoulLaser();
				Destroy(collision.gameObject);
		}
	}
}
