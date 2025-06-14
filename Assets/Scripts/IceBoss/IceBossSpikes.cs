using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossSpikes : MonoBehaviour
{
	GameObject healthBar;
	HealthScript healthScript;
	float risingSpeed = 4f;

    void Start()
    {
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
        Invoke("StopRising", 0.2f);
		if (transform.rotation.z == 0)
			risingSpeed *= -1;
		Invoke("KILLYOURSELF", 1f);
    }

    void Update()
    {
        transform.position += new Vector3(0,risingSpeed * Time.deltaTime,0);
    }
	
	void StopRising()
	{
		risingSpeed = 0;
	}
	
	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			healthScript.LoseHealthBy(1);
			CancelInvoke();
			KILLYOURSELF();
		}
	}
}
