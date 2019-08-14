using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Effekseer;

public class FacilityManager : MonoBehaviour
{
    public Boolean isGenerate = false; //設置されているか
    public float atkInterval; //攻撃間隔
    public float time;
    public float deletetime;
    public GameObject atkObj;
    private List<GameObject> enemylist;
    public Facility fInfo; //自分の施設情報
    private GameObject Gene,Atk; //攻撃、召喚時のエフェクト用オブジェクト
    private Boolean isGene,isAtk; //エフェクトを使用したかどうかのフラグ
    private GameProgress gp;
    

    void Start()
    {
      time = 0.0f;
      atkObj = (GameObject)Resources.Load("takuma/Prefabs/AttackObj");
      Gene = (GameObject)Resources.Load("takuma/Prefabs/Generate");
      Atk = (GameObject)Resources.Load("takuma/Prefabs/Atk");
      enemylist = new List<GameObject>();

      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
    }

    // Update is called once per frame
    void Update()
    {

        if(gp.getStatus() != gp.NOW_GAME)return;

        if(!isGenerate)return;

        deletetime += Time.deltaTime;

        if(deletetime > fInfo.time){
          Debug.Log("delete");
          Destroy(transform.root.gameObject);
        }else{
          if(!isGene){
            Gene.transform.position = transform.position;
            Atk.transform.position = transform.position;
            EffekseerEmitter ee = Gene.GetComponent<EffekseerEmitter>();
            EffekseerEffectAsset ea = ee.effectAsset;
            ee.Play(ea);
            isGene = true;
          }

          time += Time.deltaTime;
          

          if(time >= atkInterval){
            time = 0;
            Attack();
          }
        }

    }

    public void Attack(){
      for(int i=0;i<enemylist.Count;i++){
        GameObject obj = enemylist[i];
        float distance = Vector3.Distance(obj.transform.position,transform.position);
        //敵キャラとの距離が範囲内だったら攻撃
/*
        if(distance <= fInfo.attackpos.z/2){
          Debug.Log("attack");
          EffekseerEmitter ee = Atk.GetComponent<EffekseerEmitter>();
          EffekseerEffectAsset ea = ee.effectAsset;
          ee.Play(ea);
          //targetの方に少しずつ向きが変わる
		    //  transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (obj.transform.position - transform.position), 0.009f);
          Vector3 v = new Vector3(obj.transform.position.x,transform.position.y,obj.transform.position.z);
          transform.LookAt(v);
          GameObject atkobj = Instantiate(atkObj,transform.position,Quaternion.identity) as GameObject;

        }
        */
      }
    }

    public void Generate(Vector3 pos,Vector3 scale,Facility f){
      isGenerate = true;

     // GameObject ppre = (GameObject)Resources.Load("takuma/Prefabs/GenerateParticle");
    //  Instantiate(ppre,new Vector3(pos.x,pos.y-0.05f,pos.z),Quaternion.identity);

      GameObject atkpre = (GameObject)Resources.Load("takuma/Prefabs/AtkCheck");
      GameObject atkcheck = Instantiate(atkpre,pos,Quaternion.identity) as GameObject;
      atkcheck.transform.position = pos;
      atkcheck.transform.localScale = scale;
      atkcheck.transform.parent = this.gameObject.transform;
      fInfo = f;
      deletetime = 0;


      if(gp == null){
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
      }
      gp.Generate(this.gameObject);

    }

    public void EnemyOnArea(GameObject obj){

      if(obj.CompareTag(Constants.GOBLIN_TAG) && !enemylist.Contains(obj)){
        enemylist.Add(obj);
      }
    }
}
