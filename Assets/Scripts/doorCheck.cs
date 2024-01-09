using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class doorCheck : MonoBehaviour
{
    public bool spawned;

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient) {
            if (other.gameObject.tag == "door" && spawned)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "door" && !spawned)
            {
                spawned = true;
            }
        }      
    }

}

