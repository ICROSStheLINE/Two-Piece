using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPurposeDeathTimer : MonoBehaviour
{
    [SerializeField] float deathTimer = 0;
	
    void Start()
    {
        Destroy(gameObject, deathTimer);
    }

    
}
