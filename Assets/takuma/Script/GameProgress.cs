using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    private List<GameObject> sg_objs;
    FacilitySetting fs;



    void Start()
    {
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      sg_objs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
      //GetInstanceID
    }

    //死んだ
    public void Dead(GameObject obj){

    }

    //召喚された
    public void Generate(GameObject obj){
      Debug.Log("generate");

    }

    //攻撃受けた
    public void AddHP(int serial_id,int hp){

    }

}
