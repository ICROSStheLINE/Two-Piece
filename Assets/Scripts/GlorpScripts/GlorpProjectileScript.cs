using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlorpProjectileScript : MonoBehaviour
{
    Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	GameObject healthBar;
	HealthScript healthScript;
	Animator anim;
	
	float movementSpeed = 22f;
	bool isFizzling = false;
	
	static readonly float animationDurationSpeedMultiplier = 1f;
	static readonly float animationDuration = 0.833f / animationDurationSpeedMultiplier;
	
	static readonly float fizzleAnimationDurationSpeedMultiplier = 1f;
	static readonly float fizzleAnimationDuration = 0.417f / fizzleAnimationDurationSpeedMultiplier;
	
    void Start()
    {
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		
		if (gameObject.transform.rotation.z != 0)
		{
			movementSpeed = movementSpeed * -1;
			spriteRenderer.flipY = !spriteRenderer.flipY;
		}
		Invoke("KILLYOURSELF", animationDuration);
    }

    void FixedUpdate()
    {
		rb.position += new Vector2(movementSpeed * Time.deltaTime,0);
    }
	
	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isFizzling)
		{
			if (collision.gameObject.tag == "PlayerShieldHitbox")
			{
				Fizzle();
			}
			else if (collision.gameObject.tag == "Player")
			{
				healthScript.LoseHealthBy(1);
				Fizzle();
			}
		}
	}
	
	
	void Fizzle()
	{
		CancelInvoke("KILLYOURSELF");
		Invoke("KILLYOURSELF", fizzleAnimationDuration);
		movementSpeed = movementSpeed / 3;
		anim.SetBool("Fizzle", true);
		isFizzling = true;
	}
}
