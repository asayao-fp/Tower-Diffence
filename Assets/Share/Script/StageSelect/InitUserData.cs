using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InitUserData : MonoBehaviour
{

    public int levelnum;
    public TextMeshProUGUI level;
    private SkillNumManager snm;
    private GameSettings gs;
    

    void Start(){
        GameObject obj = GameObject.FindWithTag("StaticObjects");

        gs = obj.GetComponent<GameSettings>();

        level.text = "LEVEL : " + PlayerPrefs.GetInt(UserData.USERDATA_LEVEL);

    }
    public void nextScene(){
        SoundManager.SoundPlay("click1",this.gameObject.name);
        gs.setOnlineType(false);
        snm = GetComponent<SkillNumManager>();
        gs.setStatus(snm.getAllStatus());
//        SceneManager.LoadScene("TestScene");
         SceneManager.LoadScene(Constants.STAGE_SELECT_SCNENE);
    }

    public void onlineScene(){
        SoundManager.SoundPlay("click1",this.gameObject.name);
        gs.setOnlineType(true);
        snm = GetComponent<SkillNumManager>();
        gs.setStatus(snm.getAllStatus());
//        SceneManager.LoadScene("OnlineWait");
//         SceneManager.LoadScene(Constants.CONNECT_ROOM_SCNENE);
         SceneManager.LoadScene(Constants.ONRINE_WAIT_SCENE);
    }

}
