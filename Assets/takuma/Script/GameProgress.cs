using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    private List<GameObject> sg_objs;
    FacilitySetting fs;

    //てて手手手手手手手ててててた；l代田橋駅出ました💨sfじゃs；lf.u.pooler@icloud.comじゃkdlfじゃs；ljふぁdsl；f.u.pooler@icloud.comjdさl；kfdさhl；f.u.pooler@icloud.coml；代田橋駅出ました💨さl；ファ代田橋駅出ました💨ls；fl；亜sfl；

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
