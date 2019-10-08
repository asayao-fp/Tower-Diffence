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

        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,levelnum);
        PlayerPrefs.SetInt(UserData.USERDATA_EXP,obj.GetComponent<expData>().getExp(levelnum));

        level.text = "LEVEL : " + levelnum;

    }
    public void nextScene(){

        snm = GetComponent<SkillNumManager>();
        gs.setStatus(snm.getAllStatus());
        SceneManager.LoadScene("TestScene");

    }

}
