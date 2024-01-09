using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class newMenu : MonoBehaviour {

    private ScrollRect mainMenuScroll;
    private IEnumerator coroutine;
    public float menuPosition;


    float lerpTime = 1f;
    float currentLerpTime;
    float menuPos;

    float moveDistance = 10f;

    Vector2 startPos;
    Vector2 endPos;

    void OnEnable () {
        mainMenuScroll = GetComponent<ScrollRect>();
        mainMenuScroll.horizontalNormalizedPosition = 0;
        startPos = mainMenuScroll.normalizedPosition;
        endPos = new Vector2(0.5f, 0);
    }

    public void openHunters() {

        currentLerpTime = 0f;
        endPos = new Vector2(1, 0);
    }

    private void Update()
    {
        menuPosition = mainMenuScroll.horizontalNormalizedPosition;


        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float perc = currentLerpTime / lerpTime;
        transform.position = Vector2.Lerp(startPos, endPos, perc);
    }

    public void openBloodline() {
        
    }

}

