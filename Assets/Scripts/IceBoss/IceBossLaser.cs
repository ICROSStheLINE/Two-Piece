using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossLaser : MonoBehaviour
{
    BoxCollider2D boxCollider;
	GameObject healthBar;
	HealthScript healthScript;
	GameObject bossEye;
	
	[SerializeField] GameObject laserIndicatorPrefab;
	
	static readonly float animationDurationSpeedMultiplier = 1.5f;
	static readonly float animationDuration = 2.5f / animationDurationSpeedMultiplier;
	static readonly float animationFrames = 30;
	static readonly float laserSpawnFrame = (12 / animationFrames) * animationDuration;
	
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
		bossEye = GameObject.FindWithTag("Ice Boss").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		GameObject laserIndicator = Instantiate(laserIndicatorPrefab, bossEye.transform.position, Quaternion.Euler(0,0,0));
		laserIndicator.transform.parent = bossEye.transform;
		
		Invoke("Extend", laserSpawnFrame);
		Invoke("Extend", (12 / animationFrames) * animationDuration);
		Invoke("Extend", (13 / animationFrames) * animationDuration);
		Invoke("Extend", (14 / animationFrames) * animationDuration);
		Invoke("Extend", (15 / animationFrames) * animationDuration);
		Invoke("Extend", (16 / animationFrames) * animationDuration);
		Invoke("Extend", (17 / animationFrames) * animationDuration);
		Invoke("RemoveHitbox", (27 / animationFrames) * animationDuration);
		Invoke("KILLYOURSELF", animationDuration);
    }

    void Extend()
    {
        boxCollider.size = boxCollider.size + new Vector2(0,6);
    }
	
	void RemoveHitbox()
	{
		boxCollider.size = new Vector2(0.1f,0.1f);
	}
	
	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			healthScript.LoseHealthBy(1);
		}
	}
}
