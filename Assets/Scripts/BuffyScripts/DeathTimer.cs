using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("KILLYOURSELF", 0.7f);
    }

	void Update()
	{
		if (gameObject.transform.localScale.x < 0)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
	}

    void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
