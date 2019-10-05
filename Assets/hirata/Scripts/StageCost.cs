using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCost : MonoBehaviour
{
    [SerializeField]
    private int cost = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addCost(int cost)
    {
        this.cost += cost;
    }

    public int getCost()
    {
        return this.cost;
    }

}
