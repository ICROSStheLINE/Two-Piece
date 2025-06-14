using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Louis : MonoBehaviour
{
	GameObject dialogue;
	SpeechBubbleScript speechBubbleScript;
	GameObject player;
	PlayerStats playerStats;
	Animator anim;
	
	bool waitForInteraction = true;
	[SerializeField] string[] potentialDialogue;
	[SerializeField] float textSpeed;
	[SerializeField] string preDialogueAction;
	[SerializeField] bool stopDash;
	[SerializeField] string postDialogueAction;
	
	static readonly float louisTurningDurationSpeedMultiplier = 0.7f;
	static readonly float louisTurningAnimationDuration = 0.25f / louisTurningDurationSpeedMultiplier;
	//static readonly float louisTurningAnimationFrames = 3;
	static readonly float louisWalkingDurationSpeedMultiplier = 0.5f;
	static readonly float louisWalkingAnimationDuration = 0.333f / louisWalkingDurationSpeedMultiplier;
	//static readonly float louisWalkingAnimationFrames = 3;
	static readonly float louisWavingDurationSpeedMultiplier = 0.5f;
	static readonly float louisWavingAnimationDuration = 0.5f / louisWavingDurationSpeedMultiplier;
	//static readonly float louisWavingAnimationFrames = 6;
	
	float movement = 0;
	[SerializeField] float movementSpeed = 0.1f;

    void Start()
    {
		dialogue = GameObject.FindWithTag("Dialogue");
		speechBubbleScript = dialogue.GetComponent<SpeechBubbleScript>();
		player = GameObject.FindWithTag("Player");
		playerStats = player.GetComponent<PlayerStats>();
		anim = gameObject.GetComponent<Animator>();
    }

	void Update()
	{
		/*if (Input.GetKeyDown("/"))
		{
			StartCoroutine(MoveSteps(2));
		}
		if (Input.GetKeyDown("."))
		{
			StartCoroutine(MoveSteps(-2));
		}*/
	}

    void FixedUpdate()
    {
		transform.position += new Vector3(movement,0,0);
    }
	
	IEnumerator MoveSteps(int stepCount = default(int)) // Keep stepCount a multiple of 2 lol
	{
		transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(stepCount), transform.localScale.y, transform.localScale.z);
		anim.SetTrigger("Turning");
		yield return new WaitForSeconds(louisTurningAnimationDuration);
		anim.SetBool("Walking", true);
		movement = movementSpeed * Mathf.Sign(stepCount);
		yield return new WaitForSeconds(louisWalkingAnimationDuration * Mathf.Abs(stepCount) / 2); // (Dividing by 2 here since he takes 2 steps in the animation)
		anim.SetBool("Walking", false);
		movement = 0;
		yield return new WaitForSeconds(louisTurningAnimationDuration);
		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if ((collision.gameObject.tag == "Player") && Input.GetKey(playerStats.interactKey) && waitForInteraction)
		{
			dialogue.SetActive(true);
			speechBubbleScript.textComponent.text = string.Empty;
			//speechBubbleScript.textComponent.color = new Color(1f,1f,1f,1f);
			speechBubbleScript.lines = potentialDialogue;
			speechBubbleScript.textSpeed = textSpeed;
			speechBubbleScript.preDialogueAction = preDialogueAction;
			speechBubbleScript.stopDash = stopDash;
			speechBubbleScript.postDialogueAction = postDialogueAction;
			speechBubbleScript.StartDialogue();
			waitForInteraction = false;
			
			anim.SetTrigger("Waving");
		}
	}
}

