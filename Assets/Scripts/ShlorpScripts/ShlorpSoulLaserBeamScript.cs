using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpSoulLaserBeamScript : MonoBehaviour
{
	GameObject healthBar;
	HealthScript healthScript;
	NextSoulHitboxScript nextSoulLaserHitboxScript;

	readonly float laserAnimationTime = (0.25f / 0.5f);

    void Start()
    {
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
		nextSoulLaserHitboxScript = gameObject.transform.GetChild(0).gameObject.GetComponent<NextSoulHitboxScript>();
		
        Invoke("ShlorpSoulLaserShouldKILLYOURSELF", laserAnimationTime);
    }

	public void ShlorpSoulLaserShouldKILLYOURSELF()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
			healthScript.LoseHealthBy(1);
		if (collision.gameObject.tag == "Floor or Wall")
		{
			// Create an empty gameobject hitbox at the very end of the laserbeam object and have any laserbeam object touching that get deleted
			nextSoulLaserHitboxScript.DeleteTheNextSoulLaser();
		}
	}
}
