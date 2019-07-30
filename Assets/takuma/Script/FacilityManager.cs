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
    private List<GameObject> enemylist;
    private Facility fInfo; //自分の施設情報


    void Start()
    {
      time = 0.0f;
      atkObj = (GameObject)Resources.Load("takuma/Prefabs/AttackObj");
      enemylist = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGenerate)return;

        time += Time.deltaTime;

        if(time >= atkInterval){
          time = 0;
          Attack();
        }

    }

    public void Attack(){
      for(int i=0;i<enemylist.Count;i++){
        GameObject obj = enemylist[i];
        float distance = Vector3.Distance(obj.transform.position,transform.position);
        //敵キャラとの距離が範囲内だったら攻撃
        if(distance <= fInfo.attackpos.z/2){
          Debug.Log("attack");
          //targetの方に少しずつ向きが変わる
		    //  transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (obj.transform.position - transform.position), 0.009f);
          Vector3 v = new Vector3(obj.transform.position.x,transform.position.y,obj.transform.position.z);
          transform.LookAt(v);
          GameObject atkobj = Instantiate(atkObj,transform.position,Quaternion.identity) as GameObject;

        }
      }
    }

    public void Generate(Vector3 pos,Vector3 scale,Facility f){
      isGenerate = true;

      GameObject atkpre = (GameObject)Resources.Load("takuma/Prefabs/AtkCheck");
      GameObject atkcheck = Instantiate(atkpre,pos,Quaternion.identity) as GameObject;
      atkcheck.transform.position = pos;
      atkcheck.transform.localScale = scale;
      atkcheck.transform.parent = this.gameObject.transform;
      fInfo = f;
    }

    public void EnemyOnArea(GameObject obj){
      if(obj.CompareTag("Enemy") && !enemylist.Contains(obj)){
        enemylist.Add(obj);
      }
    }
}
