using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossBehaviour : MonoBehaviour
{
	Rigidbody2D rb;
	GameObject healthBar;
	HealthScript healthScript;
	IceBossStats iceBossStats;
	GameObject player;
	GameObject fightingZone;
	PlayerStats playerStats;
	GameObject bossSclera;
	GameObject bossEye;

	Vector3 nextPosition;

	string[] queuedAttack = new string[6];

	// Idling Variables
	Vector3 idleTargetPosition;
	Vector3[] idleTargetPositionPoints = new Vector3[2];
	int idlePositionIndex = 0;

	// Attacking Variables
	Vector3 attackingTarget;
	[SerializeField] float speed = 150;
	[SerializeField] GameObject chompingPrefab;

	// Laser Variables
	[SerializeField] GameObject laserPrefab;
	float laserYPosition = 8.5f;
	
	// Ground Slam Variables
	Vector3 slamTarget;
	Vector3 postSlamTarget;
	[SerializeField] GameObject slamSpike;
	[SerializeField] GameObject glintOfLight;
	[SerializeField] GameObject icePlatform;
	GameObject icePlatform_;
	Vector3 platformSpawn;
	float spikePosition = 0;
	bool spikeGoesBackwards = false;
	[SerializeField] float slamSpeed = 60;

	// Energy Orb Variables
	[SerializeField] GameObject energyOrbPrefab;
	GameObject chargedOrb;
	
	// Pattern Variables
	Vector3 fleeingDestination;


    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
        iceBossStats = transform.parent.GetComponent<IceBossStats>();
		player = GameObject.FindWithTag("Player");
		fightingZone = GameObject.FindWithTag("Ice Arena Fighting Zone");
		laserYPosition = fightingZone.transform.position.y;
		Vector3 arenaStartPos = fightingZone.transform.position - new Vector3(fightingZone.transform.localScale.x/2,0,0);
		fleeingDestination = new Vector3(arenaStartPos.x + fightingZone.transform.localScale.x/3,fightingZone.transform.position.y + 2,fightingZone.transform.position.z);
		playerStats = player.GetComponent<PlayerStats>();
		bossSclera = transform.GetChild(0).gameObject;
		bossEye = bossSclera.transform.GetChild(0).gameObject;
		
		
		SetNewIdlePositionPoints(transform.position, new Vector3(0,2,0), new Vector3(0,0,0));
    }

    void FixedUpdate()
    {
		if (iceBossStats.iceBossSpecialPattern == 0) // Special patterns are cool attacks that aren't typical
		{
			// Checking Attack Variables
			if (iceBossStats.iceBossMidAttack && !iceBossStats.iceBossAttemptOrb)
				Invoke("AttackMiddle", 0.4f);
			else if (iceBossStats.iceBossMidSlam)
				Invoke("GroundSlamMiddle", 0.6f);
			else if (iceBossStats.iceBossAttemptOrb)
			{
				ChargeEnergyOrb();
				AttackFinish();
			}
			else if (iceBossStats.iceBossMidOrb)
				nextPosition = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y + 5, transform.position.z), 15 * Time.deltaTime);
			else if (iceBossStats.iceBossIdling && !iceBossStats.iceBossMidOrb && !iceBossStats.iceBossMidAttack && !iceBossStats.iceBossAttemptOrb)
				Idling();
		}
		else if (iceBossStats.iceBossSpecialPattern == 1)
			SpecialPatternOne();
		else if (iceBossStats.iceBossSpecialPattern == 2)
			SpecialPatternTwo();
		else if (iceBossStats.iceBossSpecialPattern == 3)
			SpecialPatternThree();
		
		// Movement
		if (iceBossStats.iceBossIsAwake)
			transform.position = nextPosition;
    }

	void Idling()
	{
		nextPosition = Vector3.MoveTowards(transform.position, idleTargetPosition, 6 * Time.deltaTime);
		
		if (transform.position == idleTargetPosition)
		{
			idlePositionIndex = Mathf.Abs(idlePositionIndex - 1);
			idleTargetPosition = idleTargetPositionPoints[idlePositionIndex]; // Switch the idle bobbing destination
		}
	}

	void AttackStart(Vector3 customTarget = default(Vector3))
	{
		iceBossStats.iceBossIdling = false;
		if (customTarget == default(Vector3))
		{
			attackingTarget = player.transform.position + new Vector3(0,3,0);
			Instantiate(chompingPrefab, transform.position + Vector3.Normalize(player.transform.position - transform.position)*7 - new Vector3(0,1.7f,0), Quaternion.Euler(0,0,0));
		}
		else
			attackingTarget = customTarget;
		
		iceBossStats.iceBossMidAttack = true;
		
		IceBossEyeStare(customTarget);
	}

	void AttackMiddle()
	{
		nextPosition = Vector3.MoveTowards(transform.position, attackingTarget, speed * Time.fixedDeltaTime);
		if (transform.position == attackingTarget)
		{
			AttackFinish();
			
			CheckAttackQueue();
		}
	}

	void AttackFinish()
	{
		iceBossStats.iceBossMidAttack = false;
		CancelInvoke("AttackMiddle");
		
		SetNewIdlePositionPoints(transform.position, new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3,0), new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3 - 2,0));

		iceBossStats.iceBossIdling = true;
		bossEye.transform.position = bossSclera.transform.position;
		
		if (iceBossStats.iceBossSpecialPattern != 0)
			iceBossStats.iceBossSpecialPatternStage += 0.5f;
	}

	void Laser(Vector3 customTarget = default(Vector3))
	{
		if (customTarget == default(Vector3))
		{
			Instantiate(laserPrefab, new Vector3(player.transform.position.x,laserYPosition,0), Quaternion.Euler(0,0,0));
		}
		else
		{
			Instantiate(laserPrefab, customTarget, Quaternion.Euler(0,0,0));
		}
		
		CheckAttackQueue();
	}

	void ChargeEnergyOrb(float customChargeTime = default(float))
	{
		float oldChargeTime = iceBossStats.iceBossOrbChargeTime;
		int angleOfOrb = 0;
		if (Mathf.Sign(player.transform.position.x - transform.position.x) == -1)
			angleOfOrb = 180;
		if (customChargeTime != default(float))
			iceBossStats.iceBossOrbChargeTime = customChargeTime;
		chargedOrb = Instantiate(energyOrbPrefab, transform.position - new Vector3(0,5,0), Quaternion.Euler(0,0,angleOfOrb));
		Invoke("FireEnergyOrb", iceBossStats.iceBossOrbChargeTime);
		
		iceBossStats.iceBossOrbChargeTime = oldChargeTime;
		
		iceBossStats.iceBossMidOrb = true;
		iceBossStats.iceBossAttemptOrb = false;
		
		if (iceBossStats.iceBossSpecialPattern != 0)
			iceBossStats.iceBossSpecialPatternStage += 0.5f;
	}

	void FireEnergyOrb()
	{
		// Orb now fires itself (Code is in the "IceBossProjectile" script)
		iceBossStats.iceBossMidOrb = false;
		
		CheckAttackQueue();
	}

	void GroundSlamStart(string customDirection = default(string))
	{
		iceBossStats.iceBossIdling = false;
		iceBossStats.iceBossMidSlam = true;

		if (customDirection == default(string) || customDirection == "Downwards")
		{
			Vector3 arenaBottom = fightingZone.transform.position - new Vector3(0,fightingZone.transform.localScale.y/2,0);
			
			GameObject lightGlint = Instantiate(glintOfLight, iceBossStats.getIceBossJaw.transform.position - new Vector3(-0.2f,2.7f,0), Quaternion.Euler(0,0,0));
			lightGlint.transform.parent = iceBossStats.getIceBossJaw.transform;
			
			slamTarget = arenaBottom;
		}
		else if (customDirection == "Upwards")
		{
			Vector3 arenaTop = fightingZone.transform.position + new Vector3(0,fightingZone.transform.localScale.y/2,0);
			
			GameObject lightGlint = Instantiate(glintOfLight, transform.position + new Vector3(1.3f,3.6f,0), Quaternion.Euler(0,0,0));
			lightGlint.transform.localScale -= new Vector3(0.3f,0.3f,0.3f);
			lightGlint.transform.parent = transform;
			lightGlint = Instantiate(glintOfLight, transform.position + new Vector3(-1.3f,3.6f,0), Quaternion.Euler(0,0,0));
			lightGlint.transform.localScale -= new Vector3(0.3f,0.3f,0.3f);
			lightGlint.transform.parent = transform;
			
			slamTarget = arenaTop;
		}

		IceBossEyeStare(slamTarget);
	}

	void GroundSlamMiddle()
	{
		nextPosition = Vector3.MoveTowards(transform.position, slamTarget, slamSpeed * Time.fixedDeltaTime);
		if (transform.position == slamTarget)
		{
			GroundSlamFinish();
			
			CheckAttackQueue();
		}
	}

	void GroundSlamFinish()
	{
		CancelInvoke("GroundSlamMiddle");
		iceBossStats.iceBossIdling = true;
		iceBossStats.iceBossMidSlam = false;
		postSlamTarget = slamTarget;
		
		for (float i = 0; i < 0.5f; i += 0.011f)
		{
			Invoke("SpawnIceSpike", i);
		}
		Invoke("ResetSpikePositionVar", 0.51f);
		bossEye.transform.position = bossSclera.transform.position;
		
		if (iceBossStats.iceBossSpecialPattern != 0)
			iceBossStats.iceBossSpecialPatternStage += 0.5f;
	}

	void SpawnIceSpike()
	{
		if (spikeGoesBackwards)
			spikePosition *= -1;
	
		Vector3 arenaBottom = fightingZone.transform.position - new Vector3(0,fightingZone.transform.localScale.y/2,0);
		if (postSlamTarget == arenaBottom)
			Instantiate(slamSpike, postSlamTarget + new Vector3(spikePosition,0f,0), Quaternion.Euler(0,0,180));
		else
			Instantiate(slamSpike, postSlamTarget - new Vector3(spikePosition,0f,0), Quaternion.Euler(0,0,0));
		
		spikePosition = Mathf.Abs(spikePosition) + 0.6f;
		spikeGoesBackwards = !spikeGoesBackwards;
	}

	void ResetSpikePositionVar()
	{
		spikePosition = 0;
	}

	void CheckAttackQueue()
	{
		// Corrects the order of the queued attacks (Moves the order down by 1)
		for (int i = 0; i < queuedAttack.Length - 1; i += 1)
			queuedAttack[i] = queuedAttack[i + 1];
		queuedAttack[5] = null;
		
		// Checks if any attacks were queued up and activates the next queued attack
		if (queuedAttack[0] == "Attack")
			ActivateAttack();
		else if (queuedAttack[0] == "Orb")
			ActivateOrb();
		else if (queuedAttack[0] == "Laser")
			ActivateLaser();
		else if (queuedAttack[0] == "CeilingSlam")
			GroundSlamStart("Upwards");
		else if (queuedAttack[0] == "GroundSlam")
			GroundSlamStart("Downwards");
		else if (queuedAttack[0] == "ResetPatternVar")
			ResetPatternVar();
	}

	public void IceBossEyeStare(Vector3 customTarget = default(Vector3))
	{
		if (customTarget == default(Vector3)) // If no parameters were given, stare at the player
			bossEye.transform.position += new Vector3(Vector3.Normalize(player.transform.position - bossEye.transform.position).x, Vector3.Normalize(player.transform.position - bossEye.transform.position).y/2, 0);
		else // Otherwise, stare at the custom target (Obviously)
			bossEye.transform.position += new Vector3(Vector3.Normalize(customTarget - bossEye.transform.position).x/2, Vector3.Normalize(customTarget - bossEye.transform.position).y/2, 0);
	}

	void SetNewIdlePositionPoints(Vector3 position_, Vector3 offset1, Vector3 offset2)
	{
		idleTargetPositionPoints[0] = position_ + offset1;
		idleTargetPositionPoints[1] = position_ + offset2;
		idleTargetPosition = idleTargetPositionPoints[0];
	}

	Vector3 BossPositionInArena()
	{
		Vector3 bossPos = gameObject.transform.position;
		Vector3 arenaStartPos = fightingZone.transform.position - new Vector3(fightingZone.transform.localScale.x/2,0,0);
		return (bossPos - arenaStartPos); // If returns (0,y,z) then the boss is at the leftmost side of the arena
	}

	Vector3 PlayerPositionInArena()
	{
		Vector3 playerPos = player.transform.position;
		Vector3 arenaStartPos = fightingZone.transform.position - new Vector3(fightingZone.transform.localScale.x/2,0,0);
		return (playerPos - arenaStartPos); // If returns (0,y,z) then the player is at the leftmost side of the arena
	}
	
	Vector3 PlayerSideOfArena()
	{
		Vector3 playerPos = player.transform.position;
		Vector3 arenaMiddlePos = fightingZone.transform.position;
		return (playerPos - arenaMiddlePos); // If returns (-x,y,z) then the player is on the left side of the arena
	}

	// Attack Patterns

	void PatternOne() // Just charging at the player
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossPerformingPattern = true;
			
			ActivateAttack();
			queuedAttack[1] = "Attack";
			queuedAttack[2] = "Attack";
			queuedAttack[3] = "ResetPatternVar";
		}
	}

	void PatternTwo() // Charging twice then energy orb
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossPerformingPattern = true;
			
			ActivateAttack();
			queuedAttack[1] = "Attack";
			queuedAttack[2] = "Orb";
			queuedAttack[3] = "ResetPatternVar";
		}
	}

	void PatternThree() // Energy Orb + Ground Slam Combo
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossPerformingPattern = true;
			
			ActivateOrb();
			queuedAttack[1] = "Orb";
			queuedAttack[2] = "CeilingSlam";
			queuedAttack[3] = "Attack";
			queuedAttack[4] = "Orb";
			queuedAttack[5] = "ResetPatternVar";
		}
	}


	void SpecialPatternOne() // Cancer
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossSpecialPattern = 1; // Set this variable to 1 so the Update() function only does this shit
			
			if (iceBossStats.iceBossSpecialPatternStage == 0)
			{	
				AttackStart(player.transform.position + new Vector3(0,Mathf.Sign(PlayerPositionInArena().y * -1)*11,0));
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 0.5f)
				Invoke("AttackMiddle", 0.05f);
			else if (iceBossStats.iceBossSpecialPatternStage == 1)
			{
				AttackStart(player.transform.position + new Vector3(0,Mathf.Sign(PlayerPositionInArena().y * -1)*11,0));
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 1.5f)
				Invoke("AttackMiddle", 0.05f);
			else if (iceBossStats.iceBossSpecialPatternStage == 2)
			{
				AttackStart(player.transform.position + new Vector3(0,Mathf.Sign(PlayerPositionInArena().y * -1)*11,0));
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 2.5f)
				Invoke("AttackMiddle", 0.05f);
			else if (iceBossStats.iceBossSpecialPatternStage == 3)
			{
				AttackStart(player.transform.position + new Vector3(0,Mathf.Sign(PlayerPositionInArena().y * -1)*11,0));
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 3.5f)
				Invoke("AttackMiddle", 0.05f);
			else if (iceBossStats.iceBossSpecialPatternStage == 4)
			{	
				AttackStart();
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 4.5f)
				Invoke("AttackMiddle", 0.4f);
			else if (iceBossStats.iceBossSpecialPatternStage == 5) // Fly to the right side of arena
			{
				Vector3 endOfArena = fightingZone.transform.position + new Vector3(fightingZone.transform.localScale.x/2,0,0);
				nextPosition = Vector3.MoveTowards(transform.position, endOfArena + new Vector3(1.5f,0,0), 45 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage > 5 && iceBossStats.iceBossSpecialPatternStage < 8) // Fires a couple energy orbs
			{
				if (!iceBossStats.iceBossMidOrb)
					ChargeEnergyOrb(0.3f);
				else // This else statement makes the boss track the player a bit
					nextPosition = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y + 5, transform.position.z), 35 * Time.deltaTime);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 8)
			{
				Vector3 middleOfArena = fightingZone.transform.position;
				nextPosition = Vector3.MoveTowards(transform.position, middleOfArena + new Vector3(7,0,0), 45 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
				Laser(new Vector3(transform.position.x, laserYPosition, 0f));
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 8.5f)
			{
				Vector3 startOfArena = fightingZone.transform.position - new Vector3(fightingZone.transform.localScale.x/2,0,0);
				nextPosition = Vector3.MoveTowards(transform.position, startOfArena - new Vector3(1.5f,0,0), 45 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage > 8.5f && iceBossStats.iceBossSpecialPatternStage < 12)
			{
				if (!iceBossStats.iceBossMidOrb)
					ChargeEnergyOrb(0.3f);
				else // This else statement makes the boss track the player a bit
					nextPosition = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y + 5, transform.position.z), 35 * Time.deltaTime);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 12)
			{
				Vector3 middleOfArena = fightingZone.transform.position;
				nextPosition = Vector3.MoveTowards(transform.position, middleOfArena - new Vector3(7,0,0), 45 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
				Laser(new Vector3(transform.position.x, laserYPosition, 0f));
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 12.5)
			{
				SetNewIdlePositionPoints(transform.position, new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3,0), new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3 - 2,0));
				iceBossStats.iceBossSpecialPattern = 0;
				iceBossStats.iceBossSpecialPatternStage = 0;
			}
		}
	}

	void SpecialPatternTwo() // Lightning Barrage
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossSpecialPattern = 2; // Set this variable to 2 so the Update() function only does this shit
			Vector3 arenaStartPos = fightingZone.transform.position - new Vector3(fightingZone.transform.localScale.x/2,0,0);
			
			if (iceBossStats.iceBossSpecialPatternStage == 0)
			{
				InvokeRepeating("ActivateLaser", 0.5f, 0.5f);
				iceBossStats.iceBossSpecialPatternStage++;
				
				Invoke("IncreasePatternStage", 10f); // Sets up the pattern stage increase for the next attack
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 1)
			{
				if ((player.transform.position.x <= fleeingDestination.x + 5) && (player.transform.position.x >= fleeingDestination.x - 5))
				{
					if (fleeingDestination == new Vector3(arenaStartPos.x + fightingZone.transform.localScale.x/3,fightingZone.transform.position.y + 2,fightingZone.transform.position.z))
						fleeingDestination = new Vector3(arenaStartPos.x + fightingZone.transform.localScale.x*2/3,fightingZone.transform.position.y + 2,fightingZone.transform.position.z);
					else
						fleeingDestination = new Vector3(arenaStartPos.x + fightingZone.transform.localScale.x/3,fightingZone.transform.position.y + 2,fightingZone.transform.position.z);
				}
				nextPosition = Vector3.MoveTowards(transform.position, fleeingDestination, 100 * Time.deltaTime);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 2)
			{
				CancelInvoke("ActivateLaser");
				SetNewIdlePositionPoints(transform.position, new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3,0), new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3 - 2,0));
				iceBossStats.iceBossSpecialPattern = 0;
				iceBossStats.iceBossSpecialPatternStage = 0;
			}
		}
	}

	void SpecialPatternThree() // Ground Slam Barrage (Spawns platform)
	{
		if (!iceBossStats.iceBossPerformingPattern)
		{
			iceBossStats.iceBossSpecialPattern = 3; // Set this variable to 3 so the Update() function only does this shit
			
			if (iceBossStats.iceBossSpecialPatternStage == 0)
			{
				GroundSlamStart("Downwards");
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 0.5f)
			{
				Invoke("GroundSlamMiddle", 1.0f);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 1)
			{
				platformSpawn = fightingZone.transform.position + new Vector3(-13*Mathf.Sign(PlayerSideOfArena().x),-fightingZone.transform.localScale.y/2,0);
				icePlatform_ = Instantiate(icePlatform, platformSpawn, Quaternion.Euler(0,0,0));
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 1.5f)
			{
				icePlatform_.transform.position = Vector3.MoveTowards(icePlatform_.transform.position, platformSpawn + new Vector3(0,fightingZone.transform.localScale.y/2,0), 50 * Time.deltaTime);
				nextPosition = Vector3.MoveTowards(transform.position, fightingZone.transform.position, 20 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 2)
			{
				nextPosition = Vector3.MoveTowards(transform.position, fightingZone.transform.position + new Vector3(0.5f,0,0), 25 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 2.5f)
			{
				nextPosition = Vector3.MoveTowards(transform.position, fightingZone.transform.position - new Vector3(0.5f,0,0), 25 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 3)
			{
				nextPosition = Vector3.MoveTowards(transform.position, fightingZone.transform.position + new Vector3(0.5f,0,0), 25 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 3.5f)
			{
				nextPosition = Vector3.MoveTowards(transform.position, fightingZone.transform.position - new Vector3(0.5f,0,0), 25 * Time.deltaTime);
				if (transform.position == nextPosition)
					iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 4)
			{
				GroundSlamStart("Downwards");
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 4.5f)
			{
				Invoke("GroundSlamMiddle", 0.3f);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 5)
			{
				GroundSlamStart("Upwards");
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 5.5f)
			{
				Invoke("GroundSlamMiddle", 0.1f);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 6)
			{
				GroundSlamStart("Downwards");
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 6.5f)
			{
				Invoke("GroundSlamMiddle", 0.1f);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 7)
			{
				GroundSlamStart("Upwards");
				iceBossStats.iceBossSpecialPatternStage += 0.5f;
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 7.5f)
			{
				Invoke("GroundSlamMiddle", 0.1f);
			}
			else if (iceBossStats.iceBossSpecialPatternStage == 8)
			{
				Destroy(icePlatform_.gameObject, 1);
				SetNewIdlePositionPoints(transform.position, new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3,0), new Vector3(0,Mathf.Sign(BossPositionInArena().y * -1)*3 - 2,0));
				iceBossStats.iceBossSpecialPattern = 0;
				iceBossStats.iceBossSpecialPatternStage = 0;
			}
		}
	}

	void SpecialPatternFour()
	{
		
	}

	void IncreasePatternStage()
	{
		iceBossStats.iceBossSpecialPatternStage++;
	}

	void ActivateAttack()
	{
		if (!iceBossStats.iceBossMidAttack && !iceBossStats.iceBossMidOrb && !iceBossStats.iceBossAttemptOrb)
			AttackStart();
	}

	void ActivateLaser()
	{
		Laser();
	}

	void ActivateOrb()
	{
		iceBossStats.iceBossAttemptOrb = true;
	}

	void ResetPatternVar()
	{
		iceBossStats.iceBossPerformingPattern = false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && iceBossStats.iceBossMidAttack)
		{
			healthScript.LoseHealthBy(1);
		}
	}

}

/* Curved Movement Code

Vector3[] movementPoints = new Vector3[4];
bool curvedMovement = false;
int curvedMovementIndex = 0;

if (curvedMovement && curvedMovementIndex < 4)
	{
		transform.position = Vector3.MoveTowards(transform.position, movementPoints[curvedMovementIndex], 10f * Time.deltaTime);
		if (transform.position == movementPoints[curvedMovementIndex])
			curvedMovementIndex++;
	}
	else
	{
		curvedMovement = false;
		curvedMovementIndex = 0;
	}

void CurvedMovement()
	{
		movementPoints[0] = gameObject.transform.position + new Vector3(0,1,0);
		movementPoints[1] = gameObject.transform.position + new Vector3(1,4,0);
		movementPoints[2] = gameObject.transform.position + new Vector3(3,6,0);
		movementPoints[3] = gameObject.transform.position + new Vector3(6,7,0);
		curvedMovement = true;
		curvedMovementIndex = 0;
	}

*/