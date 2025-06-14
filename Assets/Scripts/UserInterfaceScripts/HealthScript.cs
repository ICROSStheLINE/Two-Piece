using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthScript : MonoBehaviour
{
	[SerializeField] Image healthBar;
	//[SerializeField] Image healthChunk; // This will be used to make the healthbar aesthetically pleasing when lowering
	
	GameObject player;
	Animator playerAnim;
	PlayerStats playerStats;
	RectTransform healthChunkRectTransform;
	RectTransform healthBarRectTransform;
	
	[SerializeField] GameObject damageStatic;
	SpriteRenderer playerSpriteRenderer;
	public float maxHealth = 15f;
	public float currentHealth = 15f;
	//int healthChunkPosition = 15;

    void Start()
    {
		player = GameObject.FindWithTag("Player");
		playerAnim = player.GetComponent<Animator>();
		playerStats = player.GetComponent<PlayerStats>();
		playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
		//healthChunkRectTransform = healthChunk.GetComponent<RectTransform>();
		healthBarRectTransform = healthBar.GetComponent<RectTransform>();

		//healthChunk.transform.position = healthBar.transform.position + new Vector3(4.15f,0f,0f) + new Vector3(((healthChunkRectTransform.rect.width / 2) * (healthChunkPosition - 1)) - (3.84f * (healthChunkPosition - 1)),0,0); // 3.84 is the magic number to keep the healthchunk in the exact perfect spot

        healthBar.fillAmount = (currentHealth / 15f);
		/*if (currentHealth != 15)
		{
			for (int i = 0; i < Mathf.Abs(15 - currentHealth); i += 1)
				healthChunk.transform.position -= new Vector3(((healthChunkRectTransform.rect.width / 2) + (healthBarRectTransform.rect.width / 250)),0,0);
		}*/
    }
	
	void Update()
	{
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
	}

	public void LoseHealthBy(int amount)
	{
		currentHealth -= amount;
		healthBar.fillAmount = (currentHealth / 15f);
		if (Mathf.Sign(amount) == -1)
		{
			return;
		}
		else if (currentHealth > 0)
		{
			SwitchColorsRetardedly();
		}
		else if (currentHealth == 0)
		{
			playerStats.Die();
		}
	}

	void SwitchColorsRetardedly()
	{
		GameObject damageStatic_ = Instantiate(damageStatic, player.transform.position, player.transform.rotation);
		damageStatic_.transform.parent = player.transform;
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
		playerSpriteRenderer.color = new Color(0f,0f,0f,1f);
	}

	void TurnGray()
	{
		playerSpriteRenderer.color = new Color(0.3f,0.3f,0.3f,1f);
	}

	void TurnNormal()
	{
		playerSpriteRenderer.color = new Color(1f,1f,1f,1f);
	}
}
