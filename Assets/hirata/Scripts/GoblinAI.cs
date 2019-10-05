using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class GoblinAI : MonoBehaviour
{
    private float timeleft = 0;
    private int generatoCount = 0;

    [Tooltip("生成する際の性格")]
    [SerializeField]
    private GoblinPersonality personality;

    [Tooltip("生成する際の好み")]
    [SerializeField]
    private GoblinType favorite;

    //AI側のコスト
    [SerializeField]
    private float cost = 0;
    [SerializeField]
    private const float maxCost = 100;
    [SerializeField]
    private int[] goblinCost = { 10, 15, 20 };

    [SerializeField]
    private int costUpSpeed = 2;

    [Tooltip("頭の良さ")]
    [SerializeField]
    private double IQ;

    public GameObject[] goblins = new GameObject[3];

    private StageCostManager scm;

    // Start is called before the first frame update
    void Start()
    {
        scm = this.GetComponent<StageCostManager>();

        //平均50、標準偏差10で、いわゆる偏差値
        IQ = Normal(50, 10);
        Debug.Log("IQ = " + IQ);
    }

    // Update is called once per frame
    void Update()
    {
        //大体1秒ごとにコスト増加とゴブリン生成判定
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

            GeneratorAlgolithm(personality, favorite, generatoCount);

            if (generatoCount >= 10)
            {
                generatoCount = 0;
            }
        }

    }

    void GeneratorAlgolithm(GoblinPersonality personality, GoblinType favorite, int count)
    {
        switch (personality)
        {
            case GoblinPersonality.Aggressive:
                AggressiveAlgolithm(favorite, count);
                break;

            case GoblinPersonality.Negative:
                NegativeAlgolithm(favorite, count);
                break;

            case GoblinPersonality.Random:
                RandomAlgolithm(favorite, count);
                break;

            case GoblinPersonality.flexible:
                FlexibleAlgolithm(favorite, count);
                break;

            default:
                Debug.LogError("エラー。誤った値が入っている可能性があります。");
                break;
        }
    }

    //一気に出すタイプのアルゴリズム
    void AggressiveAlgolithm(GoblinType favorite, int count)
    {
        if (count % 4 != 0)
        {
            return;
        }


        for (int i = 0; ; i++)
        {
            float percent = Mathf.Pow(cost, 2) / Mathf.Pow(maxCost, 2) * 100;
            if (Probability(percent))
            {
                GeneratGoblin(favorite);
            }
            else
            {
                break;
            }
            //Debug.Log(i);
        }

    }

    //小出しにするタイプのアルゴリズム
    void NegativeAlgolithm(GoblinType favorite, int count)
    {
        if (count % 2 != 0)
        {
            return;
        }

        float percent = cost / maxCost * 100;
        if (Probability(percent))
        {
            GeneratGoblin(favorite);
        }

    }

    //ランダムなタイプのアルゴリズム
    void RandomAlgolithm(GoblinType favorite, int count)
    {
        if (Probability(50))
        {
            AggressiveAlgolithm(favorite, count);
        }
        else
        {
            NegativeAlgolithm(favorite, count);
        }
    }

    //プレイヤーに合わせるタイプのアルゴリズム
    void FlexibleAlgolithm(GoblinType favorite, int count)
    {
        GeneratGoblin(favorite);

    }

    void GeneratGoblin(GoblinType favorite)
    {
        GoblinType goblinType = DecideGoblinType(favorite);
        int lineSize = scm.line.Count;
        int minCost = 0;
        int generateLine = 0;

        for (int i = 0; i < lineSize; i++)
        {

            int calCost = scm.caliculateCost(i);
            minCost = scm.caliculateCost(0);

            if (minCost > calCost)
            {

                minCost = calCost;
                generateLine = i;
            }
        }

        goblins[(int)goblinType].GetComponent<GobMane>().setLine(generateLine);
        Instantiate(goblins[(int)goblinType]);
        cost -= goblinCost[(int)favorite];
    }

    //生成するゴブリンの種類を決定する
    GoblinType DecideGoblinType(GoblinType favorite)
    {
        double favoriteProbability = 100 - IQ;
        GoblinType retVal = favorite;

        if (favoriteProbability < 33.3)
        {
            favoriteProbability = 33.4;
        }

        double elseProbability = (100 - favoriteProbability) / 2;

        if (Probability((float)favoriteProbability))
        {
            return retVal;
        }
        else if (Probability((float)elseProbability))
        {
            retVal += 1;
            if (retVal == GoblinType.Num)
            {
                retVal = 0;
            }
            return retVal;
        }
        else
        {
            retVal += 2;
            if (retVal == GoblinType.Num)
            {
                retVal = 0;
            }
            else if (retVal > GoblinType.Num)
            {
                retVal = (GoblinType)1;
            }

            return retVal;
        }

    }


    public static bool Probability(float percent)
    {
        float probabilityRate = UnityEngine.Random.value * 100.0f;

        if (probabilityRate < percent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //正規乱数を生成する。ex=平均値、sd=標準偏差
    double Normal(double ex, double sd)
    {
        double xw = 0.0;
        double x;
        int n;
        for (n = 1; n <= 12; n++)
        {        /* 12個の一様乱数の合計 */
            xw = xw + Random.Range(0f, 1.0f);
        }
        x = sd * (xw - 6.0) + ex;
        return x;
    }

}
