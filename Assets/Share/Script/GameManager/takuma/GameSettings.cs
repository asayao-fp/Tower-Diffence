using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

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
}

