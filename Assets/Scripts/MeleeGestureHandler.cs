using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GestureRecognizer;
using System.Linq;
using Invector.vCharacterController;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using Invector.vEventSystems;
using Invector.vCharacterController.vActions;
public class MeleeGestureHandler : MonoBehaviour {

    public static MeleeGestureHandler instance { protected set; get; }
    public Text textResult;
    private string gestureInput;
	public Transform referenceRoot;
    public PUN_ThirdPersonController cc;

    public int consumableUsed;

    //OLD COMBO SYSTEM
   // private int[] currentCombo = new int[3];
   // private List<int[]> correctCombos = new List<int[]>();
    [HideInInspector]
    public bool isSwipeAllowed = true;
   

    GesturePatternDraw[] references;

	void Start () {
      //  currentCombo[0] = 0;
      //  currentCombo[1] = 0;
      //  currentCombo[2] = 0;

        // ADD AVAILABLE COMBOS TO CORRECT COMBOS LIST
     //  int[] combo1 = new int[3];
     //   combo1[0] = 8; //UP
     //   combo1[1] = 8; //UP
     //   combo1[2] = 6; //RIGHT
      //  correctCombos.Add(combo1);


        instance = this;
		references = referenceRoot.GetComponentsInChildren<GesturePatternDraw> ();
        
     
	}

	void ShowAll(){
		for (int i = 0; i < references.Length; i++) {
			references [i].gameObject.SetActive (true);
		}
	}

	public void OnRecognize(RecognitionResult result){
		StopAllCoroutines ();
		ShowAll ();
        //isSwipeAllowed
		if (result != RecognitionResult.Empty && cc.GetComponent<PUN_MeleeCombatInput>().MeleeAttackStaminaConditions() && isSwipeAllowed) {
            // player.res = result;


            
            if (result.gesture.id == "right" || result.gesture.id == "left" || result.gesture.id == "up" || result.gesture.id == "down") {
               
                switch (result.gesture.id)
                {
                    case "up":

                        consumableUsed = 0;

                        break;
                    case "right":
                        consumableUsed = 1;

                        break;
                    case "left":
                        consumableUsed = 2;
                        break;                   
                    case "down":
                        consumableUsed = 3;
                        break;
                }


                //check if current combo matches correct combo list.

                //  foreach (int[] combo in correctCombos) {
                //      if (combo[0] == currentCombo[0] && combo[1] == currentCombo[1] && combo[2] == currentCombo[2]) {
                //          Debug.Log("COMBO DETECTED");
                //         cc.animator.SetInteger("AttackID", 5);
                //         cc.animator.SetInteger("ComboID", 0);
                //    
                //     }

                // }


                //if we perform combo attack, check again for stamina conditions, but for combo attack
                //and then decide if it's a combo attack or weak attack.
                //also make combo reset every few seconds if there is no successful hit.

                if (vHUDController.instance.stashSlots[consumableUsed].item != null) {
                    cc.GetComponent<PUN_GenericAnimation>().animationClip = vHUDController.instance.stashSlots[consumableUsed].item.actionName;
                    gestureInput = "A";
                    SetDownState();
                    SetUpState();
                }
                

                isSwipeAllowed = false;
                Invoke("resetSwipe", 0.35f);
            }
         


            textResult.text = result.gesture.id + "\n" + Mathf.RoundToInt (result.score.score * 100) + "%";
            
            Debug.Log(result.gesture.id);
            ///StartCoroutine (Blink (result.gesture.id));

        } else {
            Debug.Log("?");
			textResult.text = "";
		}
	}

    public void useConsumable()
    {

        vHUDController.instance.stashSlots[consumableUsed].item.Consume();

    }

    IEnumerator Blink(string id){
		var draw = references.Where (e => e.pattern.id == id).FirstOrDefault ();
		if (draw != null) {
			var seconds = new WaitForSeconds (0.02f);
			for (int i = 0; i <= 20; i++) {
				draw.gameObject.SetActive (i % 2 == 0);
				yield return seconds;
			}
			draw.gameObject.SetActive (true);
		}
	}

    private void resetSwipe() {
        isSwipeAllowed = true;
    }
    public void SetDownState()
    {
        CrossPlatformInputManager.SetButtonDown(gestureInput);
    }


    public void SetUpState()
    {
        CrossPlatformInputManager.SetButtonUp(gestureInput);
    }

  
}
