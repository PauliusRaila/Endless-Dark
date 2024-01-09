using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class notification : MonoBehaviour
{
    public Image itemIcon;
    public Text itemAmount;
    public Text notificationText;
    public Transform notificationPosition;
    

    public static notification instance { get; set; }
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    private void TweenMove()
    {
        System.Action<ITween<Vector3>> updateCirclePos = (t) =>
        {
            gameObject.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> circleMoveCompleted = (t) =>
        {
            Debug.Log("Circle move completed");
        };

        Vector2 startPos = transform.position;
        Vector2 endPos = notificationPosition.position;

        // completion defaults to null if not passed in
        gameObject.Tween("MoveNotification", startPos, endPos, 0.8f, TweenScaleFunctions.CubicEaseIn, updateCirclePos)
            .ContinueWith(new Vector3Tween().Setup(endPos, endPos, 4f, TweenScaleFunctions.CubicEaseOut, updateCirclePos))
            .ContinueWith(new Vector3Tween().Setup(endPos, startPos, 1.5f, TweenScaleFunctions.CubicEaseOut, updateCirclePos, circleMoveCompleted)); ;
    
    }

    public void showNotification(Sprite icon, string name, int amount) {

        itemIcon.sprite = icon;
        itemAmount.text = amount.ToString() + "  x";
        notificationText.text = name + "  added  to  inventory";
       
        TweenMove();

    }





}
