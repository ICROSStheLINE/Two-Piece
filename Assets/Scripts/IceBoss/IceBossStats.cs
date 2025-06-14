using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossStats : MonoBehaviour
{
	IceBossBehaviour iceBossBehaviour;
	IceBossJaw iceBossJaw;
	GameObject bossHead;
	GameObject bossJaw;
	SpriteRenderer headSpriteRenderer;
	SpriteRenderer jawSpriteRenderer;
	[SerializeField] GameObject damageStatic;

	// Basic Stats
	public float iceBossTimeBetweenPatterns = 1.6f;
	public float iceBossOrbChargeTime = 1f;
	float currentHealth = 12f;

	// Idle Variables
	public bool iceBossIsAwake = false;
	[HideInInspector] public bool iceBossIdling = true;

	// Basic Attack Variables
	[HideInInspector] public bool iceBossMidAttack = false;

	// Energy Orb Variables
	public bool iceBossAttemptOrb = false;
	[HideInInspector] public bool iceBossMidOrb = false;
	
	// Ground Slam Variables
	[HideInInspector] public bool iceBossMidSlam = false;
	public bool iceBossSlamDown = false;
	public bool iceBossSlamUp = false;

	// Pattern Variables
	[HideInInspector] public bool iceBossPerformingPattern = false;
	[HideInInspector] public int iceBossSpecialPattern = 0;
	[HideInInspector] public float iceBossSpecialPatternStage = 0;
	
	bool oldPerformingPattern;
	int oldSpecialPattern = 69420;



    void Start()
    {
        iceBossBehaviour = GetComponentInChildren<IceBossBehaviour>();
		iceBossJaw = GetComponentInChildren<IceBossJaw>();
		bossHead = transform.GetChild(0).gameObject;
		bossJaw = transform.GetChild(1).gameObject;
		headSpriteRenderer = bossHead.GetComponent<SpriteRenderer>();
		jawSpriteRenderer = bossJaw.GetComponent<SpriteRenderer>();
    }

	// Used in "EnterBossArena.cs" script
	public void WakeUpIceBoss()
	{
		iceBossIsAwake = true;
		// InvokeRepeating("ActivateAttack", 6f, 1.8f);
		// Activate cutscene
		// Activate aggro
	}


	void Update()
	{
		if (iceBossIsAwake)
		{
			if (!iceBossPerformingPattern && iceBossSpecialPattern == 0 && (oldPerformingPattern != iceBossPerformingPattern || oldSpecialPattern != iceBossSpecialPattern))
			{
				Invoke("PerformRandomPattern", iceBossTimeBetweenPatterns);
			}
			
			oldPerformingPattern = iceBossPerformingPattern;
			oldSpecialPattern = iceBossSpecialPattern;
		}
	}
	
	void PerformRandomPattern()
	{
		int randomNumber = Random.Range(1,7);
		
		if (randomNumber == 1)
			iceBossBehaviour.Invoke("PatternOne", 0f);
		else if (randomNumber == 2)
			iceBossBehaviour.Invoke("PatternTwo", 0f);
		else if (randomNumber == 3)
			iceBossBehaviour.Invoke("PatternThree", 0f);
		else if (randomNumber == 4)
			iceBossBehaviour.Invoke("SpecialPatternOne", 0f);
		else if (randomNumber == 5)
			iceBossBehaviour.Invoke("SpecialPatternTwo", 0f);
		else if (randomNumber == 6)
			iceBossBehaviour.Invoke("SpecialPatternThree", 0f);
	}
	
	public void IceBossLoseHealthBy(int amount)
	{
		currentHealth -= amount;
		if (currentHealth > 0)
		{
			// Make Ceiling Icicle fall or make the boss appear more damaged or some shit idk
		}
		else if (currentHealth <= 0)
		{
			// Die
		}

		SwitchColorsRetardedly();
	}
	
	void SwitchColorsRetardedly()
	{
		GameObject damageStatic_ = Instantiate(damageStatic, bossHead.transform.position, bossHead.transform.rotation);
		damageStatic_.transform.parent = bossHead.transform;
		Invoke("TurnBlack", 0.1f);
		Invoke("TurnGray", 0.2f);
		Invoke("TurnBlack", 0.3f);
		Invoke("TurnGray", 0.4f);
		Invoke("TurnBlack", 0.6f);
		Invoke("TurnGray", 0.7f);
		Invoke("TurnBlack", 0.8f);
		Invoke("TurnGray", 0.9f);
		Invoke("TurnNormal", 1f);
	}

	void TurnBlack()
	{
		headSpriteRenderer.color = new Color(0f,0f,0f,1f);
		jawSpriteRenderer.color = new Color(0f,0f,0f,1f);
	}

	void TurnGray()
	{
		headSpriteRenderer.color = new Color(0.3f,0.3f,0.3f,1f);
		jawSpriteRenderer.color = new Color(0.3f,0.3f,0.3f,1f);
	}

	void TurnNormal()
	{
		headSpriteRenderer.color = new Color(1f,1f,1f,1f);
		jawSpriteRenderer.color = new Color(1f,1f,1f,1f);
	}

	public GameObject getIceBossJaw {
		get { return bossJaw; }
	}
}
