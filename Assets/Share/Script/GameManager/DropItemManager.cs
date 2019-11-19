using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{

    //取得できるアイテムを設定
    public int[] dropItem1 = {0,1,2,7};

    public Dictionary<int,float> dropDict1;
    void Awake(){
        dropDict1 = new Dictionary<int,float>();

        dropDict1.Add(0,50.0f);
        dropDict1.Add(1,35.0f);
        dropDict1.Add(2,10.0f);
    }
    public int getDropItem1(){
        float rand = Random.value * 100;

        foreach(KeyValuePair<int,float> elem in dropDict1){
            if(rand < elem.Value){
                return elem.Key;
            }else{
                rand -= elem.Value;
            }
        }
        return 0;
    }
}
