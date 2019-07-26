using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateManager : MonoBehaviour
{

    [SerializeField]
    private int generateType;

    private DrawPosition dp;
    private FacilityPosition fp;
    void Start()
    {
        dp = GameObject.Find("StageSelect").GetComponent<DrawPosition>();
        fp = GameObject.Find("StageSelect").GetComponent<FacilityPosition>();
    }

    void Update()
    {

    }

    public void GenerateTower(int type){
      if(generateType != type){
        generateType = type;
        dp.setType = fp.getFacilityList(type).settype;
        dp.isShow = true;
      }
    }
}
