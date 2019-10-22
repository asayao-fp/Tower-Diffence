using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public static class SoundManager
{

    public static void SoundPlay(string name,string classname){
        GameObject sound = ResourceManager.getObject("Sounds/" + name);
        if(sound == null){
            GameSettings.printLog("[SoundManager] sound is null : " + name + " classname : " + classname);
            return;
        }
        GameObject obj = MonoBehaviour.Instantiate(sound,sound.transform.position,Quaternion.identity) as GameObject;
        obj.tag = "sound";
        CriAtomSource audio = obj.GetComponent<CriAtomSource>();
        audio.Play();

    }
}
