using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportIndicatorScript : MonoBehaviour
{
    GameObject player;
	PlayerStats playerStats;
	SpriteRenderer playerSpriteRenderer;
	SpriteRenderer spriteRenderer;
	
	Color purple = new Color(0.688f,0f,1f,1f);
	Color red = new Color(1f,0f,0f,1f);
	Color fullyTransparent = new Color(0,0,0,0);
	
	bool insideWall = false;
	
    void Start()
    {
        player = GameObject.FindWithTag("Player");
		playerStats = player.GetComponent<PlayerStats>();
		playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		//spriteRenderer.color = purple;
    }

    
    void FixedUpdate()
    {
		if (Mathf.Sign(player.transform.localScale.x) == -1)
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
		else
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		
		if (!insideWall)
			spriteRenderer.color = purple;
    }



	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
		{
			float objectUpCoord = collision.transform.position.y + collision.transform.localScale.y/2;
			float objectDownCoord = collision.transform.position.y - collision.transform.localScale.y/2;
			float objectRightCoord = collision.transform.position.x + collision.transform.localScale.x/2;
			float objectLeftCoord = collision.transform.position.x - collision.transform.localScale.x/2;

			
			insideWall = true;
			spriteRenderer.color = red;
			
			if (Input.GetKey(playerStats.aimDownKey))
			{
				if (objectUpCoord <= player.transform.position.y)
				{
					transform.position = new Vector3(transform.position.x, objectUpCoord + 1.3f, transform.position.z);
				}
				//else
					//spriteRenderer.color = red;
			}
			else if (Input.GetKey(playerStats.aimUpKey))
			{
				if (objectDownCoord >= player.transform.position.y)
				{
					transform.position = new Vector3(transform.position.x, objectDownCoord - 1.3f, transform.position.z);
				}
				//else
					//spriteRenderer.color = red;
			}
			//else
				//spriteRenderer.color = red;
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Floor or Wall")
		{
			insideWall = false;
		}
	}
	
	
	
			/* Old.
			float objectUpCoord = collision.transform.position.y + collision.transform.localScale.y/2;
			float objectDownCoord = collision.transform.position.y - collision.transform.localScale.y/2;
			float objectRightCoord = collision.transform.position.x + collision.transform.localScale.x/2;
			float objectLeftCoord = collision.transform.position.x - collision.transform.localScale.x/2;
			
			Vector3 potentialDestinationUp = new Vector3(transform.position.x, objectUpCoord, transform.position.z);
			Vector3 potentialDestinationDown = new Vector3(transform.position.x, objectDownCoord, transform.position.z);
			Vector3 potentialDestinationRight = new Vector3(objectRightCoord, transform.position.y, transform.position.z);
			Vector3 potentialDestinationLeft = new Vector3(objectLeftCoord, transform.position.y, transform.position.z);
			
			//Debug.Log(collision.gameObject.name + " is colliding with the TP indicator!");
			
			if (Input.GetKey("s"))
			{
				if (objectUpCoord <= player.transform.position.y)
				{
					transform.position = new Vector3(transform.position.x, objectUpCoord, transform.position.z);
					spriteRenderer.color = purple;
				}
			}
			else if (Input.GetKey("w"))
			{
				if (objectDownCoord >= player.transform.position.y)
				{
					transform.position = new Vector3(transform.position.x, objectDownCoord, transform.position.z);
					spriteRenderer.color = purple;
				}
			}*/
}
