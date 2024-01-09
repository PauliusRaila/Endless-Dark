using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public Text loadAmountText;
    public Image progressBar;

    void Start()
    {
       Invoke("LoadLevel", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadLevel() {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        PhotonNetwork.LoadLevel(gameManager.instance.sceneToLoad);

        while (PhotonNetwork.LevelLoadingProgress < 1)
        {       
            //progressBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
           // Debug.Log(PhotonNetwork.LevelLoadingProgress);
            yield return new WaitForEndOfFrame();
        }
    }

}
