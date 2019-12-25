using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class GameSettings : MonoBehaviour
{

    [SerializeField]
    private Material Dark_material;
    [SerializeField]
    private Material Light_material;
    [SerializeField]
    private Material Generating_material;

    [SerializeField]
    private bool log;
    private static bool isLog; //ログを出力するか

    //制限時間
    [SerializeField]
    private int limitTime4gobrin = 120;
    [SerializeField]
    private int limitTime4statue = 60;
    [SerializeField]
    private int limitTime4online = 120;

    //召喚コストの上限
    [SerializeField]
    private int maxcost;

    [SerializeField]
    private bool isLight;

    [SerializeField]
    private bool isstatue; //攻撃側か守備側か true : statue , false : gobrin

    AddStatus[] addStatuses;
    private int skillType; //必殺技のタイプ

    private bool isOnline; //オンライン対戦かどうか

    public bool showVisualizer;

    //現在の広告表示カウント
    private int advertiseCount = 0;
    private GameSettings instance = null;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";
            GameObject duplicater = GameObject.Find(name);
            if(duplicater != null){
                Destroy(gameObject);
            }else{
                gameObject.name = name;
            }
        }else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (addStatuses == null)
        {
            addStatuses = new AddStatus[5];
        }
        skillType = 0;
        isLog = log;
    }
    
    public int getLimitTime(int type){
        switch(type){
            case 0:
                return limitTime4statue;
                break;
            case 1:
                return limitTime4gobrin;
                break;
            case 2:
                return limitTime4online;
                break;
        }
        return limitTime4gobrin;
    }

    public int getMaxCost(){
        return maxcost;
    }

    public bool getStatueType(){
        return isLight;
    }

    public Material getMaterial(){
        if(InputManager.generating){
            return Generating_material;
        }
        return isLight ? Light_material : Dark_material;

    }

    public void setStatue(bool isstatue){
        this.isstatue = isstatue;
    }
    public bool isStatue(){
        return isstatue;
    }
    public static void printLog(String msg){
        if(isLog){
            Debug.Log(msg);
        }
    }

    //ゲームシーンに遷移するときにAddStatusを設定
    public void setStatus(AddStatus[] astatus){
        addStatuses = astatus;

        GameSettings.printLog("[GameSettings] set Status");
        for(int i=0;i<addStatuses.Length;i++){
            GameSettings.printLog("[" + i + "] name : " + addStatuses[i].name + " , hp : " + addStatuses[i].hp + " , attack : " + addStatuses[i].attack + " , speed : " + addStatuses[i].speed);
        }        
    }

    public AddStatus getStatus(int num){
        return addStatuses[num];
    }

    public AddStatus getStatus(String name){
        for(int i=0;i<addStatuses.Length;i++){
            if(addStatuses[i].name.Equals(name)){
                return addStatuses[i];
            }
        }

        return null;
    }


    public void setSkillType(int type){
        skillType = type;
        GameSettings.printLog("[GameSettings] setSkillType : " + type);
    }

    public int getSkillType(){
        return skillType;
    }

    //スキル毎の最大使用回数
    public bool isUseSkill(int num)
    {
        int skillnum = 1;

        switch(skillType){
            case GameProgress.SKILL_RECOVERY:
                skillnum = 1;
                break;
            case GameProgress.SKILL_ENEMY_DEAD:
                skillnum = 2;
                break;
        }

        return skillnum <= num;
    }

    public void setOnlineType(bool isonline){
        this.isOnline = isonline;
    }
    public bool getOnlineType(){
        return this.isOnline;
    }

      public int getAdvertiseCount()
    {
        return this.advertiseCount;
    }

    public void setAdvertiseCount(int ac)
    {
        this.advertiseCount = ac;
    }
}

//スキル割り振りなどで追加されるステータス
public class AddStatus{
    public int hp;
    public int attack;
    public int speed;
    public String name;

}

