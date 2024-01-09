using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using SimpleJSON;
using DigitalRuby.Tween;

public class selectedItemShopPanel : MonoBehaviour
{
    public string itemTitle;
    public int itemPrice; 
    public string itemDescription;
    public string itemID;
    public string storeID;
    public static selectedItemShopPanel instance { get; protected set; }
    public Button backButton;
    public Button buyButton;
    public Item selectedItem;

    public GameObject buy, processing;
    private CanvasGroup canvasGroup;

    public Transform helmPosition;
    public Transform armorPosition;
    public Transform legsPosition;


    public Mesh mesh;
    public Material mat;
    public ItemCategory itemCat;





    public Text shopTitle;
    public Text title;
    public Text rarity;
    public Text description;



    [HideInInspector]
    public string newOrderID;

    private void Start()
    {

        instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
       // buyButton.onClick.AddListener(DefinePurchase);
    }


    private void TweenRotate()
    {
        GameObject rotObject = processing.GetComponentInChildren<Image>().gameObject;

        System.Action<ITween<float>> circleRotate = (t) =>
        {
            // start rotation from identity to ensure no stuttering
            rotObject.transform.rotation = Quaternion.identity;
            rotObject.transform.Rotate(Camera.main.transform.forward, t.CurrentValue);
        };

        float startAngle = rotObject.transform.rotation.eulerAngles.z;
        float endAngle = startAngle + 720.0f;

        // completion defaults to null if not passed in
        rotObject.gameObject.Tween("RotateCircle", startAngle, endAngle, 1.0f, TweenScaleFunctions.CubicEaseInOut, circleRotate);
    }

    public void setupPanel() {
        title.text = itemTitle;
        gameManager.instance.localPlayer.SetActive(false);
        helmPosition.gameObject.SetActive(false);
        armorPosition.gameObject.SetActive(false);
        legsPosition.gameObject.SetActive(false);

        switch (itemCat) {
            case (ItemCategory.helm):
                helmPosition.GetComponent<MeshFilter>().mesh = mesh;
                helmPosition.GetComponent<MeshRenderer>().material = mat;
                helmPosition.gameObject.SetActive(true);
                break;
            case (ItemCategory.armor):
                armorPosition.GetComponent<MeshFilter>().mesh = mesh;
                armorPosition.GetComponent<MeshRenderer>().material = mat;
                armorPosition.gameObject.SetActive(true);
                break;
            case (ItemCategory.leg):
                legsPosition.GetComponent<MeshFilter>().mesh = mesh;
                legsPosition.GetComponent<MeshRenderer>().material = mat;
                legsPosition.gameObject.SetActive(true);
                break;


        }
        //description.text = itemDescription;

    
    }

    //public void FinishPurchase()
    //  {
    //     var request = new ConfirmPurchaseRequest { OrderId = newOrderID };
    //      PlayFabClientAPI.ConfirmPurchase(request, LogSuccess, LogFailure);
    //  }

   // private void LogSuccess(ConfirmPurchaseResult result)
    //   {
    //       newOrderID = null;
    //     
   //        //Inform player that he successfully bought item.
    //  }

       void LogFailure(PlayFabError error)
       {
           newOrderID = null;
           Debug.LogError(error.GenerateErrorReport());
    
      }

   
}
