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

    // Update is called once per frame
    void Update()
    {
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
        goblinHP = tmpHP;
        StatueType st = DecideStatueType(goblinHP);


        Debug.Log(position);
        //生成コストが足りていたら生成
        if (STATUE_COST[(int)st] <= this.cost)
        {
            Debug.Log(st);
            this.cost -= STATUE_COST[(int)st];

            Quaternion q = new Quaternion();
            q = Quaternion.identity;
            Instantiate(statues[(int)st], position, q);
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
