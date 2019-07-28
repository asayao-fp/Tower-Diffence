using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FacilityManager : MonoBehaviour
{
    public Boolean isGenerate = false; //設置されているか
    public float atkInterval; //攻撃間隔
    public float time;
    public GameObject atkObj;
    // Start is called before the first frame update
    void Start()
    {
      time = 0.0f;
      atkObj = (GameObject)Resources.Load("takuma/Prefabs/f_atk");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGenerate)return;

        time += Time.deltaTime;

        if(time >= atkInterval){
          Debug.Log("[Facility] Attack");
          time = 0;
          GameObject parobj = Instantiate(atkObj,transform.forward,Quaternion.identity) as GameObject;
          parobj.GetComponent<ParticleSystem>().Play();
        }

    }

    public void Generate(Vector3 pos,Vector3 scale){
      isGenerate = true;


      GameObject atkpre = (GameObject)Resources.Load("takuma/Prefabs/AtkCheck");
      GameObject atkcheck = Instantiate(atkpre,pos,Quaternion.identity) as GameObject;
      atkcheck.transform.position = pos;
      atkcheck.transform.localScale = scale;
      atkcheck.transform.parent = this.gameObject.transform;

    }
}
