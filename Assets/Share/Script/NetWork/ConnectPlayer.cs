using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectPlayer : MonoBehaviour
{
    GameObject canvas;

    GameObject panel;

    private PhotonView photonView;

    void Start()
    {

        DontDestroyOnLoad(this);
        photonView = GetComponent<PhotonView>();

        canvas = GameObject.Find("Canvas");
        foreach(Transform child in canvas.transform){
            if(child.gameObject.name.Equals("aitemitukatta")){
                child.gameObject.SetActive(true);
            }
        }

        StartCoroutine("GameStart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getParent(){
        return this.photonView.isMine;
    }

    public IEnumerator GameStart(){

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("OnlineGameScene");


    }
}
