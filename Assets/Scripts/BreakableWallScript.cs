using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
	

    void Start()
    {
        
    }


    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Truth Seeking Orb")
		{
			if (GameObject.FindWithTag("Player").GetComponent<PlayerStats>().playerMidTSOAttack == true)
			{
				foreach (Transform child in transform.parent)
				{
					child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
				}
			}
		}
	}
}
