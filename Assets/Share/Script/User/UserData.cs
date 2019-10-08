using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserData : MonoBehaviour
{
    public static String USERDATA_ID = "user_id"; //ユーザーID
    public static String USERDATA_NAME = "user_name"; //ユーザの名前
    public static String USERDATA_LEVEL = "user_level"; //ユーザの現在のレベル
    public static String USERDATA_EXP = "user_exp"; //現在の累計経験値

    public void init(){
      // PlayerPrefs.SetString(USERDATA_ID,"test");
      //  PlayerPrefs.SetString(USERDATA_NAME,"takuma");
      //  PlayerPrefs.SetInt(USERDATA_LEVEL,UnityEngine.Random.Range(1,10));
      //  PlayerPrefs.SetInt(USERDATA_EXP,0);

    }
 
}

