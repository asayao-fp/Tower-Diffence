using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class expData : MonoBehaviour
{
    private int[] exp;

    void Awake(){
        DontDestroyOnLoad(this);

        exp = new int[100];
        //最初は1レベルだとして、exp[0] -> レベル2に必要な経験値
        for(int i=0;i<exp.Length;i++){
            exp[i] = (int)Mathf.Pow(i+1,3);
        }
    }

    //指定したレベルに必要な経験値(2~)
    public int getExp(int level){
        return exp[level-2];
    }

    //現在のレベルから指定したレベルに必要な経験値
    public int getExp(int level,int nowlevel){
        return exp[level-2] - exp[nowlevel-2];
    }

    //経験値から現在のレベルを求める
    public int getLevel(int exp){
        int level = 1;
        for(int i=0;i<this.exp.Length;i++){
            if(this.exp[i] > exp){
                level = i+2;
                break;
            }
        }
        return level;
    }
}
