using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPurposeDoorMovementP2 : MonoBehaviour
{
	GameObject dialogue;
	SpeechBubbleScript speechBubbleScript;
	GameObject player;
	PlayerStats playerStats;
	AllPurposeDoorMovement allPurposeDoorMovement;
	
	
	[SerializeField] bool activateDialogue;
	[SerializeField] string[] potentialDialogue;
	[SerializeField] float textSpeed;
	[SerializeField] string preDialogueAction;
	[SerializeField] bool stopDash;
	[SerializeField] string postDialogueAction;
	
	void Start()
	{
		dialogue = GameObject.FindWithTag("Dialogue");
		speechBubbleScript = dialogue.GetComponent<SpeechBubbleScript>();
		player = GameObject.FindWithTag("Player");
		playerStats = player.GetComponent<PlayerStats>();
		allPurposeDoorMovement = transform.parent.GetComponent<AllPurposeDoorMovement>();
	}
	
	
	void OnTriggerEnter2D(Collider2D collision)
	{	
		if (collision.gameObject.tag == "Player")
		{
			if (allPurposeDoorMovement.gateMustBeClosed == false)
			{
				allPurposeDoorMovement.gateMustBeClosed = true;
				
				if (activateDialogue)
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
				}
			}
		}
	}
}
