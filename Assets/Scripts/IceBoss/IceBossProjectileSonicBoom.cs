using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossProjectileSonicBoom : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
	static readonly float animationDurationSpeedMultiplier = 1f;
	static readonly float animationDuration = 0.667f / animationDurationSpeedMultiplier;
	Vector3 destination;
	
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		destination = transform.position + (transform.right * 8);
		
        Invoke("KILLYOURSELF", animationDuration);
		//Invoke("MakeTranslucent", (animationDuration*2)/4);
		Invoke("MakeTranslucenter", (animationDuration*3)/4);
    }
	
	void FixedUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, destination, 20 * Time.deltaTime);
	}

	void MakeTranslucent()
	{
		spriteRenderer.color = new Color(0f,0f,0f,0.7f);
	}
	
	void MakeTranslucenter()
	{
		spriteRenderer.color = new Color(0f,0f,0f,0.20f);
	}

	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
