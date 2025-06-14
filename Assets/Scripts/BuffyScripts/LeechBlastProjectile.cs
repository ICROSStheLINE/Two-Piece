using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechBlastProjectile : MonoBehaviour
{
	Rigidbody2D rb;
	GameObject player;
	BuffyLeechBlast buffyLeechBlast;
	SpriteRenderer spriteRenderer;
	Animator shlorpNGlorpAnimator;
	IceBossStats iceBossStats;

	static readonly float airtimeDuration = 0.7f;

	float movementSpeed = 30f;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.FindWithTag("Player");
		buffyLeechBlast = player.GetComponent<BuffyLeechBlast>();
		iceBossStats = GameObject.FindWithTag("Ice Boss").GetComponent<IceBossStats>();
		
        if (gameObject.transform.rotation.z != 0)
		{
			movementSpeed = movementSpeed * -1;
			spriteRenderer.flipY = !spriteRenderer.flipY;
		}
		
		Invoke("KILLYOURSELF", airtimeDuration);
    }

    void FixedUpdate()
    {
        rb.position += new Vector2(movementSpeed * Time.deltaTime,0);
    }
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
		{
			CancelInvoke("KILLYOURSELF");
			KILLYOURSELF();
		}
		if ((collision.gameObject.tag == "Glorp") || (collision.gameObject.tag == "Shlorp"))
		{
			CancelInvoke("KILLYOURSELF");
			buffyLeechBlast.Invoke("AnimateW", 0f);
			shlorpNGlorpAnimator = collision.GetComponent<Animator>();
			shlorpNGlorpAnimator.SetBool("isDying", true);
			Destroy(gameObject);
		}
		else if ((collision.gameObject.tag == "Ice Boss Head") || (collision.gameObject.tag == "Ice Boss Jaw"))
		{
			iceBossStats.IceBossLoseHealthBy(1);
			CancelInvoke("KILLYOURSELF");
			buffyLeechBlast.Invoke("AnimateW", 0f);
			Destroy(gameObject);
		}
	}
	

	void KILLYOURSELF()
	{
		buffyLeechBlast.Invoke("AnimateL", 0f);
		Destroy(gameObject);
	}
}
