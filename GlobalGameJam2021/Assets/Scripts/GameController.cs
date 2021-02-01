using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        FindObjectOfType<GoToScene>().RemoteTransite("Cemetery", GoToScene.DestinationIdenifier.C, false);
    }

    public void CloseGame(){
        Application.Quit();
    }

    public void SaveChanges()
    {
        PlayerPrefs.Save();
    }

}
