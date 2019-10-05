using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ResourceManager
{
    public static String LIGHT_PASS = "Light/";
    public static String DARK_PASS = "Dark/";

    //オブジェクト読み込み Light or Dark
    public static GameObject getObject(String pass,bool isLight){
        GameObject obj = (GameObject)Resources.Load((isLight ? LIGHT_PASS : DARK_PASS) + pass) as GameObject;
        return obj;
    }

    //上記以外のオブジェクト読み込み
    public static GameObject getObject(String path){
        GameObject obj = (GameObject)Resources.Load(path) as GameObject;
        return obj;
    }
    
}
