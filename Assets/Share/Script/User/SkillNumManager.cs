using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillNumManager : MonoBehaviour
{
    private int skill; //振り分けられるパラメータの合計数
    private int usenum; //振り分けられたパラメータ数

    public TextMeshProUGUI skilltext;
    bool isshow ;

    public GameObject panel;

    void Start()
    {
        usenum = 0;
        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,15);
        skill = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);

        skilltext.text = "SKILL : " + skill;
        isshow = false;
        showPanel();
    }

    public void updateLayout(){
        skilltext.text = "SKILL : " + (skill - usenum);
    }

    //ステータスを変更できるか
    public bool checkStatus(int type){
        //up
        if(type == 1){
            return usenum < skill;
        }
        //down
        else if(type == 2){
            return 0 < usenum;
        }

        return false;
    }

    public void addStatus(){
        usenum++;
        updateLayout();
    }

    public void minusStatus(){
        usenum--;
        updateLayout();

    }

    public void showPanel(){
        isshow = !isshow;
        panel.gameObject.SetActive(isshow);

    }

}
