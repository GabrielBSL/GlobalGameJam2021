using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void CloseGame(){
        Application.Quit();
    }

    public void SaveChanges()
    {
        PlayerPrefs.Save();
    }

}
