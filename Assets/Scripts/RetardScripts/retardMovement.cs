using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retardMovement : MonoBehaviour
{
	Rigidbody2D rb;
	[SerializeField] float retardSpeed = 5f;
	
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }
	
	void UpdateMovement()
	{
		float deltaTimeRetardSpeed = retardSpeed * Time.deltaTime;
		rb.position -= new Vector2(deltaTimeRetardSpeed, 0);
	}
}
