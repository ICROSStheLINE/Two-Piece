using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpSoulLaserBeamSonicBoomScript : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	
    readonly float animationDuration = 0.5f;
	
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		
        Invoke("KILLYOURSELF", animationDuration);
		Invoke("MakeTranslucent", 0.3f);
    }

	void MakeTranslucent()
	{
		spriteRenderer.color = new Color(0f,0f,0f,0.3f);
	}

	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
