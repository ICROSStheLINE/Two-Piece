using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
	Animator anim;
	PlayerStats playerStats;
	GameObject healthBar;
	HealthScript healthScript;
    public PlayerPosition playerPosition = new PlayerPosition();
	
	static readonly float pettingDurationSpeedMultiplier = 0.4f;
	static readonly float pettingAnimationDuration = 0.833f / pettingDurationSpeedMultiplier;
	//static readonly float pettingAnimationFrames = 10;
	static readonly float drinkingDurationSpeedMultiplier = 0.4f;
	static readonly float drinkingAnimationDuration = 1.333f / drinkingDurationSpeedMultiplier;
	//static readonly float drinkingAnimationFrames = 16;
	
	Transform nearestBissbiss;
	
	void Start()
	{
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		healthBar = GameObject.FindWithTag("HealthBar");
		healthScript = healthBar.GetComponent<HealthScript>();
		LoadFromJson();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			SaveToJson();
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			LoadFromJson();
		}
	}
	
	public void SaveToJson()
	{
		playerPosition.xPosition = transform.position.x;
		playerPosition.yPosition = transform.position.y;
		playerPosition.zPosition = transform.position.z;
		
		string positionData = JsonUtility.ToJson(playerPosition);
		string filePath = Application.persistentDataPath + "/playerPositionData.json";
		//Debug.Log(filePath);
		System.IO.File.WriteAllText(filePath, positionData);
		//Debug.Log("Save Completed");
		StartCoroutine(DrinkThenPetCat());
	}

	public void LoadFromJson()
	{
		string filePath = Application.persistentDataPath + "/playerPositionData.json";
		string positionData = System.IO.File.ReadAllText(filePath);
		
		playerPosition = JsonUtility.FromJson<PlayerPosition>(positionData);
		//Debug.Log("Save Loaded");
		
		transform.position = new Vector3(playerPosition.xPosition, playerPosition.yPosition, playerPosition.zPosition);
	}

	IEnumerator DrinkThenPetCat()
	{
		playerStats.midCutscene = true;
		playerStats.playerCanMove = false;
		playerStats.playerCanDash = false;
		
		StartCoroutine(nearestBissbiss.gameObject.GetComponent<Bissbiss>().WalkToPosition(transform.position + new Vector3(1.5f*Mathf.Sign(transform.localScale.x),0,0)));
		
		anim.SetBool("isDrinking", true);
		SpriteRenderer tso = GameObject.FindWithTag("Truth Seeking Orb").GetComponent<SpriteRenderer>();
		tso.color = new Color(1,1,1,0);
		yield return new WaitForSeconds(drinkingAnimationDuration/2);
		healthScript.LoseHealthBy(-1);
		yield return new WaitForSeconds(drinkingAnimationDuration/2);
		tso.color = new Color(1,1,1,1);
		anim.SetBool("isDrinking", false);
		anim.SetBool("isPetting", true);
		yield return new WaitForSeconds(pettingAnimationDuration);
		playerStats.midCutscene = false;
		playerStats.playerCanMove = true;
		playerStats.playerCanDash = true;
		anim.SetBool("isPetting", false);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.gameObject.tag == "Checkpoint"))
		{
			nearestBissbiss = collision.transform.Find("Bissbiss");
		}
	}
}

[System.Serializable]
public class PlayerPosition
{
	public float xPosition;
	public float yPosition;
	public float zPosition;
}