using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class potionSlider : MonoBehaviour , IDragHandler , IEndDragHandler
{
    // Start is called before the first frame update

    [Header("Limit Slider Area")]
    public bool clampSlider = true;
    public float sliderMaxX;
    public float sliderMinX;
    RectTransform rectTransform;
    Vector3 StartPosition;
    private CanvasScaler canvasScaler;
    private Vector2 ScreenScale
    {
        get
        {
            if (canvasScaler == null)
            {
                canvasScaler = transform.parent.GetComponentInParent<CanvasScaler>();
            }

            if (canvasScaler)
            {
                return new Vector2(canvasScaler.referenceResolution.x / Screen.width, canvasScaler.referenceResolution.y / Screen.height);
            }
            else
            {
                return Vector2.one;
            }
        }
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartPosition = rectTransform.position;
        sliderMaxX = rectTransform.position.x;   
        Debug.Log(rectTransform.position);
        Debug.Log(rectTransform.position.x);
        //sliderMaxX = sliderMaxX - 740;
    }


        // Update is called once per frame
        void Update()
        {

           sliderClamp();
   
        
        }


    // Clamp Camera to bounds
    private void sliderClamp()
    {


        if (rectTransform.position.x > sliderMaxX) {
            Debug.Log("clamp");
            rectTransform.position = new Vector2(sliderMaxX, rectTransform.position.y);
        }
           
        else if (rectTransform.anchoredPosition.x < sliderMinX)
            rectTransform.anchoredPosition = new Vector2(sliderMinX, rectTransform.anchoredPosition.y);

    }

    public void OnDrag(PointerEventData eventData)
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            // Debug.Log(rectTransform.anchoredPosition);
            for (int i = 0; i < Input.touchCount; i++)
            {

                if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    rectTransform.position = new Vector2(Input.GetTouch(i).position.x, rectTransform.position.y);
                }
            }
        }
        else
        {

           
                    rectTransform.position = new Vector2(Input.mousePosition.x, rectTransform.position.y);
           
        }
   
                
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.position = StartPosition;
    }
}
