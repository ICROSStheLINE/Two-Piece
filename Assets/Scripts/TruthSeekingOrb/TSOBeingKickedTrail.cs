using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSOBeingKickedTrail : MonoBehaviour
{
    static readonly float animationDuration = 0.583f;
	
    void Start()
    {
        Invoke("KILLYOURSELF", animationDuration);
    }

    
    void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
