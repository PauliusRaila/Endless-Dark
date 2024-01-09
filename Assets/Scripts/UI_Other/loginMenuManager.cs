using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class loginMenuManager : MonoBehaviour
{

    public GameObject loginWindow;
    public GameObject splashWindow;
    public Image background;

    void Start()
    {
        RenderTexture rt;
        GameObject screen = GameObject.Find("Screen");
        screen.transform.localScale = new Vector3(screen.transform.localScale.y * Screen.width / Screen.height, screen.transform.localScale.y, screen.transform.localScale.z);

        float aspectRatio = Screen.width / (float)Screen.height;

        if (aspectRatio >= 2)
        {
            Debug.Log("18:9");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else if (aspectRatio >= 1.7f)
        {
            Debug.Log("16:9");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else if (aspectRatio >= 1.5f)
        {
            Debug.Log("3:2 FIX THIS");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else
        {
            Debug.Log("4:3 FIX THIS");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }

        rt.antiAliasing = 1;
        rt.dimension = TextureDimension.Tex2D;
        rt.format = RenderTextureFormat.ARGB64;
        rt.useDynamicScale = false;
        rt.filterMode = FilterMode.Point;
        rt.Create();

        Camera.main.targetTexture = rt;

        GameObject.Find("Screen").GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", rt);



    }

    public void goToLogin() {
        splashWindow.SetActive(false);
       // loginWindow.SetActive(true);
        //Color c;
       // c = background.color;
       // c.a = 255f;
       // background.color = c;
    }



}
