using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SkillNumManager : MonoBehaviour
{
    private int skill; //振り分けられるパラメータの合計数
    private int usenum; //振り分けられたパラメータ数

    public TextMeshProUGUI skilltext;
    bool isshow ;
    public GameObject panel; //スキル割り振りのパネル

    bool isshow4op;
    public GameObject optionpanel;　//スキル、必殺技選択画面のパネル

    bool isshow4dbpanel; 
    public GameObject dppanel; //必殺技設定のパネル

    bool isshow4os;
    public GameObject ospanel; //オブジェクト配置パネル

    SkillStatus[] sstatus;
    AddStatus[] astatus;

    private Toggle[] skillselect; //必殺技選択トグル

    private GameSettings gs;
    void Start()
    {
        usenum = 0;
        skill = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);
        skilltext.text = "SKILL : " + skill;
        isshow = false;
        isshow4op = false;
        isshow4dbpanel = false;
        isshow4os = false;


        int count = 0;
        sstatus = new SkillStatus[5];
        foreach(Transform child in panel.transform){
            if(child.gameObject.name.StartsWith("status_")){
                sstatus[count++] = child.gameObject.GetComponent<SkillStatus>();
            }
        }

        skillselect = new Toggle[5];
        count = 0;
        foreach(Transform child in dppanel.transform){
            GameObject skillobj = child.gameObject;
            foreach(Transform chi in skillobj.transform){
                if(chi.gameObject.name.Equals("Toggle")){
                    skillselect[count++] = chi.gameObject.GetComponent<Toggle>();
                    break;
                }
            }
        }

        for(int i=0;i<skillselect.Length;i++){
            skillselect[i].isOn = false;
        }
        skillselect[0].isOn = true;

        gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();

        count= 1;
        foreach(Transform child in ospanel.transform){
            child.gameObject.name = PlayerPrefs.GetString(UserData.USERDATA_SETOBJ+(count++),"");
            if(count > 5)break;
        }
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

    public void showPanel4op(){
        isshow4op = !isshow4op;
        optionpanel.gameObject.SetActive(isshow4op);
    }

    public void showPanel4dp(){
        isshow4dbpanel = !isshow4dbpanel;
        dppanel.gameObject.SetActive(isshow4dbpanel);
    }

    public void showPanel4os(){
        isshow4os = !isshow4os;
        ospanel.gameObject.SetActive(isshow4os);
    }

    public AddStatus[] getAllStatus(){
        astatus = new AddStatus[5];
        for(int i=0;i<astatus.Length;i++){
            astatus[i] = sstatus[i].GetStatus();
        }
        
        for(int i=0;i<skillselect.Length;i++){
            if(skillselect[i].isOn){
                gs.setSkillType(i);
                break;
            }
        }
        return astatus;
    }

}
