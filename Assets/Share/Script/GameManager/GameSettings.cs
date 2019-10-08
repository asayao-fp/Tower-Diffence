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
    private  int limitTime;

    //召喚コストの上限
    [SerializeField]
    private int maxcost;

    [SerializeField]
    private bool isLight;

    [SerializeField]
    private byte battleType; //攻撃側か守備側か 0 : statue , 1 : gobrin

    AddStatus[] addStatuses;
    int skillType;
    void Start()
    {
        DontDestroyOnLoad(this);
        addStatuses = new AddStatus[5];
        skillType = 0;
        isLog = log;
    }


    public int getLimitTime(){
        return limitTime;
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

    public bool isStatue(){
        return battleType == 0;
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

}

//スキル割り振りなどで追加されるステータス
public class AddStatus{
    public int hp;
    public int attack;
    public int speed;
    public String name;

}

