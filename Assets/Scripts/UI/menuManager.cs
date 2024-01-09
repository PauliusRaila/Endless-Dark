using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class menuManager : MonoBehaviour
{

    public CanvasGroup[] menuTabs;
    public int currentTab = 1;
    public Text selectedMenuTitle;
    public GameObject marker;
    private Vector2 markerPosition;
    public List<Transform> markerPos = new List<Transform>();

    public Animator lobbyDoor;

    public Transform itemsParent;
    public Transform utilitiesItemsParents;
    public Transform bagItems;

    public Transform categoriesParent;
    public selectedItemPanel selectedItemPanel;

    public Button backButton;
    public GameObject menuSmoke, menuSmoke2;
    

    public List<GameObject> characterInfoPages;
    public CanvasGroup mainCanvas;
    public static menuManager instance { get; set; }

    public Sprite commonSlot, selectedSlot;


    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
        mainCanvas = GameObject.Find("_MenuCanvas").GetComponent<CanvasGroup>();
    }

    public void changeTab(int index)
    {
        
        if (index == currentTab) return;
        // StopAllCoroutines();
        StartCoroutine(FadeImage(true, menuTabs[currentTab], 3));
        menuTabs[currentTab].gameObject.SetActive(false);
        menuTabs[index].alpha = 0;
        menuTabs[index].gameObject.SetActive(true);

        StartCoroutine(FadeImage(false, menuTabs[index], 3));

        currentTab = index;
        selectedMenuTitle.text = menuTabs[currentTab].name;
        markerPosition = markerPos[currentTab].position;

        TweenMove();

        if (index == 0)
        {
            menuSmoke.GetComponent<ParticleSystem>().Stop();
            menuSmoke2.GetComponent<ParticleSystem>().Play();
        }

        else {
            menuSmoke.GetComponent<ParticleSystem>().Play();
            menuSmoke2.GetComponent<ParticleSystem>().Stop();
        }
           


    }


    private void TweenMove()
    {
        System.Action<ITween<Vector3>> updateCirclePos = (t) =>
        {
            marker.GetComponent<RectTransform>().position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> circleMoveCompleted = (t) =>
        {
          //Debug.Log("Circle move completed");
        };

        Vector2 startPos = marker.transform.position;
        Vector2 endPos = markerPosition;

        // completion defaults to null if not passed in
        marker.Tween("MoveNotification", startPos, endPos, 0.2f, TweenScaleFunctions.Linear, updateCirclePos, circleMoveCompleted);                    
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

    public void changeInfoPage(int index) {
        for (int i = 0; i < characterInfoPages.Count; i++) {
            characterInfoPages[i].SetActive(false);
           
        }     
        characterInfoPages[index].SetActive(true);        
     }


    // When everything is loaded, open door and move into lobby, fade UI and enable smoke.
    public void startLobby() {
        Invoke("_startLobby", 2);
        
    }
    
    private void _startLobby() {

        lobbyDoor.SetTrigger("open");
        StartCoroutine(FadeImage(false, mainCanvas, 4));
        changeTab(1);
        Camera.main.GetComponent<DragCamera2D>().setCameraState(1);
        menuSmoke.SetActive(true);

    }

}
