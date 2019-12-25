using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCostManager : MonoBehaviour
{
    [System.SerializableAttribute]
    public class lineList
    {
        public List<GameObject> List = new List<GameObject>();

        public lineList(List<GameObject> list)
        {
            List = list;
        }
    }

    [SerializeField]
    private List<lineList> lineCost = new List<lineList>();

    [SerializeField]
    public List<lineList> line = new List<lineList>();

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int caliculateCost(int lineNum)
    {
        int sumCost = 0;

        foreach (GameObject item in lineCost[lineNum].List)
        {
            sumCost += item.GetComponent<StageCost>().getCost();
        }

        return sumCost;
    }
}
