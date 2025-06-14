using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleScript : MonoBehaviour
{
	GameObject player;
	PlayerStats playerStats;
	SaveData saveData;
	
	public TextMeshProUGUI textComponent;
	public string[] lines;
	public float textSpeed;
	
	public string preDialogueAction;
	public bool stopDash;
	public string postDialogueAction;
	
	int index;
	
    void Start()
    {
		player = GameObject.FindWithTag("Player");
		if (player)
		{
			playerStats = player.GetComponent<PlayerStats>();
			saveData = player.GetComponent<SaveData>();
		}
		
        textComponent.text = string.Empty;
		//StartDialogue();
		Invoke("Retardation", 0.1f);
    }

    void Retardation()
	{
		gameObject.SetActive(false);
		/*if (player)
		{
			playerStats.midCutscene = false;
			playerStats.playerCanMove = true;
			playerStats.playerCanDash = true;
		}*/
	}
	
    void Update()
    {
		if (stopDash == true)
			CancelInvoke("Retardation");
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (textComponent.text == lines[index])
			{
				NextLine();
			}
			else
			{
				StopAllCoroutines();
				textComponent.text = lines[index];
			}
		}
    }
	
	public void StartDialogue()
	{
		CheckPreDialogueAction();
		index = 0;
		StartCoroutine(TypeLine());
	}
	
	IEnumerator TypeLine()
	{
		// Type each character 1 by 1
		foreach (char c in lines[index].ToCharArray())
		{
			textComponent.text += c;
			yield return new WaitForSeconds(textSpeed);
		}
	}
	
	void NextLine()
	{
		if (index < lines.Length - 1)
		{
			index++;
			textComponent.text = string.Empty;
			StartCoroutine(TypeLine());
		}
		else // AT THE END OF THE DIALOGUE
		{
			gameObject.SetActive(false);
			if (player)
			{
				playerStats.midCutscene = false;
				playerStats.playerCanMove = true;
				playerStats.playerCanDash = true;
			}
			CheckPostDialogueAction();
		}
	}

	void CheckPreDialogueAction()
	{
		if (preDialogueAction == "Cutscene Mode")
		{
			playerStats.midCutscene = true;
			playerStats.playerCanMove = false;
			playerStats.playerCanDash = false;
		}
		if (stopDash)
		{
			playerStats.ResetPlayerDashCooldown();
		}
	}

	void CheckPostDialogueAction()
	{
		if (postDialogueAction == "Next Scene")
		{
			GetComponent<MenuManager>().SwitchScene();
		}
		if (postDialogueAction == "Save Game")
		{
			saveData.SaveToJson();
		}
	}
}
