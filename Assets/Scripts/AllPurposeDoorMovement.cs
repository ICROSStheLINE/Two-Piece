using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPurposeDoorMovement : MonoBehaviour
{
	GameObject dialogue;
	SpeechBubbleScript speechBubbleScript;
	
	[SerializeField] int numberOfDoors;
	GameObject[] arenaEntranceGates;
	
	[HideInInspector] public bool gateMustBeClosed = false;
	[HideInInspector] public bool gateMustBeOpened = false;
	
	Vector3[] gateStartingPoses;
	Vector3[] gateTargetPoses;
	
	[SerializeField] string doorName;
	[SerializeField] float doorSpeed;
	
	[SerializeField] string doorOpeningCondition;
	[SerializeField] float enemyDetectionRadius;
	[SerializeField] bool makeRadiusWide = false;

	[SerializeField] bool dialogueWhenDoorOpens;
	[SerializeField] string[] potentialDialogue;
	[SerializeField] float textSpeed;
	[SerializeField] string preDialogueAction;
	[SerializeField] string postDialogueAction;

    void Start()
    {
		dialogue = GameObject.FindWithTag("Dialogue");
		speechBubbleScript = dialogue.GetComponent<SpeechBubbleScript>();
		
		arenaEntranceGates = new GameObject[numberOfDoors];
		gateStartingPoses = new Vector3[numberOfDoors];
		gateTargetPoses = new Vector3[numberOfDoors];
		
		for (int i = 0; i < arenaEntranceGates.Length; i++)
		{
			arenaEntranceGates[i] = gameObject.transform.Find(doorName + (i+1)).gameObject;
			gateTargetPoses[i] = arenaEntranceGates[i].transform.position + ((-arenaEntranceGates[i].transform.up) * 9);
			gateStartingPoses[i] = arenaEntranceGates[i].transform.position;
		}
    }

	void Update()
	{
		if (gateMustBeOpened)
		{
			CheckForDialogue();
			
			for (int i = 0; i < arenaEntranceGates.Length; i++)
			{
				Vector3 gateCurrentPos = arenaEntranceGates[i].transform.position;
				arenaEntranceGates[i].transform.position = Vector3.MoveTowards(gateCurrentPos, gateStartingPoses[i], doorSpeed * Time.deltaTime);
			}
		}
		else if (gateMustBeClosed)
		{
			CheckDoorOpeningCondition();
			
			for (int i = 0; i < arenaEntranceGates.Length; i++)
			{
				Vector3 gateCurrentPos = arenaEntranceGates[i].transform.position;
				arenaEntranceGates[i].transform.position = Vector3.MoveTowards(gateCurrentPos, gateTargetPoses[i], doorSpeed * Time.deltaTime);
			}
		}
	}

	void CheckDoorOpeningCondition()
	{
		if (doorOpeningCondition == "Killing All Enemies")
		{
			if (!makeRadiusWide)
			{
				Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, enemyDetectionRadius, 01000000);
				if (hitCollider == null)
				{
					gateMustBeOpened = true;
				}
			}
			else
			{
				Collider2D hitCollider1 = Physics2D.OverlapCircle(transform.position + new Vector3(25,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider2 = Physics2D.OverlapCircle(transform.position + new Vector3(20,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider3 = Physics2D.OverlapCircle(transform.position + new Vector3(15,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider4 = Physics2D.OverlapCircle(transform.position + new Vector3(10,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider5 = Physics2D.OverlapCircle(transform.position + new Vector3(5,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider6= Physics2D.OverlapCircle(transform.position + new Vector3(0,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider7 = Physics2D.OverlapCircle(transform.position + new Vector3(-5,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider8 = Physics2D.OverlapCircle(transform.position + new Vector3(-10,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider9 = Physics2D.OverlapCircle(transform.position + new Vector3(-15,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider10 = Physics2D.OverlapCircle(transform.position + new Vector3(-20,0,0), enemyDetectionRadius, 01000000);
				Collider2D hitCollider11 = Physics2D.OverlapCircle(transform.position + new Vector3(-25,0,0), enemyDetectionRadius, 01000000);
				if (hitCollider1 == null && hitCollider2 == null && hitCollider3 == null && hitCollider4 == null && hitCollider5 == null && hitCollider6 == null && hitCollider7 == null && hitCollider8 == null && hitCollider9 == null && hitCollider10 == null && hitCollider11 == null)
				{
					gateMustBeOpened = true;
				}
			}
		}
	}
	
	void CheckForDialogue()
	{
		if (dialogueWhenDoorOpens)
		{
			dialogueWhenDoorOpens = false;
			dialogue.SetActive(true);
			speechBubbleScript.textComponent.text = string.Empty;
			//speechBubbleScript.textComponent.color = new Color(1f,1f,1f,1f);
			speechBubbleScript.lines = potentialDialogue;
			speechBubbleScript.textSpeed = textSpeed;
			speechBubbleScript.preDialogueAction = preDialogueAction;
			speechBubbleScript.postDialogueAction = postDialogueAction;
			speechBubbleScript.StartDialogue();
		}
	}
}
