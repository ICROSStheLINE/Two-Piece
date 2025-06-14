using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossProjectileTrail : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	
	[HideInInspector] public GameObject iceBossProjectileTrailParent;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
		SetInvisible();
		
		if (gameObject.transform.rotation.z != 0)
		{
			spriteRenderer.flipY = !spriteRenderer.flipY;
			BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y * -1);
		}
    }
	
	void Update()
	{
		transform.position = iceBossProjectileTrailParent.transform.position;
	}

    void SetInvisible()
	{
		spriteRenderer.color = new Color(0f,0f,0f,0f);
	}

	void SetVisible()
	{
		spriteRenderer.color = new Color(1f,1f,1f,1f);
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
		{
			Invoke("SetVisible", 0.2f);
		}
	}
}
