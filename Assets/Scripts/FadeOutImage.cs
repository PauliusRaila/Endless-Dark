using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutImage : MonoBehaviour
{
    private void Start()
    {

        Invoke("FadeOut", 2);

    }

    private void FadeOut() {

        StartCoroutine(FadeImage(true, GetComponent<CanvasGroup>(), 1));
    }
    public IEnumerator FadeImage(bool fadeAway, CanvasGroup cg, float fadeSpeed)
    {

        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i > 0; i -= fadeSpeed * Time.deltaTime)
            {
                // set color with i as alpha
                cg.alpha = i;

                yield return null;
            }

            cg.alpha = 0;


        }
        // fade from transparent to opaque
        else
        {
            // if (index == 5) yield return new WaitForSeconds(0.5f);
            // loop over 1 second
            for (float i = 0; i < 1; i += fadeSpeed * Time.deltaTime)
            {
                // set color with i as alpha
                cg.alpha = i;

                yield return null;
            }

            cg.alpha = 1;
        }

    }

}
