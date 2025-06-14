using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
	GameObject player;
	[SerializeField] bool isTrackingPlayer = true;
	PlayerKickingTSO playerKickingTSO;
	PlayerStats playerStats;
	Camera cam;
	
	float velocity = 0; // This variable exists for a stupid reason LOL
	float targetZoom = 11.5f;
	Vector3 targetPosition;
	[SerializeField] float speed = 7f;
	
	//[HideInInspector] public bool midCutscene = false;

	void Start()
	{
		cam = GetComponent<Camera>();
		cam.orthographicSize = targetZoom;
		player = GameObject.FindWithTag("Player");
		playerKickingTSO = player.GetComponent<PlayerKickingTSO>();
		playerStats = player.GetComponent<PlayerStats>();
	}

    void Update()
    {
		if (isTrackingPlayer)
		{
			transform.position = new Vector3(player.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
			if (!playerStats.playerMidKickingTSOButForTheCameraGameObject)
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(gameObject.transform.position.x,player.transform.position.y + Mathf.Sign(player.transform.localScale.y)*2,gameObject.transform.position.z), 70 * Time.deltaTime);
		}
		else
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		
		if (cam.orthographicSize != targetZoom)
		{
			cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref velocity, 1.2f);
		}
		
		//Debug.Log(cam.velocity);
    }
	// Used in "EnterBossArena.cs" script
	void ActivateCutsceneMode()
	{
		/*playerStats.playerCanDash = false;
		playerStats.ResetPlayerDashCooldown();
		playerStats.playerCanMove = false;
		playerStats.midCutscene = true;*/
	}
	// Used in "EnterBossArena.cs" script
	void DeactivateCutsceneMode()
	{
		CancelInvoke("ActivateCutsceneMode");
		playerStats.playerCanDash = true;
		playerStats.playerCanMove = true;
		playerStats.midCutscene = false;
	}
	// Used in "EnterBossArena.cs" script
	void EnterIceArena()
	{
		isTrackingPlayer = false;
		targetZoom = 15.8f;
		Vector3 collidedObjectCoords = GameObject.FindWithTag("Ice Arena Fighting Zone").transform.position;
		targetPosition = new Vector3(collidedObjectCoords.x, collidedObjectCoords.y, gameObject.transform.position.z);
	}
	// Used in "EnterBossArena.cs" script
	void TrackPlayerAgain()
	{
		isTrackingPlayer = true;
		targetZoom = 11.5f;
	}
}
