using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class UserBaggage : MonoBehaviour{
    public static String EXPUP_S = "expup_small";
    public static String EXPUP_M = "expup_midium";
    public static String EXPUP_B = "expup_big";
    public static String EXPBOOST_S = "expboost_small";
    public static String EXPBOOST_M = "expboost_midium";
    public static String EXPBOOST_B = "expboost_big";
    public static String LEVELMAX_U = "levelmax_up";
    public static String GACHA = "gacha";
    /** 所持数を持たせる*/
    private int EXPUP_SMALL; // 0
    private int EXPUP_MIDIUM; // 1
    private int EXPUP_BIG; // 2
    private int EXPBOOST_SMALL; // 3
    private int EXPBOOST_MIDIUM; // 4
    private int EXPBOOST_BIG; // 5
    private int LEVELMAX_UP; // 6
    private int GACHA_ITEM; // 7

    public void addItem(int type,int num){
         switch(type){
            case 0:
                EXPUP_SMALL += num;
                break;
            case 1:
                EXPUP_MIDIUM += num;
                break;
            case 2:
                EXPUP_BIG += num;
                break;
            case 3:
                EXPBOOST_SMALL += num;
                break;
            case 4:
                EXPBOOST_MIDIUM += num;
                break;
            case 5:
                EXPUP_BIG += num;
                break;
            case 6:
                LEVELMAX_UP += num;
                break;
            case 7:
                GACHA_ITEM += num;
                break;
        }

    }

    public void useItem(int type){
         switch(type){
            case 0:
                EXPUP_SMALL--;
                break;
            case 1:
                EXPUP_MIDIUM--;
                break;
            case 2:
                EXPUP_BIG--;
                break;
            case 3:
                EXPBOOST_SMALL--;
                break;
            case 4:
                EXPBOOST_MIDIUM--;
                break;
            case 5:
                EXPUP_BIG--;
                break;
            case 6:
                LEVELMAX_UP--;
                break;
            case 7:
                GACHA_ITEM--;
                break;
        }

    }

    public int getItem(int type){
        switch(type){
            case 0:
                return EXPUP_SMALL;
            case 1:
                return EXPUP_MIDIUM;
            case 2:
                return EXPUP_BIG;
            case 3:
                return EXPBOOST_SMALL;
            case 4:
                return EXPBOOST_MIDIUM;
            case 5:
                return EXPBOOST_BIG;
            case 6:
                return LEVELMAX_UP;
            case 7:
                return GACHA_ITEM;
        }
        return 0;
    }

}
