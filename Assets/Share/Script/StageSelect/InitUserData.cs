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
    void Start(){
        GameObject obj = GameObject.FindWithTag("StaticObjects");

        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,levelnum);
        PlayerPrefs.SetInt(UserData.USERDATA_EXP,obj.GetComponent<expData>().getExp(levelnum));

        level.text = "LEVEL : " + levelnum;

    }
    public void nextScene(){
        SceneManager.LoadScene("TestScene");

    }

}
