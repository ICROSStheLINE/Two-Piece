using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTSOBasicAttack : MonoBehaviour
{
    Animator anim;
	PlayerStats playerStats;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.playerMidTSOAttack)
        {
            
        }
    }
}
