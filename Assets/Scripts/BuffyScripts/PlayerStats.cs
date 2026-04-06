using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	BoxCollider2D boxCollider;
	
	
	// Controls
	
	public KeyCode moveRightKey = KeyCode.D;
	public KeyCode moveLeftKey = KeyCode.A;
	public KeyCode aimUpKey = KeyCode.W;
	public KeyCode aimDownKey = KeyCode.S;
	public KeyCode dashKey = KeyCode.LeftShift;
	public KeyCode gravityShiftKey = KeyCode.I;
	public KeyCode teleportKey = KeyCode.U;
	public KeyCode basicAttackKey = KeyCode.J;
	public KeyCode orbKickKey = KeyCode.K;
	public KeyCode leechBlastKey = KeyCode.L;
	public KeyCode orbShieldKey = KeyCode.N;
	public KeyCode interactKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.V;
	
	
	
	MonoBehaviour[] allComponents;
	
	static readonly float deathZeroAnimationDurationSpeedMultiplier = 0.5f;
	static readonly float deathZeroAnimationDuration = 0.75f / deathZeroAnimationDurationSpeedMultiplier;

	[HideInInspector] public bool playerMidActionNoDash = false;
	
	// Movement
	[HideInInspector] public float playerMovementSpeed = 7f;
	[HideInInspector] public bool isMoving = false;
	[HideInInspector] public bool playerCanMove = true;
	[HideInInspector] public bool midCutscene = false;
	[HideInInspector] public bool isSprinting = false;
	[HideInInspector] public float sprintSpeedMultiplier = 3f;

    // Dash
	[HideInInspector] public bool playerIsDashing = false;
	[HideInInspector] public bool playerIsDashingButResets1MillisecondEarlier = false;
	[HideInInspector] public bool playerCanDash = true;
	
	// Gravity Shift
	[HideInInspector] public bool playerMidGravityShift = false;
	
	// Teleport
	[HideInInspector] public bool playerMidTeleport = false;
	[HideInInspector] public bool playerQueuingTeleport = false;
	
	// Shielding
	[HideInInspector] public bool playerMidShielding = false;
	
	// TSO Stats
	[HideInInspector] public bool playerMidTSOAttack = false;
	[HideInInspector] public bool canTSOAttack = true;

	// TSO Kick
	[HideInInspector] public bool playerMidKickingTSO = false;
	[HideInInspector] public bool playerMidKickingTSOButForTheCameraGameObject = false;
	
	// TSO Leech Blast
	[HideInInspector] public bool playerMidLeechBlast = false;
	
	// TSO Basic Attack
	[HideInInspector] public bool isTSOBasicAttacking = false;
	
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		allComponents = GetComponents<MonoBehaviour>();
	}

	void Update()
	{
		if (playerMidGravityShift || playerMidTeleport || playerQueuingTeleport || playerMidShielding || playerMidKickingTSO || playerMidLeechBlast || playerMidKickingTSOButForTheCameraGameObject)
			playerMidActionNoDash = true;
		else
			playerMidActionNoDash = false;
		
		if (isTSOBasicAttacking || playerMidKickingTSO)
			playerMidTSOAttack = true;
		else
			playerMidTSOAttack = false;
	}



	public void IgnoreEnemyCollisions(bool so = default(bool))
	{
		if (so)
			boxCollider.excludeLayers = 01000000;
		else
			boxCollider.excludeLayers = 00000000;
	}

	// PlayerDashing
    public void ResetPlayerDashCooldown()
	{
		playerIsDashing = false;
		anim.SetBool("isDashing", false);
		rb.velocity = new Vector2(0,0);
	}
	// PlayerDashing
	void ResetTheIsDashingButResets1MillisecondEarlierVariableSoThatTurningWhileDashingIsntGlitched()
	{
		playerIsDashingButResets1MillisecondEarlier = false;
	}
	// PlayerKickingTSO
	void ResetPlayerMidKickingTSOButForTheCameraGameObjectVariable()
	{
		playerMidKickingTSOButForTheCameraGameObject = false;
	}
	
	public void Die(int deathType = default(int))
	{
		ResetPlayerDashCooldown();
		anim.SetBool("isAttacking", false);
		anim.SetBool("isDashing", false);
		anim.SetBool("isGravityShifting", false);
		anim.SetBool("isTeleporting", false);
		anim.SetBool("kickingTSO", false);
		anim.SetBool("kickingTSOP2", false);
		anim.SetBool("isLeechBlasting", false);
		anim.SetBool("isLeechBlastingW", false);
		anim.SetBool("isLeechBlastingL", false);
		anim.SetBool("isDying" + deathType, true);
		DeactivateAllFunction();
		anim.enabled = true;
		Invoke("DeactivateAllFunction", deathZeroAnimationDuration);

		Invoke("SwitchToMainMenu", deathZeroAnimationDuration + 2);
	}
	
	void SwitchToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
	
	void DeactivateAllFunction() // Deactivates all the SCRIPTS plus extra stuff
	{
		anim.enabled = false;
		rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		foreach(MonoBehaviour i in allComponents)
		{
			if (i != GetComponent<PlayerStats>())
			{
				i.CancelInvoke();
				i.StopAllCoroutines();
				i.enabled = false;
			}
		}
	}
}
