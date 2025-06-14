using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossArena : MonoBehaviour
{
    GameObject cam;
	PlayerTracker cameraPlayerTracker;
	IceBossStats iceBossStats;
	IceArenaScript iceArenaScript;
	
	bool alreadyInIceArena = false;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
		cameraPlayerTracker = cam.GetComponent<PlayerTracker>();
		iceArenaScript = GameObject.FindWithTag("Ice Arena Fighting Zone").gameObject.transform.parent.GetComponent<IceArenaScript>();
		iceBossStats = GameObject.FindWithTag("Ice Boss").GetComponent<IceBossStats>();
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ice Arena Fighting Zone")
		{
			cameraPlayerTracker.Invoke("EnterIceArena", 0f);
			cameraPlayerTracker.CancelInvoke("TrackPlayerAgain");
			if (!alreadyInIceArena)
			{
				iceArenaScript.CloseIceArenaEntranceGate();
				cameraPlayerTracker.InvokeRepeating("ActivateCutsceneMode", 0f, 0.3f);
				cameraPlayerTracker.Invoke("DeactivateCutsceneMode", 5f);
				alreadyInIceArena = true;
				iceBossStats.WakeUpIceBoss();
			}
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ice Arena Fighting Zone")
		{
			cameraPlayerTracker.Invoke("TrackPlayerAgain", 1f);
		}
	}
}
