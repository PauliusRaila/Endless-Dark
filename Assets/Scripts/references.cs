using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class references : MonoBehaviour
{
    public static references instance { get; set; }

    public selectedItemPanel selectedItemPanel;



    void Start()
    {
        instance = this;
    }

}
