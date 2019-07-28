using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStagePosition : MonoBehaviour
{
    public static int MAXSETTYPE = 4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showSetPosition(int t){
      for(int i=0;i<transform.childCount;i++){
        Transform child = transform.GetChild(i);
        if(child.tag.StartsWith("Type_")){
          int type = int.Parse(child.tag.Substring(5));
          if(type <= t){
            child.gameObject.SetActive(true);
          }else{
            child.gameObject.SetActive(false);
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
