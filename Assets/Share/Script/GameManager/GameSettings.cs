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

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

