using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ShowStagePosition : MonoBehaviour
{
    public static int MAXSETTYPE = 4;

    public void showSetPosition(int[] num){

      for(int i=0;i<transform.childCount;i++){
        Transform child = transform.GetChild(i);
        if(child.tag.StartsWith("Type_")){
          int type = int.Parse(child.tag.Substring(5));
          child.gameObject.SetActive(false);
          for(int j=0;j<num.Length;j++){
            if(num[j] == type){
              child.gameObject.SetActive(true);
              break;
            }            
          }
        }
      }
    }

    public void hideSetPosition(){
      for(int i=0;i<transform.childCount;i++){
        Transform child = transform.GetChild(i);
        if(child.tag.StartsWith("Type_")){
          child.gameObject.SetActive(false);
        }
      }
    }
}
