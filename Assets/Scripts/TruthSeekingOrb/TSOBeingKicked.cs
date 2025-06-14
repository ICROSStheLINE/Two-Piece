using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOBeingKicked : MonoBehaviour
{
    Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	Animator shlorpNGlorpAnimator;
	[SerializeField] GameObject trailPrefab;
	[SerializeField] GameObject sonicBoom;
	PlayerKickingTSO playerKickingTSO;
	GameObject iceBoss;
	IceBossStats iceBossStats;
	
	static readonly float ballAirtimeDuration = 0.4f;

	float movementSpeed = 50f;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerKickingTSO = GameObject.FindWithTag("Player").GetComponent<PlayerKickingTSO>();
		iceBoss = GameObject.FindWithTag("Ice Boss");
		iceBossStats = iceBoss.GetComponent<IceBossStats>();
		
		
        if (gameObject.transform.rotation.z != 0)
		{
			movementSpeed = movementSpeed * -1;
			spriteRenderer.flipY = !spriteRenderer.flipY;
		}
		
		InvokeRepeating("SpawnTrail", 0.1f, 0.04f);
		Invoke("KILLYOURSELF", ballAirtimeDuration);
    }

    void FixedUpdate()
    {
		rb.position += new Vector2(movementSpeed * Time.deltaTime,0);
    }
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
		{
			playerKickingTSO.CancelInvoke("SpawnTSOPrefab");
			playerKickingTSO.SpawnTSOPrefab();
			playerKickingTSO.Invoke("ResetAbilityToTPCooldown", 0f);
			Destroy(gameObject);
		}
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

	void SpawnTrail()
	{
		Instantiate(trailPrefab, transform.position, transform.rotation);
		Instantiate(sonicBoom, transform.position, transform.rotation);
	}

	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
