using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class StatueAI : MonoBehaviour
{
    private float timeleft = 0;
    private int generatoCount = 0;
    public int tmpHP = 100;

    //AI側のコスト
    [SerializeField]
    private float cost = 0;
    [SerializeField]
    private const float maxCost = 100;

    [SerializeField]
    private int costUpSpeed = 2;

    public GameObject[] statues = new GameObject[5];

    private StageCostManager scm;

    private bool isAi;

    private GameProgress gp;


    void Start(){

        //プレイヤーがStatueだったら動作しない
        if(GameObject.Find("StaticManager").GetComponent<GameSettings>().isStatue()){
            isAi = true;
        }else {
            isAi = false;
        }
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();

    }
    // Update is called once per frame
    void Update()
    {
        if(isAi){
            return;
        }
        
        //大体1秒ごとにコスト増加
        timeleft -= Time.deltaTime;
        if (timeleft <= 0.0)
        {
            timeleft = 1.0f;
            generatoCount++;

            if (cost < maxCost)
            {
                cost += costUpSpeed;
                if (cost > maxCost)
                {
                    cost = maxCost;
                }
            }
        }

    }


    public void GenerateStatue(int goblinHP, Vector3 position, GameObject childObject)
    {

        if(isAi){
            return;
        }

        goblinHP = tmpHP;
        StatueType st = DecideStatueType(goblinHP);


        //生成コストが足りていたら生成
        if (STATUE_COST[(int)st] <= this.cost)
        {
            this.cost -= STATUE_COST[(int)st];

            Quaternion q = new Quaternion();
            q = Quaternion.identity;

            gp.Generate(statues[(int)st].name,position,true,true);
            childObject.GetComponent<SearchGoblin>().setStatueExist(true);
        }

    }

    StatueType DecideStatueType(int goblinHP)
    {
        int retVal = 0;
        int i = 0;
        foreach (int item in STATUE_COST)
        {
            retVal = i;
            if (goblinHP <= item * 10)
            {
                break;
            }
            i++;
        }

        return (StatueType)retVal;
    }

}
