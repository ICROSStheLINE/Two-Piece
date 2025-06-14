using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOBeingKickedSonicBoom : MonoBehaviour
{
	static readonly float animationDurationMultiplier = 1;
    static readonly float animationDuration = 0.583f / animationDurationMultiplier;
	
    void Start()
    {
        Invoke("KILLYOURSELF", animationDuration);
    }

    void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
