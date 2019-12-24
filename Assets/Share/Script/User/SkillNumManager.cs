//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SkillNumManager : MonoBehaviour
{
    private int skill; //振り分けられるパラメータの合計数
    private int usenum; //振り分けられたパラメータ数

    public TextMeshProUGUI skilltext4statue;

    public TextMeshProUGUI skilltext4gobrin;
    bool isshow ;
    public GameObject panel4statue; //スキル割り振りのパネル
    public GameObject panel4gobrin; //スキル割り振りのパネル

    bool isshow4op;
    public GameObject optionpanel;　//スキル、必殺技選択画面のパネル

    bool isshow4dbpanel; 
    public GameObject dppanel; //必殺技設定のパネル

    bool isshow4os;
    public GameObject ospanel; //オブジェクト配置パネル

    SkillStatus[] sstatus;
    AddStatus[] astatus;

    private Toggle[] skillselect; //必殺技選択トグル

    public GameObject baggagepanel;//所持品のパネル
    bool isshowbp;
    public Image goblinPanel;
    public Image statuePanel;
    public Image background;
    private GameSettings gs;
    void Start()
    {
        usenum = 0;
        skill = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);
        skilltext4statue.text = "SKILL : " + skill;
        skilltext4gobrin.text = "SKILL : " + skill;
        isshow = false;
        isshow4op = false;
        isshow4dbpanel = false;
        isshow4os = false;
        isshowbp = false;



        skillselect = new Toggle[5];
        int count = 0;
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


        
        //Statue
        String str = ""; 
        String nokori = PlayerPrefs.GetString(UserData.USERDATA_STATUE);
        String[] statuestr = new String[5];
        for(int i=0;i<5;i++){
            /*
            if(nokori.IndexOf(",") == -1){
                str = nokori;
            }else{
                str = nokori.Substring(0,nokori.IndexOf(","));
                nokori = nokori.Substring(nokori.IndexOf(",") + 1);
            }*/
            str = "facility_" + (i+1);
            statuestr[i] = str;
        }

        //Goblin
        str = ""; 
        nokori = PlayerPrefs.GetString(UserData.USERDATA_GOBLIN);
        String[] goblinstr = new String[3];
        for(int i=0;i<3;i++){
            /*
            if(nokori.IndexOf(",") == -1){
                str = nokori;
            }else{
                str = nokori.Substring(0,nokori.IndexOf(","));
                nokori = nokori.Substring(nokori.IndexOf(",") + 1);
            }
            */
            str = "gobrin_" + (i + 1);
            goblinstr[i] = str;
        }

        count = 0;
        Transform tt = panel4statue.transform;
        foreach(Transform child in tt){
            if(child.gameObject.name.StartsWith("status_")){
                child.gameObject.GetComponent<SkillStatus>().setName(statuestr[count]);
                count++;
            }
        }

        count = 0;
        tt = panel4gobrin.transform;
        foreach(Transform child in tt){
            if(child.gameObject.name.StartsWith("status_")){
                child.gameObject.GetComponent<SkillStatus>().setName(goblinstr[count]);
                
                count++;
            }
        }

        changeType();
    }

    //スタチュー、ゴブリンのタイプが変更された
    public void changeType(){

        if(gs == null){
            gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();

        }

        int count = 0;

        sstatus = new SkillStatus[gs.isStatue() ? 5 : 3];
        Transform transform = gs.isStatue() ? panel4statue.transform : panel4gobrin.transform;
        foreach(Transform child in transform){
            if(child.gameObject.name.StartsWith("status_")){
                sstatus[count++] = child.gameObject.GetComponent<SkillStatus>();
            }
        }

        int dpnum = gs.isStatue() ? 2 : 3;
        string[] dpname = gs.isStatue() ? new string[]{"skill1","skill2"} : new string[]{"skill1","skill2","skill3"};
        foreach(Transform child in dppanel.transform){
            if(!child.gameObject.name.StartsWith("skill")){
                continue;
            }
            child.gameObject.SetActive(false);
            for(int i=0;i<dpname.Length;i++){
                if(child.gameObject.name.Equals(dpname[i])){
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }

        for(int i=0;i<skillselect.Length;i++){
            if(i >= dpnum)continue;
            if(i == 0){
                skillselect[i].isOn = true;
                gs.setSkillType(i);
            }else{
                skillselect[i].isOn = false;
            }
        }


        usenum = 0;
        updateLayout();

        statuePanel.gameObject.SetActive(gs.isStatue());
        goblinPanel.gameObject.SetActive(!gs.isStatue());

    }

    public void updateLayout(){
        if(gs.isStatue()){
            skilltext4statue.text = "SKILL :  " + (skill - usenum);

        }else{
            skilltext4gobrin.text = "SKILL :  " + (skill - usenum);
        }
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
        SoundManager.SoundPlay("click1",this.gameObject.name);

        usenum++;
        updateLayout();
    }

    public void minusStatus(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        usenum--;
        updateLayout();

    }

    public void showPanel(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        isshow = !isshow;

        if(gs.isStatue()){
            panel4statue.gameObject.SetActive(isshow);
            panel4gobrin.gameObject.SetActive(false);

        }else{
            panel4statue.gameObject.SetActive(false);
            panel4gobrin.gameObject.SetActive(isshow);

        }

    }

    public void showPanel4bp(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        isshowbp = !isshowbp;
        baggagepanel.gameObject.SetActive(isshowbp);
        if(isshowbp){
            baggagepanel.GetComponent<UseItemManager>().setBaggage();
        }

    }

    public void showPanel4op(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        isshow4op = !isshow4op;
        optionpanel.gameObject.SetActive(isshow4op);
    }

    public void showPanel4dp(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        isshow4dbpanel = !isshow4dbpanel;
        dppanel.gameObject.SetActive(isshow4dbpanel);
    }

    public void showPanel4os(){
        /*
        SoundManager.SoundPlay("click1",this.gameObject.name);


        isshow4os = !isshow4os;
        ospanel.gameObject.SetActive(isshow4os);
        */
    }

    public AddStatus[] getAllStatus(){
        astatus = new AddStatus[gs.isStatue() ? 5 : 3];
        for(int i=0;i<astatus.Length;i++){
            astatus[i] = sstatus[i].GetStatus();
        }
        
        /*
        for(int i=0;i<skillselect.Length;i++){
            if(skillselect[i].isOn){
                gs.setSkillType(i);
                break;
            }
        }*/

        if(gs.isStatue()){
            gs.setSkillType(1);
        }else{
            gs.setSkillType(2);
        }
        return astatus;
    }

}
