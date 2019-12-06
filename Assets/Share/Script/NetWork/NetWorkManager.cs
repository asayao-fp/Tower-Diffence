using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NetWorkManager : MonoBehaviour
{

    public GameObject SearchButton;
    public GameObject ReturnButton;
    public GameObject FindPanel;
    public TextMeshProUGUI FindLabel;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReturnMain(){

        SoundManager.SoundPlay("click1",this.gameObject.name);
        SceneManager.LoadScene("StageSelectScene");

    }

    public void SearchRoom(){
        SearchButton.gameObject.SetActive(false);
        ReturnButton.gameObject.SetActive(false);
        
        GameSettings.printLog("[NetWorkManager] SEARCH START");

        PhotonNetwork.ConnectUsingSettings(null);

    }

    void OnJoinedLobby(){
        GameSettings.printLog("[NetWorkManager] JOINED ROBBY");

        //生成されてる部屋にランダムで入室
        PhotonNetwork.JoinRandomRoom();

    }

    void OnJoinedRoom(){
        GameSettings.printLog("[NetWorkManager] JOIN ROOM");

        int num = PhotonNetwork.playerList.Length;

        if(num == 2){
            GameSettings.printLog("[NetWorkManager] MEMBER FULL");

            //StartCoroutine("GameStart");
            GameObject obj = PhotonNetwork.Instantiate("NetWork/NetObject", Vector3.zero, Quaternion.identity, 0) as GameObject;
           // GameObject gmobj = PhotonNetwork.Instantiate("NetWork/GameManager", Vector3.zero, Quaternion.identity, 0) as GameObject;

        }else if(num == 1){
            GameSettings.printLog("[NetWorkManager] MEMBER NOT FULL");
        }

    }

    void OnPhotonRandomJoinFailed(){
        GameSettings.printLog("[NetWorkManager] JOIN FAILED");
        PhotonNetwork.CreateRoom(null);
    }

//    public IEnumerator GameStart(){
       // DontDestroyOnLoad(this);
 //   }


}
