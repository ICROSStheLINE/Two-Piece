using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int targetScene;
    
    public void SwitchScene()
	{
		SceneManager.LoadScene(targetScene);
	}
}
