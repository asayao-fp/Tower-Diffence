using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateCostManager : MonoBehaviour
{
    public GameObject generatecostbar;
    private float time;
    private int maxcost; //コスト上限
    private int cost; //現在のコスト
    float fill;
    GameProgress gp;

    void Start()
    {
        time = 0.0f;
        generatecostbar.GetComponent<Image>().fillAmount = 0;
        fill = generatecostbar.GetComponent<Image>().fillAmount;
        fill = 0;
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        maxcost = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>().getMaxCost();
    }

    void Update()
    {
        if(gp.getStatus() != gp.NOW_GAME)return;

        time += Time.deltaTime;
        if(time > maxcost)time = maxcost;
        fill = time / maxcost ;
        cost = (int)time;

        generatecostbar.GetComponent<Image>().fillAmount = fill;
    }

    public int getCost(){
        return cost;
    }

    public void addCost(float cost){
        if(gp.getStatus() != gp.NOW_GAME)return;

        time += cost;
        if(time > maxcost){
            time = maxcost;
        }
    }

    public void generateCost(float cost){
        time -= cost;
        if(time <= 0){
            time = 0;
        }
    }
}
