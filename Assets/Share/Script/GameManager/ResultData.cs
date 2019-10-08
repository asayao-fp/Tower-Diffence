using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultData : MonoBehaviour
{  
    private bool resulttype; //0 -> 勝利 1 -> 敗北
    private int exp; //取得経験値

    void Start(){
        DontDestroyOnLoad(this);
    }
    public void SetResult(bool result,int exp){
        resulttype = result;
        this.exp = exp;

        Debug.Log("setresult : " + result + " " + exp);
    }

    public bool getResultType(){
        return resulttype;
    }

    public int getExp(){
        return exp;
    }

}
