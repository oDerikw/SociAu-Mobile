using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{   
    // Trocar de scenas
  public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quitar do jogo
    public void QuitGame(){
        Application.Quit();
    }
}