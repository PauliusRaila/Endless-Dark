using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Invector.vCharacterController;


public class Skill : MonoBehaviour
{
   public Sprite skillSprite;
   private Image skillImage;
   public Color fadedColor, normalColor;
   public int skillID;

    private void Start() {
        skillImage = GetComponent<Image>();
        skillImage.sprite = skillSprite;
   }

    public void OnPointerEnter() {
        skillImage.color = normalColor;
        //vThirdPersonController.instance.selectedSkillID = skillID;
    }

    public void OnPointerExit()
    {        
        skillImage.color = fadedColor;
        //vThirdPersonController.instance.selectedSkillID = 0;
    }

    public void castSkill() {
        
        
        UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.SetButtonDown("Y");
        UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.SetButtonUp("Y");
    }


}
