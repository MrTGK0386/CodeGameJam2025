using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadSceneAsync("Julien");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
