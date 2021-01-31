using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GoToScene;

public class HitManager : MonoBehaviour
{
    string scene;
    DestinationIdenifier portalIndex;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            StartCoroutine(StartDeath());
        }
    }

    public void SetSceneValues(string sceneName, DestinationIdenifier index)
    {
        scene = sceneName;
        portalIndex = index;
    }

    IEnumerator StartDeath()
    {
        GetComponent<PlayerController>().killPlayer();

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<GoToScene>().RemoteTransite(scene, portalIndex);
    }
}
