using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public float faderTime = 1f;

    private CanvasGroup faderImage;

    private void Awake()
    {
        faderImage = GetComponent<CanvasGroup>();
    }

    public void AlphaOne()
    {
        faderImage.alpha = 1;
    }

    public IEnumerator FadeOut()
    {
        while(faderImage.alpha > 0)
        {
            faderImage.alpha = Mathf.MoveTowards(faderImage.alpha, 0, Time.deltaTime / faderTime);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        while (faderImage.alpha < 1)
        {
            faderImage.alpha = Mathf.MoveTowards(faderImage.alpha, 1, Time.deltaTime / faderTime);
            yield return null;
        }
    }
}
