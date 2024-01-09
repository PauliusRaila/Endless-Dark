using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appear : MonoBehaviour
{
    private Material mat;
    public float dissolveValue = 1;

    private void OnEnable()
    {

        mat = GetComponent<Renderer>().material;
        mat.SetFloat("Vector1_FEFF47F1", 1);
    }

    private void Update()
    {
        if (dissolveValue > 0)
        {
            dissolveValue -= Time.deltaTime * 0.3f;
            mat.SetFloat("Vector1_FEFF47F1", dissolveValue);
        }
        else
        {
            enabled = false;
        } 

    }
}
