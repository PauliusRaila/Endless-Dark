using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vMelee;

public class Destruction : MonoBehaviour
{
    public GameObject destroyedItem;

    private void destruction()
    {       
        Instantiate(destroyedItem , this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weaponTrigger")
        {
            if (other.gameObject.GetComponent<BoxCollider>().enabled)
                destruction();
        }
    }
}
