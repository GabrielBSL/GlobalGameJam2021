using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public enum DestinationIdenifier
    {
        A, B, C
    }
    
    public Transform spawnPoint;

    public string scene;
    [SerializeField] DestinationIdenifier portalIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (scene == "") return;

            StartCoroutine(TransiteScene());
        }
    }

    public void RemoteTransite(string sceneName, DestinationIdenifier index)
    {
        scene = sceneName;
        portalIndex = index;
        StartCoroutine(TransiteScene());
    }

    public IEnumerator TransiteScene()
    {
        DontDestroyOnLoad(gameObject);

        yield return FindObjectOfType<Fader>().FadeIn();

        yield return SceneManager.LoadSceneAsync(scene);

        GoToScene otherPortal = GetOtherPortal();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = otherPortal.spawnPoint.position;
        player.GetComponent<HitManager>().SetSceneValues(scene, portalIndex);

        FindObjectOfType<Fader>().AlphaOne();
        yield return FindObjectOfType<Fader>().FadeOut();

        Destroy(gameObject);
    }

    private GoToScene GetOtherPortal()
    {
        foreach (GoToScene portal in FindObjectsOfType<GoToScene>())
        {
            if (portal == this) continue;
            if (portal.portalIndex != portalIndex) continue;

            return portal;
        }

        return null;
    }
}
