using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectRoomManager : MonoBehaviour
{
 void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log("サーバに接続");
        PhotonNetwork.ConnectUsingSettings(null);
    }

    void OnJoinedLobby(){
        Debug.Log("ロビーに入室");

        PhotonNetwork.JoinRandomRoom();
    }

    void OnJoinedRoom(){
        Debug.Log("ルームに入室しました");
        int l = PhotonNetwork.playerList.Length;
        if(l >= 2){
            Debug.Log("人数が揃ったよ");
            PhotonNetwork.Instantiate("takuma/Prefabs/AtkCheck", Vector3.zero, Quaternion.identity, 0);

            SceneManager.LoadScene("OnlineGameScene");

        }

    }

    void OnPhotonRandomJoinFailed(){
        Debug.Log("ルーム入室が失敗");
        
        PhotonNetwork.CreateRoom(null);
    }
}
