using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int targetScene;
    
    public void SwitchScene()
	{
		SceneManager.LoadScene(targetScene);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
