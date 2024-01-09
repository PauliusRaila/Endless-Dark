using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bossHealth;
    public GameObject bossCamera;
    private Camera playerCamera;


    //Activate boss health after camera cinematics.
    public void activateHealth() {
        bossHealth.SetActive(true);
    }


    public void StartBossCinematics() {
        //Fade screen, change to cinematics camera,start boss music,, play animation , show boss name and fade again to player camera.

    }


}
