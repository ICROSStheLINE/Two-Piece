using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceBossProjectile : MonoBehaviour
{
    Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	GameObject healthBar;
	HealthScript healthScript;
	BoxCollider2D boxCollider;
	IceBossStats iceBossStats;
	GameObject iceBossHead;
	GameObject iceBossJaw;
	
	[SerializeField] GameObject projectileTrail;
	[SerializeField] GameObject projectileSonicBoom;
	GameObject trail;
	GameObject sonicBoom;
	
	float movementSpeed = 35f;
	
	float chargeTime;
	
	
	void Awake()
	{
		iceBossStats = GameObject.FindWithTag("Ice Boss").GetComponent<IceBossStats>();
		chargeTime = iceBossStats.iceBossOrbChargeTime;
	}
	
    void Start()
    {
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		iceBossHead = GameObject.FindWithTag("Ice Boss").transform.GetChild(0).gameObject;
		iceBossJaw = GameObject.FindWithTag("Ice Boss").GetComponent<IceBossStats>().getIceBossJaw;
		
		if (gameObject.transform.rotation.z != 0)
		{
			movementSpeed = movementSpeed * -1;
			spriteRenderer.flipY = !spriteRenderer.flipY;
		}
		
		
		Invoke("Fire", chargeTime);
		InvokeRepeating("SpawnSonicBoom", chargeTime, 0.2f);
    }

    void FixedUpdate()
    {
		if (boxCollider.enabled)
			rb.position += new Vector2(movementSpeed * Time.deltaTime,0);
		else
			transform.position = iceBossHead.transform.position - new Vector3(0,5,0);
    }
	
	void KILLYOURSELF()
	{
		CancelInvoke("SpawnSonicBoom");
		Destroy(sonicBoom);
		Destroy(trail);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerShieldHitbox")
		{
			KILLYOURSELF();
		}
		else if (collision.gameObject.tag == "Player")
		{
			healthScript.LoseHealthBy(1);
			KILLYOURSELF();
		}
		else if (collision.gameObject.tag == "Floor or Wall")
		{
			KILLYOURSELF();
		}
	}
	
	void Fire()
	{
		trail = Instantiate(projectileTrail, transform.position, transform.rotation);
		trail.GetComponent<IceBossProjectileTrail>().iceBossProjectileTrailParent = gameObject;
		boxCollider.enabled = true;
	}
	
	void SpawnSonicBoom()
	{
		sonicBoom = Instantiate(projectileSonicBoom, transform.position + new Vector3(Mathf.Sign(movementSpeed) * 1.5f,0,0), transform.rotation);
	}
}
