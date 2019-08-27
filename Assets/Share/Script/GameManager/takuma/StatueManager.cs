using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Effekseer;

public class StatueManager : FacilityManager
{
    public Boolean isGenerate = false; //設置されているか
    public float atkInterval; //攻撃間隔
    public float time;
    public float deletetime;
    public GameObject atkObj;
    protected List<GameObject> enemylist;
    protected Facility fInfo; //自分の施設情報
    protected GameObject Gene;
    public GameObject Atk; //攻撃、召喚時のエフェクト用オブジェクト
    protected Boolean isGene,isAtk; //エフェクトを使用したかどうかのフラグ
    protected GameProgress gp;    
    protected int obj_num; //オブジェクトのユニークid
    public Boolean isDebug;
    protected FacilitySetting fs;
    public Image hpbar;

    private bool isAMM; //AttackMakeManagerか

    void Start()
    {
        GameObject amm = GameObject.Find("AttackMakeManager");
        if(amm != null){
            isAMM = true;
            return;
        }
        time = 0.0f;
        atkObj = (GameObject)Resources.Load("takuma/Prefabs/AttackObj");
        Gene = (GameObject)Resources.Load("takuma/Prefabs/Generate");
        enemylist = new List<GameObject>();

        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
  

        if(hpbar != null){
          hpbar.transform.position = new Vector3(this.transform.position.x ,this.transform.position.y + 0.3f,this.transform.position.z);
          hpbar.fillAmount = 1;
        }
    }

    void Update()
    {
        if(isAMM)return;
        
        if(gp.getStatus() != gp.NOW_GAME)return;

        if(!isGenerate)return;

        fInfo = gp.getFM(obj_num,isDebug);

        deletetime += Time.deltaTime;

        if(deletetime > fInfo.time){
          gp.Dead(obj_num);
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
            Atk.GetComponent<AttackManager>().Attack();
            time = 0;
            Attack();
          }

          if(hpbar != null){
            hpbar.fillAmount = (float)(fInfo.time - deletetime) / (float)fInfo.time;
          }
        }        
    }

    public override void Attack(){
      for(int i=0;i<enemylist.Count;i++){
        if(enemylist[i] == null){
          continue;
        }
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

     public override　void Generate(Vector3 pos,Vector3 scale,Facility f){

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

      isGenerate = true;
    }
    
    public override　void EnemyOnArea(GameObject obj){

      if(obj.CompareTag(Constants.GOBLIN_TAG) && !enemylist.Contains(obj)){
        enemylist.Add(obj);
      }
    }

    public override　void setId(int id){
      this.obj_num = id;
    }

    public override　void addHP(int hp){
      gp.AddHP(obj_num,hp,isDebug);
    }

    public override void Dead(){
      Destroy(this.gameObject);
    }

    public override Facility getFinfo(){
        return fInfo;
    }
}
