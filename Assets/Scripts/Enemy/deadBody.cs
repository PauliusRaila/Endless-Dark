using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class deadBody : MonoBehaviour {

    private Material mat;
    public float dissolveValue = 0;

	private void OnEnable () {

        mat = GetComponent<Renderer>().material;
        mat.SetFloat("Vector1_FEFF47F1", 0);
    }

    private void Update()
    {
        if (dissolveValue < 1) {
            dissolveValue += Time.deltaTime * 0.3f;
            mat.SetFloat("Vector1_FEFF47F1", dissolveValue);
        }
        else
        {
           Destroy(this.transform.parent.gameObject); 
        }

        if (dissolveValue > 0.5f && GetComponent<CapsuleCollider>().enabled)
        {

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
     
    }

}
