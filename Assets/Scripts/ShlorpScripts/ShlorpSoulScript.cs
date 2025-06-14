using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlorpSoulScript : MonoBehaviour
{
    GameObject player;
	[SerializeField] GameObject laserBeam;
	[SerializeField] GameObject laserSonicBoom;
	Animator anim;
	
	readonly float shlorpSoulLaserAnimationSpeed = 1.417f / 2;
	
	float speed = 0.04f;
	Vector3 flightPath;
	bool isMoving = true;

    void Start()
    {
		anim = GetComponent<Animator>();
		
        player = GameObject.FindWithTag("Player");
		flightPath = gameObject.transform.position + new Vector3(0,5,0);
		Invoke("ChangeFlightPath1", 0.2f);
		Invoke("ChangeFlightPath2", 0.4f);
		Invoke("ChangeFlightPath3", 0.6f);
		Invoke("ChangeFlightPath4", 0.8f);
		Invoke("ChangeFlightPath5", 1.0f);
    }

    void Update()
    {
		if (isMoving)
		{
			FacePlayer();
		}
		MoveAround();
    }

	void MoveAround()
	{
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, flightPath, speed);
	}


	void ChangeFlightPath1()
	{
		flightPath = gameObject.transform.position + new Vector3(Mathf.Sign(player.transform.localScale.x) * 3,0,0);
	}

	void ChangeFlightPath2()
	{
		flightPath = gameObject.transform.position + new Vector3(Mathf.Sign(player.transform.localScale.x) * 3,-2,0);
	}

	void ChangeFlightPath3()
	{
		flightPath = gameObject.transform.position + new Vector3(Mathf.Sign(player.transform.localScale.x) * 3,-1,0);
	}

	void ChangeFlightPath4()
	{
		flightPath = gameObject.transform.position + new Vector3(Mathf.Sign(player.transform.localScale.x) * 3,2,0);
	}

	void ChangeFlightPath5()
	{
		flightPath = gameObject.transform.position + new Vector3(Mathf.Sign(player.transform.localScale.x) * 4,Random.Range(-3,2),0);
		Invoke("FireLaser", 0.4f);
	}

	void FireLaser()
	{
		isMoving = false;
		anim.SetBool("fireShlorpSoulLaser", true);
		Invoke("KILLYOURSELF", shlorpSoulLaserAnimationSpeed);
		Invoke("ProduceLaser", shlorpSoulLaserAnimationSpeed / 1.75f);
	}
	
	void ProduceLaser()
	{
		int amountOfLaserObjects = 25;
		GameObject[] laserBeamInstance = new GameObject[amountOfLaserObjects];
		for (int i = 0; i < amountOfLaserObjects; i++)
		{
			laserBeamInstance[i] = Instantiate(laserBeam, transform.position + transform.right * ((i + 1) * 2), transform.rotation);
		}
		for (int i = 0; i < amountOfLaserObjects; i++)
		{
			GameObject laserSonicBoomEffect = Instantiate(laserSonicBoom, laserBeamInstance[i].gameObject.transform.position, laserBeamInstance[i].gameObject.transform.rotation);
			laserSonicBoomEffect.transform.parent = laserBeamInstance[i].gameObject.transform;
		}
	}

	void FacePlayer()
	{
		Vector2 directionTowardsPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
		
		transform.right = directionTowardsPlayer;
	}
	
	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
